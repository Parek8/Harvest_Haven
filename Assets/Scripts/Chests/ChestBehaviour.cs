using UnityEngine;

[RequireComponent(typeof(Animator))]
internal class ChestBehaviour : Interactable
{
    Animator _animator;
    bool _isOpened = false;
    [field: SerializeField] UI_Behaviour LootChestScreen;
    [field: SerializeField] UI_Behaviour PlayerInventoryScreen;
    new private void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!(Vector3.Distance(_player.position, transform.position) <= 5.5f))
            OutDistance();
    }

    protected void OutDistance()
    {
        if (_isOpened)
        {
            LootChestScreen.Hide();
            _isOpened = false;
        }
    }

    internal override void Interact()
    {
        if (Vector3.Distance(_player.position, transform.position) <= 5.5f)
        {
            _isOpened = !_isOpened;
            _animator.SetTrigger((_isOpened) ? "Open" : "Close");
            if (_isOpened)
                LootChestScreen.Show();
            else
                LootChestScreen.Hide();
        }
    }
}