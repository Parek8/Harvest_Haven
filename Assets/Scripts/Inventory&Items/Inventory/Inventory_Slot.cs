using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class Inventory_Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField] Item item;
    [field: SerializeField] internal int slot_index { get; private set; }

    [field: SerializeField] Image item_image; // I was way too lazy to fix teh thing commented in the Start() method
    [field: SerializeField] TMP_Text item_count;
    [field: SerializeField] bool isHotbarSlot = false;
    [field: SerializeField] Color focusedColor = Color.blue;
    [field: SerializeField] Color unfocusedColor = Color.black;


    bool is_dragging = false;
    Vector3 item_image_initial_position;
    Vector3 item_count_initial_position;
    Image background;

    int _itemCount = 0;
    internal int ItemCount => _itemCount;
    internal bool IsAvailable => (this.item == null || this.ItemCount < 10);

    void Start()
    {
        //Transform childTransform = transform.GetChild(0);
        //item_image = childTransform.GetComponentInChildren<Image>();
        item_image_initial_position = item_image.transform.localPosition;
        item_count_initial_position = item_count.transform.localPosition;
        background = GetComponent<Image>();
        if (isHotbarSlot)
            GameManager.game_manager.player_inventory.AddToSlotChangedAction(() => { SetBackground(unfocusedColor); });
    }

    void FixedUpdate()
    {
        if (is_dragging)
        {
            item_image.transform.position = Input.mousePosition;
            item_count.transform.position = Input.mousePosition + new Vector3(-19, 2.2f, 0);
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
                is_dragging = true;
        if (eventData.button == PointerEventData.InputButton.Right && isHotbarSlot)
            Equip();
    }
    internal float Return_Distance_From_Mouse()
    {
        return (Vector2.Distance(transform.position, Input.mousePosition));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            is_dragging = false;
            item_image.transform.localPosition = item_image_initial_position;
            item_count.transform.localPosition = item_count_initial_position;
            Swap_Slots();
        }
    }

    private void Swap_Slots()
    {
        Inventory_Slot closest_slot = GameManager.game_manager.player_inventory.Return_Closest_Slot();
        Item current_item = this.item;
        int _currentCount = this._itemCount;

        if(closest_slot.Is_Empty())
        {
            closest_slot.Assign_Item(current_item, _currentCount);
            this.Clear_Item();
            if (Tutorial.TutorialInstance != null)
                Tutorial.TutorialInstance.SwappedSlots();
        }
        else if(Is_Current_Slot(closest_slot))
            is_dragging = false;
        else
        {
            this.Assign_Item(closest_slot.Get_Item(), closest_slot.ItemCount);
            closest_slot.Assign_Item(current_item, _currentCount);
            is_dragging = false;

            if (Tutorial.TutorialInstance != null)
                Tutorial.TutorialInstance.SwappedSlots();
        }
    }
    internal void Equip()
    {
        GameManager.game_manager.player_inventory.Equip(item);
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
            Debug.LogError("Item is null!");
    }
    internal void DecreaseCount(int _count = 1)
    {
        _itemCount -= _count;
        if (_itemCount <= 0)
            Clear_Item();
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
        if(this.item != null)
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