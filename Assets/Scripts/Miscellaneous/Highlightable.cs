using UnityEngine;

internal class Highlightable : MonoBehaviour
{
    [field: SerializeField] string _itemMessage;
    [field: SerializeField] bool Interactable;
    [field: SerializeField] bool Destroyable;
    [field: SerializeField] bool Attackable;
    internal string GetMessage()
    {
        if (_itemMessage == null)
            Debug.Log("No message!");

        string _message = _itemMessage;
        if (Interactable)
            _message = $"{_itemMessage} [{Input_Manager.GetKeyByName(KeybindNames.interact)}]";
        else if (Attackable)
            _message = $"Attack {_itemMessage} [{Input_Manager.GetKeyByName(KeybindNames.left_attack)}]";
        else if (Destroyable)
            _message = $"Destroy {_itemMessage} [{Input_Manager.GetKeyByName(KeybindNames.left_attack)}]";

        return _message;
    }
}