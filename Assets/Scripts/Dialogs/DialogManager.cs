using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    static DialogManager dialogManager;
    public static DialogManager DialogManagerInstance => dialogManager;

    [field: SerializeField] UI_Behaviour Dialog;
    [field: SerializeField] TMP_Text _dialogText;
    [field: SerializeField] Transform _buttonParent;

    private void Start()
    {
        if (dialogManager == null)
            dialogManager = this;
    }

    public void Show()
    {
        GameManager.game_manager.Cursor_Needed(CursorLockMode.None);
        Dialog.Show();
    }
    public void Hide()
    {
        GameManager.game_manager.Cursor_Needed(CursorLockMode.Locked);
        Dialog.Hide();
    }
    public void ChangeState()
    {
        Dialog.Change_State();
    }

    public void UpdateDialog(DialogLine _line)
    {
        _dialogText.text = _line.Line;

        for (int i = 0; i < _buttonParent.childCount; i++)
        {
            Destroy(_buttonParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < _line.Buttons.Count; i++)
        {
            _line.Buttons[i].GetButton(_buttonParent);
        }
    }
}
