using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager game_manager { get; private set; }
    void Awake()
    {
        if (game_manager == null)
            game_manager = this;
        LoadSettings();
        //Load_Item_Ids();
    }

    ////Global variables - Settings
    //public int mouse_sensitivity { get; private set; } = 50;
    //public bool is_game_paused { get; private set; }
    //[field: SerializeField] public Transform player_transform { get; private set; }
    //[field: SerializeField] public Player_Inventory player_inventory { get; private set; } 
    //[field: SerializeField] public Main_Menu_Buttons main_menu_buttons { get; private set; }
    //[field: SerializeField] public List<Item> all_items { get; private set; }

    //private Dictionary<int, Item> item_ids = new();

    //public Action On_Load { get; private set; }
    //private void Start()
    //{
    //    //player_transform = GameObject.Find("MainCharacter").transform;
    //    On_Load?.Invoke();
    //    StartGame(); //JUST FOR NOW
    //}

    //private void Load_Item_Ids()
    //{
    //    foreach(Item item in all_items)
    //    {
    //        item_ids.Add(item.id, item);
    //    }
    //}

    //public Item Get_Item_Info_By_Index(int item_index)
    //{
    //    return all_items.Find(ind => ind.id == item_index);
    //}
    //public void Save_All_Items()
    //{

    //}
    //public bool Check_Items_Ids()
    //{
    //    List<int> ids = new();
    //    List<Item> wrong_items = new();
    //    foreach (Item item in all_items)
    //    {
    //        if (!ids.Contains(item.id))
    //            ids.Add(item.id);
    //        else
    //            wrong_items.Add(item);
    //    }
    //    if (wrong_items.Count > 0)
    //        Debug.LogError($"Error with item ids {wrong_items}");

    //    return (wrong_items.Count <= 0);
    //}
    //public Item Get_Item_By_Id(int id)
    //{
    //    return item_ids[id];
    //}
    public Dictionary<KeybindNames, KeyCode> keybinds { get; private set; } = new Dictionary<KeybindNames, KeyCode>();
    public bool IsKeybindSaved(KeybindNames keybind)
    {
        return (PlayerPrefs.GetString(keybind.ToString()) != "" && PlayerPrefs.GetString(keybind.ToString()) != null);
    }
    public void SetKeybind(KeybindNames name, KeyCode input)
    {
        if(!keybinds.ContainsKey(name))
            keybinds.Add(name, input);
        else
            keybinds[name] = input;
        SaveSettings(name.ToString(), input);
    }

    //Used for saving the desired settings
    private void SaveSettings<DataType>(string key, DataType value)
    {
        PlayerPrefs.SetString(key, value.ToString());
        PlayerPrefs.Save();
    }
    private void LoadSettings()
    {
        foreach (KeybindNames key in Enum.GetValues(typeof(KeybindNames)))
        {
            if (!keybinds.ContainsKey(key))
            {
                keybinds.Add(key, Enum.Parse<KeyCode>(PlayerPrefs.GetString(key.ToString())));
            }
        }
        InputManager.SetKeybindsList(keybinds);
    }
}


// CUSTOM INPUT MANAGER FOR CUSTOM INPUTS FOR EASIER KEYBIND SYSTEM
public static class InputManager
{
    static Dictionary<KeybindNames, KeyCode> keybinds = new Dictionary<KeybindNames, KeyCode>();
    public static void SetKeybindsList(Dictionary<KeybindNames, KeyCode> keybindsDic)
    {
        keybinds = keybindsDic;
    }

    public static float GetCustomAxisRaw(string axis)
    {
        if (axis == "Vertical")
        {
            if (Input.GetKey(keybinds[KeybindNames.forward]))
                return 1;
            else if (Input.GetKey(keybinds[KeybindNames.backward]))
                return -1;
        }
        else if (axis == "Horizontal")
        {
            if (Input.GetKey(keybinds[KeybindNames.left_strafe]))
                return -1;
            else if (Input.GetKey(keybinds[KeybindNames.right_strafe]))
                return 1;
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
    public static bool GetCustomAxisRawDown(string axis)
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
        else
            Debug.Log($"No such axis as {axis}");
        return false;
    }
    public static void Add_TMP_Axis()
    {
        foreach (KeybindNames key in Enum.GetValues(typeof(KeybindNames)))
        {
            PlayerPrefs.SetString(key.ToString(), "L");
            PlayerPrefs.Save();
            Debug.Log($"Set {key} to 'L'!");
        }
        Debug.Log("Done");
    }
}
