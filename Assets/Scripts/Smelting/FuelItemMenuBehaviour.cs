using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelItemMenuBehaviour : UI_Behaviour
{
    [field: SerializeField] UI_Behaviour ResultScreen;
    FuelItemSlot _assignedSlot;

    public void AssignSlot(FuelItemSlot _slot)
    {
        if (_slot != null)
            _assignedSlot = _slot;
    }

    public void AssignItem(Item _item)
    {
        if (_item != null)
            _assignedSlot.AssignItem(_item);
    }
    public override void _Hide()
    {
        base._Hide();
        ResultScreen.Show();
    }
    public override void _Show()
    {
        base._Show();
        ResultScreen.Hide();
    }
}