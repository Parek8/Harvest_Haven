using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public int SlotIndex { get; private set; }

    [field: SerializeField] bool IsResult;
    [field: SerializeField] Item item;
    [field: SerializeField] Image item_image;


    bool is_dragging = false;

    int _itemCount = 0;
    public int ItemCount => _itemCount;
    public bool IsAvailable => (this.item == null || this.ItemCount < 10);

    void FixedUpdate()
    {
        if (is_dragging)
        {
            item_image.transform.position = Input.mousePosition;
            item_count.transform.position = Input.mousePosition + new Vector3(-19, 2.2f, 0);
        }
        if (item != null)
            if (_itemCount <= 0 || !item.is_stackable /*&& player_inventory.slots.FindAll(slot => slot.Get_Item() == this.item).Count > this.item.count*/)
                Clear_Item();
        Update_UI();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            ShowItemMenu();
    }

    private void ShowItemMenu()
    {
        throw new NotImplementedException();
    }

    public void Clear_Item()
    {
        item.AssignedSlot = null;
        this.item = null;
        _itemCount = 0;
    }
    public void Update_UI()
    {
        if (this.item != null)
        {
            if (!item_count.gameObject.activeInHierarchy && item.is_stackable)
                item_count.gameObject.SetActive(true);
            else if (item_count.gameObject.activeInHierarchy && !item.is_stackable)
                item_count.gameObject.SetActive(false);

            item_image.sprite = item.item_icon;
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
    public Item Get_Item()
    {
        if (this.item == null)
            return null;
        return (this.item);
    }
}