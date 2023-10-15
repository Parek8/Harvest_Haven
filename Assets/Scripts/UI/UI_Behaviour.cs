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
    public bool Show()
    {
        _Show();
        return is_visible;
    }

    public bool Hide()
    {
        _Hide();
        return is_visible;
    }
    public bool Change_State()
    {
        gameObject.SetActive(!is_visible);
        is_visible = !is_visible;
        return is_visible;  
    }
    public void _Hide()
    {
        gameObject.SetActive(false);
        is_visible = false;
    }
    public void _Show()
    {
        gameObject.SetActive(true);
        is_visible = true;
    }
}