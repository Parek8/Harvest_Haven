using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class GameManager : MonoBehaviour
{
    internal static GameManager GameManagerInstance { get; private set; }
    [field: SerializeField] internal PlayerSettings PlayerSettings { get; private set; }

    void Awake()
    {
        if (GameManagerInstance == null)
            GameManagerInstance = this;

        LoadSettings();

        for (int i = 0;  i < AllItems.Count; i++)
        {
            Item _item = AllItems[i];
            AllItemsDictionary[_item.ItemID] = _item;
        }

        for (int i = 0; i < AllCrops.Count; i++)
        {
            Plot _plot = AllCropsDictionary[i];
            AllCropsDictionary[_plot.PlotIndex] = _plot;
        }

        LoadGraphics();
    }
    private void Start()
    {
        StartCoroutine("Register");
    }
    IEnumerator Register()
    {
        yield return new WaitForSeconds(0.1f);
        CropsManagerInstance.LoadCrops();
    }
    internal void LoadGraphics()
    {
        Screen.SetResolution(PlayerSettings.RESX, PlayerSettings.RESY, PlayerSettings.FULLSCREEN);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)PlayerSettings.FPS;
        Camera.main.fieldOfView = PlayerSettings.FOV;
    }

    internal bool IsGamePaused { get; private set; }


    [field: Header("PLAYER")]
    [field: SerializeField] internal Transform PlayerTransform { get; private set; }
    [field: SerializeField] internal Inventory PlayerInventory { get; private set; }


    [field: Header("ITEMS")]
    [field: SerializeField] internal List<Item> AllItems { get; private set; }
    [field: SerializeField] internal List<Item> AllLootableItems { get; private set; }
    internal Dictionary<int, Item> AllItemsDictionary { get; private set; } = new Dictionary<int, Item>();


    [field: Header("CROPS")]
    [field: SerializeField] internal List<Plot> AllCrops { get; private set; }
    [field: SerializeField] internal List<PlantObject> AllPlantableObjects { get; private set; }
    [field: SerializeField] internal Item NullItem { get; private set; }
    [field: SerializeField] internal Transform EnvironmentParent { get; private set; }
    internal Dictionary<int, Plot> AllCropsDictionary { get; private set; } = new Dictionary<int, Plot>();


    [field: Header("UI")]
    [field: SerializeField] internal Button ButtonPrefab { get; private set; }
    [field: SerializeField] internal UI_Behaviour HUD { get; private set; }
    [field: SerializeField] internal Transform UIVisibleParent { get; private set; }
    [field: SerializeField] internal UI_Behaviour PauseMenu { get; private set; }
    [field: SerializeField] internal CinemachineVirtualCamera FreeCamera { get; private set; }


    [field: Header("MANAGERS")]
    [field: SerializeField] internal InventoryManager InventoryManagerInstance { get; private set; }
    [field: SerializeField] internal PlayerManager PlayerManagerInstance { get; private set; }
    [field: SerializeField] internal CropsManager CropsManagerInstance { get; private set; }
    [field: SerializeField] internal AudioManager AudioManagerInstance { get; private set; }

    private void OnApplicationQuit()
    {
        InventoryManagerInstance.SaveInventory();
        CropsManagerInstance.SaveCrops();
        PlayerManagerInstance.SavePlayer();
    }

    internal void AddItems()
    {
        foreach (Item item in AllItems)
            PlayerInventory.Add(item);
    }
    internal void Cursor_Needed(CursorLockMode lock_mode)
    {
        if (Cursor.lockState != lock_mode)
            Cursor.lockState = lock_mode;
    }
    public void CursorLocked(bool locked) => Cursor_Needed((locked) ? CursorLockMode.Locked : CursorLockMode.None);
    public void PauseGame()
    {
        Cursor_Needed(CursorLockMode.None);
        IsGamePaused = true;
        HUD.Hide(0);
        PauseMenu.Show(0);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsGamePaused = false;
        HUD.Show(0);
        PauseMenu.Hide(0);
        Cursor_Needed(CursorLockMode.Locked);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        OnApplicationQuit();
        SceneManager.LoadScene("MainMenu");
    }
    internal PlantObject FindPlantObject(int index)
    {
        return AllPlantableObjects.Find(_plant => _plant.PlantObjectIndex == index);
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