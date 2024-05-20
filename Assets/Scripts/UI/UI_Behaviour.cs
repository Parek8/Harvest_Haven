using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class UI_Behaviour : MonoBehaviour
{
    [field: SerializeField] float TimeToPopup = 1f;
    internal bool is_visible { get; private set; } = false;
    private void Start()
    {
        is_visible = gameObject.activeInHierarchy;
    }
    internal virtual bool Show()
    {
        if (!gameObject.activeInHierarchy)
        {
            transform.LeanScale(Vector3.one, TimeToPopup);
            _Show();
        }

        return is_visible;
    }

    internal virtual bool Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.LeanScale(Vector3.zero, TimeToPopup);
            _Hide();
        }

        return is_visible;
    }
    internal bool Change_State()
    {
        GameManager.game_manager.Cursor_Needed((!is_visible) ? CursorLockMode.None : CursorLockMode.Locked);
        gameObject.SetActive(!is_visible);
        is_visible = !is_visible;
        return is_visible;
    }
    public virtual void _Hide()
    {
        gameObject.SetActive(false);
        is_visible = false;
    }
    public virtual void _Show()
    {
        gameObject.SetActive(true);
        is_visible = true;
    }

    internal void CursorNone() => GameManager.game_manager.Cursor_Needed(CursorLockMode.None);

    internal void CursorLocked() => GameManager.game_manager.Cursor_Needed(CursorLockMode.Locked);
}