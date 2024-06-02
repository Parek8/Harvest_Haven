using UnityEngine;

internal class Smelter : Interactable
{
    [field: SerializeField] UI_Behaviour _smeltingScreen;
    internal override void Interact()
    {
        _smeltingScreen.Show();
        GameManager.GameManagerInstance.Cursor_Needed(CursorLockMode.None);
    }
}