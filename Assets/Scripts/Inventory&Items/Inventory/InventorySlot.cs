using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class Inventory_Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField] Item Item;
    [field: SerializeField] internal int SlotIndex { get; private set; }

    [field: SerializeField] Image ItemImage; // I was way too lazy to fix teh thing commented in the Start() method
    [field: SerializeField] TMP_Text ItemCountText;
    [field: SerializeField] bool isHotbarSlot = false;
    [field: SerializeField] Color FocusedColor = Color.blue;
    [field: SerializeField] Color UnFocusedColor = Color.black;


    bool is_dragging = false;
    Vector3 _ItemImageInitialPosition;
    Vector3 _ItemCountInitialPosition;
    Image background;

    int _ItemCount = 0;
    internal int ItemCount => _ItemCount;
    internal bool IsAvailable => (this.Item == null || this.ItemCount < 10);

    void Start()
    {
        //Transform childTransform = transform.GetChild(0);
        //ItemImage = childTransform.GetComponentInChildren<Image>();
        _ItemImageInitialPosition = ItemImage.transform.localPosition;
        _ItemCountInitialPosition = ItemCountText.transform.localPosition;
        background = GetComponent<Image>();
        if (isHotbarSlot)
            GameManager.game_manager.player_inventory.AddToSlotChangedAction(() => { SetBackground(UnFocusedColor); });
    }

    void FixedUpdate()
    {
        if (is_dragging)
        {
            ItemImage.transform.position = Input.mousePosition;
            ItemCountText.transform.position = Input.mousePosition + new Vector3(-19, 2.2f, 0);
        }

        if (Item != null)
            if (_ItemCount <= 0)
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
            ItemImage.transform.localPosition = _ItemImageInitialPosition;
            ItemCountText.transform.localPosition = _ItemCountInitialPosition;
            Swap_Slots();
        }
    }

    private void Swap_Slots()
    {
        Inventory_Slot closest_slot = GameManager.game_manager.player_inventory.Return_Closest_Slot();
        Item current_Item = this.Item;
        int _currentCount = this._ItemCount;

        if(closest_slot.Is_Empty())
        {
            closest_slot.Assign_Item(current_Item, _currentCount);
            this.Clear_Item();
            if (Tutorial.TutorialInstance != null)
                Tutorial.TutorialInstance.SwappedSlots();
        }
        else if(Is_Current_Slot(closest_slot))
            is_dragging = false;
        else
        {
            this.Assign_Item(closest_slot.Get_Item(), closest_slot.ItemCount);
            closest_slot.Assign_Item(current_Item, _currentCount);
            is_dragging = false;

            if (Tutorial.TutorialInstance != null)
                Tutorial.TutorialInstance.SwappedSlots();
        }
    }
    internal void Equip()
    {
        GameManager.game_manager.player_inventory.Equip(Item);
        SetBackground(FocusedColor);
    }
    private void SetBackground(Color c)
    {
        background.color = c;
    }
    internal bool Is_Empty()
    {
        return (this.Item == null);
    }
    
    internal void Assign_Item(Item Item, int count = 1)
    {
        if (Item != null)
        {
            if (this.Item == null)
            {
                this.Item = Item;
                this._ItemCount = count;
                Item.AssignedSlot = this;
            }
            else if (this.Item == Item)
                this._ItemCount += count;
            else
                Debug.Log($"{this.Item.name} | {Item.name}");
        }
        else
            Debug.LogError("Item is null!");
    }
    internal void DecreaseCount(int _count = 1)
    {
        _ItemCount -= _count;
        if (_ItemCount <= 0)
        {
            Clear_Item();
        }
    }
    internal void Clear_Item()
    {
        if (Item != null)
            Item.AssignedSlot = null;

        this.Item = null;
        _ItemCount = 0;
    }

    internal void DropItem()
    {
        DecreaseCount();
       
    }
    internal void Update_UI()
    {
        if(this.Item != null)
        {
            if (!ItemCountText.gameObject.activeInHierarchy && Item.IsStackable)
                ItemCountText.gameObject.SetActive(true);
            else if (ItemCountText.gameObject.activeInHierarchy && !Item.IsStackable)
                ItemCountText.gameObject.SetActive(false);

            ItemImage.sprite = Item.ItemIcon;
            ItemCountText.text = $"{_ItemCount}";
            ItemImage.color = new Color(255, 255, 255, 255);
        }
        else
        {
            ItemImage.sprite = null;
            ItemCountText.text = $"";
            ItemImage.color = new Color(0, 0, 0, 0);
        }
    }
    internal Item Get_Item()
    {
        if (this.Item == null)
            return null;
        return (this.Item);
    }
    internal bool Is_Current_Slot(Inventory_Slot compared_slot)
    {
        return (compared_slot == this);
    }
}