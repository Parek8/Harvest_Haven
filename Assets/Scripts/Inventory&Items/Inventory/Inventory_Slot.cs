using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class Inventory_Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField] Item item;
    [field: SerializeField] internal int slot_index { get; private set; }

    [field: SerializeField] Image item_image; // I was way too lazy to fix the thing commented in the Start() method
    [field: SerializeField] TMP_Text item_count;
    [field: SerializeField] bool isHotbarSlot = false;
    [field: SerializeField] Color focusedColor = Color.blue;
    [field: SerializeField] Color unfocusedColor = Color.black;

    bool is_dragging = false;
    Vector3 item_image_initial_position;
    Vector3 item_count_initial_position;
    Image background;
    Transform visibleParent;
    Camera mainCamera;
    RectTransform canvasRectTransform;
    RectTransform item_image_rectTransform;

    int _itemCount = 0;
    internal int ItemCount => _itemCount;
    internal bool IsAvailable => (this.item == null || this.ItemCount < 10);

    void Start()
    {
        //Transform childTransform = transform.GetChild(0);
        //item_image = childTransform.GetComponentInChildren<Image>();
        visibleParent = GameManager.GameManagerInstance.UIVisibleParent;
        mainCamera = Camera.main;
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        item_image_rectTransform = item_image.GetComponent<RectTransform>();

        item_image_initial_position = item_image.transform.localPosition;
        item_count_initial_position = item_count.transform.localPosition;
        background = GetComponent<Image>();

        if (isHotbarSlot)
            GameManager.GameManagerInstance.PlayerInventory.AddToSlotChangedAction((int index) => { SetBackground((index == slot_index) ? focusedColor : unfocusedColor); });
    }

    void FixedUpdate()
    {
        if (is_dragging)
        {
            Vector3 screenPos = Input.mousePosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, screenPos, mainCamera, out var worldPos);
            item_image_rectTransform.position = worldPos;
        }

        if (item != null)
            if (_itemCount <= 0)
                Clear_Item();

        Update_UI();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            if (!this.Is_Empty())
            {
                is_dragging = true;
                item_image.transform.SetParent(visibleParent);
            }

        if (eventData.button == PointerEventData.InputButton.Right && isHotbarSlot)
            Equip();
    }

    internal float Return_Distance_From_Mouse()
    {
        Vector3 screenPos = Input.mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, screenPos, mainCamera, out var worldPos);
        return Vector3.Distance(transform.position, worldPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            is_dragging = false;
            item_image.transform.SetParent(transform);
            item_image.transform.localPosition = item_image_initial_position;
            item_count.transform.SetParent(transform);
            item_count.transform.localPosition = item_count_initial_position;

            Swap_Slots();
        }
    }

    private void Swap_Slots()
    {
        Inventory_Slot closest_slot = GameManager.GameManagerInstance.PlayerInventory.Return_Closest_Slot();
        if (closest_slot != null && closest_slot != this)
        {
            Item current_item = this.item;
            int _currentCount = this._itemCount;

            if (closest_slot.Is_Empty())
            {
                closest_slot.Assign_Item(current_item, _currentCount);
                this.Clear_Item();
                if (Tutorial.TutorialInstance != null)
                    Tutorial.TutorialInstance.SwappedSlots();
            }
            else
            {
                Item temp_item = closest_slot.item;
                int temp_count = closest_slot.ItemCount;

                closest_slot.Assign_Item(current_item, _currentCount);
                this.Assign_Item(temp_item, temp_count);

                if (Tutorial.TutorialInstance != null)
                    Tutorial.TutorialInstance.SwappedSlots();
            }
        }
    }

    internal void Equip()
    {
        GameManager.GameManagerInstance.PlayerInventory.Equip(item);
        SetBackground(focusedColor);
    }

    private void SetBackground(Color c)
    {
        background.color = c;
    }

    internal bool Is_Empty()
    {
        return (this.item == null);
    }

    internal void Assign_Item(Item item, int count = 1)
    {
        if (item != null)
        {
            if (this.item == null)
            {
                this.item = item;
                this._itemCount = count;
                item.AssignedSlot = this;
            }
            else if (this.item == item)
                this._itemCount += count;
            else
                Debug.Log($"{this.item.name} | {item.name}");
        }
        else
            Debug.LogWarning("Item is null!");
    }

    internal void DecreaseCount(int _count = 1)
    {
        _itemCount -= _count;
        if (_itemCount <= 0)
        {
            Clear_Item();
        }
    }

    internal void Clear_Item()
    {
        if (item != null)
            item.AssignedSlot = null;

        this.item = null;
        _itemCount = 0;
    }

    internal void DropItem()
    {
        DecreaseCount();
    }

    internal void Update_UI()
    {
        if (this.item != null)
        {
            if (!item_count.gameObject.activeInHierarchy && item.IsStackable)
                item_count.gameObject.SetActive(true);
            else if (item_count.gameObject.activeInHierarchy && !item.IsStackable)
                item_count.gameObject.SetActive(false);

            item_image.sprite = item.ItemIcon;
            item_count.text = $"{_itemCount}";
            item_image.color = new Color(255, 255, 255, 255);
        }
        else
        {
            item_image.sprite = null;
            item_count.text = $"";
            item_image.color = new Color(0, 0, 0, 0);
        }
    }

    internal Item Get_Item()
    {
        if (this.item == null)
            return null;
        return (this.item);
    }

    internal bool Is_Current_Slot(Inventory_Slot compared_slot)
    {
        return (compared_slot == this);
    }
}
