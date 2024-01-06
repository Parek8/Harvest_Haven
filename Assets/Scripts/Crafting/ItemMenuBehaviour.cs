using UnityEngine;

internal class ItemMenuBehaviour : UI_Behaviour
{
    [field: SerializeField] UI_Behaviour ResultScreen;
    CraftingSlot _assignedSlot;

    internal void AssignSlot(CraftingSlot _slot)
    {
        if (_slot != null)
            _assignedSlot = _slot;
    }

    internal void AssignItem(Item _item)
    {
        if (_item != null)
            _assignedSlot.AssignItem(_item);
    }
    internal override void _Hide()
    {
        base._Hide();
        ResultScreen.Show();
    }
    internal override void _Show()
    {
        base._Show();
        ResultScreen.Hide();
    }
}