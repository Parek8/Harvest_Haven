using System.Collections.Generic;
using UnityEngine;

public class GetFuelItems : GetInventoryItems
{
    [field: SerializeField] private FuelItemMenuSlot FuelSlot;
    public override void GetItems()
    {
        DestroyButtons();
        CreateButtons((List<Item>)GameManager.game_manager.player_inventory.GetAllSmeltingFuel());
    }
    public override void CreateButtons(List<Item> _items)
    {
        foreach (Item _item in _items)
        {
            GameObject _obj = Instantiate(FuelSlot.gameObject, transform);
            _obj.GetComponent<FuelItemMenuSlot>().Init(_item);
        }
    }
}
