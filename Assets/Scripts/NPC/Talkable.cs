using UnityEngine;

internal class ShopTalkable : Interactable
{
    [field: SerializeField] TestDialog _dialog;
    bool talking = false;
    //bool inDistance => (Vector3.Distance(_player.position, transform.position) <= _distance);
    internal override void Interact()
    {
        if (_dialog == null)
            _dialog = GetComponent<TestDialog>();

        //Debug.Log($"Talking: {talking} | In range: {Vector3.Distance(_player.position, transform.position) <= 2.5f}");
        if (!talking && Vector3.Distance(_player.position, transform.position) <= 2.5f)
        {
            talking = true;
            DialogManager.DialogManagerInstance.Show();
            DialogManager.DialogManagerInstance.UpdateDialog(_dialog.NextIndex());
        }
    }
    private void Update()
    {
        if (!(Vector3.Distance(_player.position, transform.position) <= 2.5f))
            OutDistance();
    }
    protected void OutDistance()
    {
        if (talking)
        {
            DialogManager.DialogManagerInstance.Hide();
            StopTalking();
        }
    }
    public void StopTalking()
    {
        talking = false;
    }
}
