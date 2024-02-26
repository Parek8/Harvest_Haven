using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class GameManager : MonoBehaviour
{
    internal static GameManager game_manager { get; private set; }
    [field: SerializeField] internal PlayerSettings PlayerSettings { get; private set; }

    void Awake()
    {
        if (game_manager == null)
            game_manager = this;
        LoadSettings();

        for (int i = 0;  i < all_items.Count; i++)
        {
            Item _item = all_items[i];
            _allItems[_item.ItemID] = _item;
        }

        for (int i = 0; i < all_crops.Count; i++)
        {
            Plot _plot = _allCrops[i];
            _allCrops[_plot.PlotIndex] = _plot;
        }
        LoadGraphics();
    }

    internal void LoadGraphics()
    {
        Screen.SetResolution(PlayerSettings.RESX, PlayerSettings.RESY, PlayerSettings.FULLSCREEN);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)PlayerSettings.FPS;
        Camera.main.fieldOfView = PlayerSettings.FOV;
    }

    internal bool is_game_paused { get; private set; }
    [field: SerializeField] internal Transform player_transform { get; private set; }
    [field: SerializeField] internal Inventory player_inventory { get; private set; }
    [field: SerializeField] internal List<Item> all_items { get; private set; }
    [field: SerializeField] internal List<Plot> all_crops { get; private set; }
    [field: SerializeField] internal List<PlantObject> all_plantable_objects { get; private set; }
    [field: SerializeField] internal Item Null_Item { get; private set; }
    [field: SerializeField] internal Transform environment_parent { get; private set; }
    [field: SerializeField] internal Dictionary<int, Item> _allItems = new Dictionary<int, Item>();
    [field: SerializeField] internal Dictionary<int, Plot> _allCrops = new Dictionary<int, Plot>();
    [field: SerializeField] internal InventoryManager InventoryManagerInstance { get; private set; }
    [field: SerializeField] internal PlayerManager PlayerManagerInstance { get; private set; }
    [field: SerializeField] internal CropsManager CropsManagerInstance { get; private set; }
    [field: SerializeField] internal Button ButtonPrefab { get; private set; }
    [field: SerializeField] internal UI_Behaviour HUD { get; private set; }
    [field: SerializeField] internal UI_Behaviour PauseMenu { get; private set; }
    [field: SerializeField] internal CinemachineFreeLook FreeCamera { get; private set; }

    private void OnApplicationQuit()
    {
        InventoryManagerInstance.SaveInventory();
        CropsManagerInstance.SaveCrops();
        PlayerManagerInstance.SavePlayer();
    }

    internal void Cursor_Needed(CursorLockMode lock_mode)
    {
        if (Cursor.lockState != lock_mode)
            Cursor.lockState = lock_mode;
    }
    public void PauseGame()
    {
        Cursor_Needed(CursorLockMode.None);
        is_game_paused = true;
        HUD.Hide();
        PauseMenu.Show();
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        is_game_paused = false;
        PauseMenu.Hide();
        HUD.Show();
        Cursor_Needed(CursorLockMode.Locked);
    }

    public void ExitGame()
    {
        if (InventoryManagerInstance != null)
            InventoryManagerInstance.SaveInventory();
        SceneManager.LoadScene("MainMenu");
    }
    internal PlantObject FindPlantObject(int index)
    {
        return all_plantable_objects.Find(_plant => _plant.PlantObjectIndex == index);
    }
    internal Dictionary<KeybindNames, KeyCode> keybinds { get; private set; } = new Dictionary<KeybindNames, KeyCode>();
    internal bool IsKeybindSaved(KeybindNames keybind)
    {
        return (PlayerPrefs.GetString(keybind.ToString()) != "" && PlayerPrefs.GetString(keybind.ToString()) != null);
    }
    internal void SetKeybind(KeybindNames name, KeyCode input)
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
                try
                {
                    keybinds.Add(key, Enum.Parse<KeyCode>(PlayerPrefs.GetString(key.ToString())));
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
            }
        }
        Input_Manager.SetKeybindsList(keybinds);
    }
}


// CUSTOM INPUT MANAGER FOR CUSTOM INPUTS FOR EASIER KEYBIND SYSTEM
internal static class Input_Manager
{
    static Dictionary<KeybindNames, KeyCode> keybinds = new Dictionary<KeybindNames, KeyCode>();
    internal static void SetKeybindsList(Dictionary<KeybindNames, KeyCode> keybindsDic)
    {
        keybinds = keybindsDic;
    }
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