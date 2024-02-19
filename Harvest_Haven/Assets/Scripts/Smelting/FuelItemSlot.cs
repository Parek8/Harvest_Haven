using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class FuelItemSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] internal int SlotIndex { get; private set; }
    [field: SerializeField] FuelItemMenuBehaviour ItemMenu;
    [field: SerializeField] Item item;
    [field: SerializeField] TMP_Text countLabel;
    [field: SerializeField] UI_Behaviour SmeltSlot;
    internal int count { get; private set; }

    Image item_image;
    private void Start()
    {
        item_image = GetComponent<Image>();
    }
    void FixedUpdate() => Update_UI();
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            ShowItemMenu();
    }

    private void ShowItemMenu()
    {
        if (!ItemMenu.is_visible)
            ItemMenu.Show();
        if (SmeltSlot.is_visible)
            SmeltSlot.Hide();

        ItemMenu.AssignSlot(this);
    }

    internal void Clear_Item()
    {
        this.item = null;
        this.count = 0;
    }
    internal void Update_UI()
    {
        if (this.count <= 0)
            Clear_Item();

        if (this.item != null)
        {
            item_image.sprite = item.ItemIcon;
            countLabel.text = $"{count}x";
        }
        else
        {
            item_image.sprite = GameManager.game_manager.Null_Item.ItemIcon;
            countLabel.text = "";
        }
    }
    internal Item Get_Item()
    {
        if (this.item == null)
            return null;
        return (this.item);
    }
    internal void DecreaseCount() => this.count--;
    internal void AssignItem(Item _item)
    {
        if (this.item != null)
            for (int i = 0; i < count; i++)
                GameManager.game_manager.player_inventory.Add(this.item);

        this.item = _item;
        this.count = GameManager.game_manager.player_inventory.GetItemCountInInventory(this.item);

        for (int i = 0; i < count; i++)
            GameManager.game_manager.player_inventory.DecreaseItemCount(this.item);
    }
}