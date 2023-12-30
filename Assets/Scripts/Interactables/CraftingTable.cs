using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : Interactable
{
    [field: SerializeField] UI_Behaviour _craftingScreen;
    public override void Interact()
    {
        _craftingScreen.Show();
        GameManager.game_manager.Cursor_Needed(CursorLockMode.None);
    }
}