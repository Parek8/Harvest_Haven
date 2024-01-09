using System.Collections.Generic;
using UnityEngine;

internal class GetInventoryItems : MonoBehaviour
{
    [field: SerializeField] private ItemMenuSlot Slot;
    private void OnEnable() => GetItems();
    protected HashSet<Item> usedItems = new HashSet<Item>();

    internal void DestroyButtons()
    {
        usedItems.Clear();
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
            if (!usedItems.Contains(_item))
            {
                GameObject _obj = Instantiate(Slot.gameObject, transform);
                _obj.GetComponent<ItemMenuSlot>().Init(_item);
                usedItems.Add( _item );
            }
        }
    }
}