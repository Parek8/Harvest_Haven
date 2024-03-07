using UnityEngine;

internal class ShopTalkable : Interactable
{
    [field: SerializeField] TestDialog _dialog;
    bool talking = false;

    internal override void Interact()
    {
        if (_dialog == null)
            _dialog = GetComponent<TestDialog>();

        if (!talking)
        {
            talking = true;
            DialogManager.DialogManagerInstance.Show();
            DialogManager.DialogManagerInstance.UpdateDialog(_dialog.NextIndex());
        }
        else
            StopTalking();
    }

    public void StopTalking()
{
        talking = false;
        DialogManager.DialogManagerInstance.Hide();
        _dialog.Shop.Hide();
        _dialog.Shop.CursorLocked();
    }
}