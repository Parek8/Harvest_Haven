using UnityEngine;

internal class ShopTalkable : Interactable
{
    [field: SerializeField] TestDialog _dialog;
    bool talking = false;
    bool inDistance => (Vector3.Distance(_player.position, transform.position) <= _distance);
    internal override void Interact()
    {
        if (_dialog == null)
            _dialog = GetComponent<TestDialog>();

        if (!talking && inDistance)
        {
            talking = true;
            DialogManager.DialogManagerInstance.Show();
            DialogManager.DialogManagerInstance.UpdateDialog(_dialog.NextIndex());
        }
    }
    private void Update()
    {
        if (!inDistance)
            OutDistance();
    }
    protected void OutDistance()
    {
        if (talking)
        {
            DialogManager.DialogManagerInstance.Hide();
            talking = false;
        }
    }
    public void StopTalking() => talking = false;
}
