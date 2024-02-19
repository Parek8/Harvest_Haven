using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class SmeltItemMenuSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] Item AssignedItem;
    [field: SerializeField] Image _itemImage;
    [field: SerializeField] TMP_Text _itemCount;

    SmeltingItemMenuBehaviour _parent;
    private void Start()
    {
        _parent = transform.parent.parent.GetComponent<SmeltingItemMenuBehaviour>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            _parent.AssignItem(AssignedItem);
    }

    internal void Init(Item _item)
    {
        if (_item != null)
        {
            AssignedItem = _item;
            _itemImage.sprite = _item.ItemIcon;
            _itemCount.text = $"x{GameManager.game_manager.player_inventory.GetItemCountInInventory(AssignedItem)}";
        }
    }
}