using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : Interactable
{
    [field: SerializeField] TestDialog _dialog;
    bool talking = false;
    public override void Interact()
    {
        if (_dialog == null)
            _dialog = GetComponent<TestDialog>();

        talking = true;

        DialogManager.DialogManagerInstance.Show();
        DialogManager.DialogManagerInstance.UpdateDialog(_dialog.NextIndex());
    }
    protected override void OutDistance()
    {
        base.OutDistance();
        talking = false;
    }
}
