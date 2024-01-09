using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

internal class UI_Behaviour : MonoBehaviour
{
    internal bool is_visible { get; private set; } = false;
    private void Start()
    {
        is_visible = gameObject.activeInHierarchy;
    }
    internal virtual bool Show()
    {
        _Show();
        return is_visible;
    }

    internal virtual bool Hide()
    {
        _Hide();
        return is_visible;
    }
    internal bool Change_State()
    {
        GameManager.game_manager.Cursor_Needed((!is_visible) ? CursorLockMode.None : CursorLockMode.Locked);
        gameObject.SetActive(!is_visible);
        is_visible = !is_visible;
        return is_visible;
    }
    internal virtual void _Hide()
    {
        gameObject.SetActive(false);
        is_visible = false;
    }
    internal virtual void _Show()
    {
        gameObject.SetActive(true);
        is_visible = true;
    }

    internal void CursorNone() => GameManager.game_manager.Cursor_Needed(CursorLockMode.None);

    internal void CursorLocked() => GameManager.game_manager.Cursor_Needed(CursorLockMode.Locked);
}