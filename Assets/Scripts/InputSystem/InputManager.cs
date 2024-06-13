using System;
using System.Collections.Generic;
using UnityEngine;

// CUSTOM INPUT MANAGER FOR CUSTOM INPUTS FOR EASIER KEYBIND SYSTEM
internal static class Input_Manager
{
    static Dictionary<KeybindNames, KeyCode> keybinds = new Dictionary<KeybindNames, KeyCode>();
    internal static void SetKeybindsList(Dictionary<KeybindNames, KeyCode> keybindsDic) => keybinds = keybindsDic;
    internal static bool GetCustomKeyDown(KeybindNames _bind)
    {
        try
        {
            if (Input.GetKey(keybinds[_bind]))
                return true;
            else
                return false;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    internal static float GetCustomAxisRaw(string axis)
    {
        try
        {
            if (axis == "Vertical")
            {
                if (Input.GetKey(keybinds[KeybindNames.forward]))
                    return 1;
                else if (Input.GetKey(keybinds[KeybindNames.backward]))
                    return -1;
                else
                    return 0;
            }
            else if (axis == "Horizontal")
            {
                if (Input.GetKey(keybinds[KeybindNames.left_strafe]))
                    return -1;
                else if (Input.GetKey(keybinds[KeybindNames.right_strafe]))
                    return 1;
                else
                    return 0;
            }
            else if (axis == "Sprinting")
            {
                if (Input.GetKey(keybinds[KeybindNames.sprint]))
                    return 1;
                else
                    return 0;
            }
            else if (axis == "Jumping")
            {
                if (Input.GetKey(keybinds[KeybindNames.jump]))
                    return 1;
                else
                    return 0;
            }
            else if (axis == "Attack")
            {
                if (Input.GetKey(keybinds[KeybindNames.left_attack]))
                    return 1;
                else
                    return 0;
            }
            else if (axis == "Interact")
            {
                if (Input.GetKey(keybinds[KeybindNames.interact]))
                    return 1;
                else
                    return 0;
            }
            else
                Debug.Log($"No such axis as {axis}");
            return 0;
        }
        catch
        {
            SetPlayerPrefs();
            return -1f;
        }
    }
    internal static bool GetCustomAxisRawDown(string axis)
    {
        try
        {
            if (axis == "Interact")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.interact]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Attack")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.left_attack]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Inventory")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.inventory]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_1")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_1]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_2")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_2]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_3")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_3]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_4")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_4]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_5")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_5]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_6")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_6]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_7")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_7]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_8")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_8]))
                    return true;
                else
                    return false;
            }
            else if (axis == "Slot_9")
            {
                if (Input.GetKeyDown(keybinds[KeybindNames.slot_9]))
                    return true;
                else
                    return false;
            }
            else
                Debug.Log($"No such axis as {axis}");
            return false;
        }
        catch
        {
            SetPlayerPrefs();
            return false;
        }
    }
    private static void SetPlayerPrefs()
    {
        KeybindNames[] keybinds = (KeybindNames[])Enum.GetValues(typeof(KeybindNames));

        for (int i = 0; i < keybinds.Length; i++)
        {
            KeybindNames kb = keybinds[i];

            PlayerPrefs.SetString(kb.ToString(), ((KeyCode)kb).ToString());
        }
    }
    internal static string GetKeyByName(KeybindNames _name) => keybinds[_name].ToString();
}