using UnityEngine;

internal class SmeltingItemMenuBehaviour : UI_Behaviour
{
    [field: SerializeField] UI_Behaviour ResultScreen;
    SmeltItemSlot _assignedSlot;

    internal void AssignSlot(SmeltItemSlot _slot)
    {
        if (_slot != null)
            _assignedSlot = _slot;
    }

    internal void AssignItem(Item _item)
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