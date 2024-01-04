using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmeltFuelItemMenu : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public int SlotIndex { get; private set; }
    [field: SerializeField] SmeltingFuelItemBehaviour ItemMenu;
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

    public void Clear_Item() => this.item = null;
    public void Update_UI()
    {
        if (this.item != null)
            item_image.sprite = item.item_icon;
        else
            item_image.sprite = null;
    }
    public Item Get_Item()
    {
        if (this.item == null)
            return null;
        return (this.item);
    }
    public void AssignItem(Item _item)
    {
        this.item = _item;
    }
}