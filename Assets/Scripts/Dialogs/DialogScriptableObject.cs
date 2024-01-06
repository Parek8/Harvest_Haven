using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

internal interface IDialog
{
    DialogLine NextIndex();
}
internal class DialogLine
{
    string _line;
    internal List<DialogButton> Buttons => _buttons;

    List<DialogButton> _buttons;
    internal string Line => _line;

    internal DialogLine(string line, List<DialogButton> _buttons) 
    {
        this._line = line;
        this._buttons = _buttons;
    }
}
internal class DialogButton
{
    string _name;
    UnityAction _event;

    internal string Name => _name;
    internal UnityAction Event => _event;

    internal DialogButton(UnityAction _event, string _name)
    { 
        this._event = _event;
        this._name = _name;
    }

    internal Button GetButton(Transform _parent)
    {

        Button btn = MonoBehaviour.Instantiate(GameManager.game_manager.ButtonPrefab, _parent);
        btn.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = _name;
        btn.onClick.AddListener(_event);
        return btn;
    }
}