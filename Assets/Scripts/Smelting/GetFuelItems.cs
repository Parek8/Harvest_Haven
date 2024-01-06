using System.Collections.Generic;
using UnityEngine;

internal class GetFuelItems : GetInventoryItems
{
    [field: SerializeField] private FuelItemMenuSlot FuelSlot;
    internal override void GetItems()
    {
        DestroyButtons();
        CreateButtons((List<Item>)GameManager.game_manager.player_inventory.GetAllSmeltingFuel());
    }
    internal override void CreateButtons(List<Item> _items)
    {
        foreach (Item _item in _items)
        {
            GameObject _obj = Instantiate(FuelSlot.gameObject, transform);
            _obj.GetComponent<FuelItemMenuSlot>().Init(_item);
        }
    }
}
