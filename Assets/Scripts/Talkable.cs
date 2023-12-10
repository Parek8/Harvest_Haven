using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : Interactable
{
    //[field: SerializeField] Dialog _dialog;
    [field: SerializeField] UI_Behaviour _shop;
    public override void Interact()
    {
        _shop.Change_State();
    }
    protected override void OutDistance()
    {
        base.OutDistance();
        _shop.Hide();
    }
}
