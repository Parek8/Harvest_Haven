using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialog : MonoBehaviour, IDialog
{
    [field: SerializeField] UI_Behaviour _shop;
    List<DialogLine> _lines = new();
    int index = 0;

    public DialogLine NextIndex()
    {
        DialogLine _toReturn = _lines[0];
        if(index < _lines.Count)
        {
            _toReturn = _lines[index];
            index++;
        }

        if (index >= _lines.Count)
            index = _lines.Count - 1;

        return _toReturn;
    }
    private void Start()
    {
        _lines.Add(new DialogLine("Welcome to my store!", new List<DialogButton>() {
            new DialogButton(() => {
                DialogManager.DialogManagerInstance.Hide();
                GameManager.game_manager.Cursor_Needed(CursorLockMode.None);
                _shop.Show();
            }, "Shop!"),
        }));
    }
}
