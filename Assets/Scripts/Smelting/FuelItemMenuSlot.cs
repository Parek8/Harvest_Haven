using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FuelItemMenuSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] Item AssignedItem;
    [field: SerializeField] Image _itemImage;
    [field: SerializeField] TMP_Text _itemCount;

    FuelItemMenuBehaviour _parent;
    private void Start()
    {
        _parent = transform.parent.parent.GetComponent<FuelItemMenuBehaviour>();
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
            _itemCount.text = $"x{GameManager.game_manager.player_inventory.GetItemCountInInventory(AssignedItem)}";
        }
    }
}