using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal class GetInventoryItems : MonoBehaviour
{
    [field: SerializeField] private ItemMenuSlot Slot;
    private void OnEnable() => GetItems();

    internal void DestroyButtons()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    internal virtual void GetItems()
    {
        DestroyButtons();
        CreateButtons((List<Item>)GameManager.game_manager.player_inventory.GetAllItems());
    }

    internal virtual void CreateButtons(List<Item> _items)
    {
        foreach (Item _item in _items)
        {
            GameObject _obj = Instantiate(Slot.gameObject, transform);
            _obj.GetComponent<ItemMenuSlot>().Init(_item);
        }
    }
}