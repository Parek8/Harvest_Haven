using System.Collections;
using UnityEngine;

internal class UI_Behaviour : MonoBehaviour
{
    protected float TimeToPopup = 0.1f;
    internal bool is_visible { get; private set; } = false;
    private void Start()
    {
        is_visible = gameObject.activeInHierarchy;
    }
    internal virtual bool Show()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            is_visible = true;
            StartCoroutine("__Show");
        }

        return is_visible;
    }

    internal virtual bool Hide()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine("__Hide");

        return is_visible;
    }

    protected virtual IEnumerator __Hide()
    {
        transform.LeanScale(Vector3.zero, TimeToPopup).setEaseOutQuart();
        yield return new WaitForSeconds(TimeToPopup);
        _Hide();
    }
    protected virtual IEnumerator __Show()
    {
        _Show();
        transform.LeanScale(Vector3.one, TimeToPopup).setEaseOutQuart();
        yield return new WaitForSeconds(TimeToPopup);
    }
    internal bool Change_State()
    {
        GameManager.game_manager.Cursor_Needed((!is_visible) ? CursorLockMode.None : CursorLockMode.Locked);
        is_visible = !is_visible;

        if (!is_visible)
            Hide();
        else
            Show();

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