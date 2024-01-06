using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class FuelItemSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] internal int SlotIndex { get; private set; }
    [field: SerializeField] FuelItemMenuBehaviour ItemMenu;
    [field: SerializeField] Item item;

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

        ItemMenu.AssignSlot(this);
    }

    internal void Clear_Item() => this.item = null;
    internal void Update_UI()
    {
        if (this.item != null)
            item_image.sprite = item.ItemIcon;
        else
            item_image.sprite = null;
    }
    internal Item Get_Item()
    {
        if (this.item == null)
            return null;
        return (this.item);
    }
    internal void AssignItem(Item _item)
    {
        this.item = _item;
    }
}