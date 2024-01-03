using System.Collections.Generic;
using UnityEngine;

public class GetSmeltingItems : GetInventoryItems
{
    [field: SerializeField] SmeltItemMenuSlot SmeltingSlot;
    public override void GetItems()
    {
        DestroyButtons();
        CreateButtons((List<Item>)GameManager.game_manager.player_inventory.GetAllSmeltableItems());
    }
    public override void CreateButtons(List<Item> _items)
    {
        foreach (Item _item in _items)
        {
            GameObject _obj = Instantiate(SmeltingSlot.gameObject, transform);
            _obj.GetComponent<SmeltItemMenuSlot>().Init(_item);
        }
    }
}