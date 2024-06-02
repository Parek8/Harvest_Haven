using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class CraftingTable : Interactable
{
    [field: SerializeField] UI_Behaviour _craftingScreen;
    internal override void Interact()
    {
        _craftingScreen.Show();
        GameManager.GameManagerInstance.Cursor_Needed(CursorLockMode.None);
    }
}