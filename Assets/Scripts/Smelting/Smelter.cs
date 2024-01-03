using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : Interactable
{
    [field: SerializeField] UI_Behaviour _smeltingScreen;
    public override void Interact()
    {
        _smeltingScreen.Show();
        GameManager.game_manager.Cursor_Needed(CursorLockMode.None);
    }
}