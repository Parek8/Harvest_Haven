using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Smelter : Interactable
{
    [field: SerializeField] UI_Behaviour _smeltingScreen;
    internal override void Interact()
    {
        _smeltingScreen.Show();
        GameManager.game_manager.Cursor_Needed(CursorLockMode.None);
    }
}