using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTalkable : Interactable
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
    private void Update()
    {
        if (Vector3.Distance(_player.position, transform.position) > _distance)
            OutDistance();
    }
    protected void OutDistance()
    {
        talking = false;
    }
}
