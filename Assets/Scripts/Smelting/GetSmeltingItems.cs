using System.Collections.Generic;
using UnityEngine;

internal class GetSmeltingItems : GetInventoryItems
{
    [field: SerializeField] SmeltItemMenuSlot SmeltingSlot;
    internal override void GetItems()
    {
        DestroyButtons();
        CreateButtons((List<Item>)GameManager.game_manager.player_inventory.GetAllSmeltableItems());
    }
    internal override void CreateButtons(List<Item> _items)
    {
        foreach (Item _item in _items)
        {
            if (!usedItems.Contains(_item))
            {
                GameObject _obj = Instantiate(SmeltingSlot.gameObject, transform);
                _obj.GetComponent<SmeltItemMenuSlot>().Init(_item);
            }
        }
    }
}