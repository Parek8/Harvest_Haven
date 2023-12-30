using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMenuSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] Item AssignedItem;
    [field: SerializeField] Image _itemImage;
    [field: SerializeField] TMP_Text _itemCount;

    ItemMenuBehaviour _parent;
    private void Start()
    {
        _parent = transform.parent.parent.GetComponent<ItemMenuBehaviour>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            _parent.AssignItem(AssignedItem);
    }

    public void Init(Item _item)
    {
        if (_item != null) 
        {
            AssignedItem = _item;
            _itemImage.sprite = _item.item_icon;
            _itemCount.text = $"xy";
        }
    }
}