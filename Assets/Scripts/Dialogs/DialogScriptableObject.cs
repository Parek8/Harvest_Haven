using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IDialog
{
    DialogLine NextIndex();
}
public class DialogLine
{
    string _line;
    public List<DialogButton> Buttons => _buttons;

    List<DialogButton> _buttons;
    public string Line => _line;

    public DialogLine(string line, List<DialogButton> _buttons) 
    {
        this._line = line;
        this._buttons = _buttons;
    }
}
public class DialogButton
{
    string _name;
    UnityAction _event;

    public string Name => _name;
    public UnityAction Event => _event;

    public DialogButton(UnityAction _event, string _name)
    { 
        this._event = _event;
        this._name = _name;
    }

    public Button GetButton(Transform _parent)
    {

        Button btn = MonoBehaviour.Instantiate(GameManager.game_manager.ButtonPrefab, _parent);
        btn.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = _name;
        btn.onClick.AddListener(_event);
        return btn;
    }
}