using UnityEngine;

internal class UIBehaviour : MonoBehaviour
{
    internal bool isVisible { get; private set; } = false;
    private void Start()
    {
        isVisible = gameObject.activeInHierarchy;
    }
    internal virtual bool Show()
    {
        _Show();
        return isVisible;
    }

    internal virtual bool Hide()
    {
        _Hide();
        return isVisible;
    }
    internal bool ChangeState()
    {
        GameManager.game_manager.Cursor_Needed((!isVisible) ? CursorLockMode.None : CursorLockMode.Locked);
        gameObject.SetActive(!isVisible);
        isVisible = !isVisible;
        return isVisible;
    }
    public virtual void _Hide()
    {
        gameObject.SetActive(false);
        isVisible = false;
    }
    public virtual void _Show()
    {
        gameObject.SetActive(true);
        isVisible = true;
    }

    internal void CursorNone() => GameManager.game_manager.Cursor_Needed(CursorLockMode.None);

    internal void CursorLocked() => GameManager.game_manager.Cursor_Needed(CursorLockMode.Locked);
}