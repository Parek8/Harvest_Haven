using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

internal class Settings_Button : MonoBehaviour
{
    [SerializeField] KeybindNames keybind_name;
    [SerializeField] KeyCode default_keycode;

    internal KeyCode keycode { get; private set; }
    private bool isAwaitingInput = false;

    // For rendering some text, so user isn't so confused
    Button btn;
    TMP_Text btn_text;
    private void Awake()
    {
        btn = gameObject.GetComponentInChildren<Button>();
        btn_text = btn.GetComponentInChildren<TMP_Text>();
    }
    private void DecideKeycode()
    {
        if (GameManager.game_manager.keybinds.ContainsKey(keybind_name))
            keycode = GameManager.game_manager.keybinds[keybind_name];

        else if (keycode == KeyCode.None)
                RevertToDefault();
    }
    private void Start()
    {
        DecideKeycode();
        RenderKeyCode();
        if (!GameManager.game_manager.IsKeybindSaved(keybind_name))
            SetValue();

    }
    internal void RevertToDefault()
    {
        this.keycode = default_keycode;
    }
    private void Update()
    {
        if(isAwaitingInput)
        {
            // fr very goofy, but for this moment it's enough
            foreach(KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc) && kc != KeyCode.Escape)
                {
                    keycode = kc;
                    isAwaitingInput = false;
                    RenderKeyCode();
                    SetValue();
                }
                else if(Input.GetKeyDown(KeyCode.Escape))
                {
                    isAwaitingInput = false;
                    RenderKeyCode();
                    SetValue();
                }
            }
        }
    }
    internal void ChangeKeyCode()
    {
        btn_text.text = "Awaiting new keybind...";
        isAwaitingInput = true;
        btn.enabled = false;
    }
    private void RenderKeyCode()
    {
        btn_text.text = keycode.ToString();
        btn.enabled = true;
    }
    private void SetValue()
    {
        GameManager.game_manager.SetKeybind(keybind_name, keycode);
    }
}