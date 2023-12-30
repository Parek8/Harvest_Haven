using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UI_Behaviour : MonoBehaviour
{
    public bool is_visible { get; private set; } = false;
    private void Start()
    {
        is_visible = gameObject.activeInHierarchy;
    }
    public virtual bool Show()
    {
        _Show();
        return is_visible;
    }

    public virtual bool Hide()
    {
        _Hide();
        return is_visible;
    }
    public bool Change_State()
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

    public void CursorNone() => GameManager.game_manager.Cursor_Needed(CursorLockMode.None);

    public void CursorLocked() => GameManager.game_manager.Cursor_Needed(CursorLockMode.Locked);
}