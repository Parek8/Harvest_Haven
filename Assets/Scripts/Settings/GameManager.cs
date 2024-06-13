using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class GameManager : MonoBehaviour
{
    [field: Header("CURTAIN")]
    internal static GameManager GameManagerInstance { get; private set; }
    [field: SerializeField] internal PlayerSettings PlayerSettings { get; private set; }
    [field: SerializeField] internal Image Curtain { get; private set; }
    [field: SerializeField] internal float OpeningTime { get; private set; } = 1f;
    [field: SerializeField] internal float OpeningDelay { get; private set; } = 1f;

    private float m_CurrentTime = 0;

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
    private void OnEnable()
    {
        StartCoroutine(OpenCurtain());
    }

    IEnumerator OpenCurtain()
    {
        Curtain.gameObject.SetActive(true);
        Curtain.material.SetFloat("_CurtainTransitionProgress", 0);
        //yield return new WaitForSeconds(OpeningDelay);

        while (m_CurrentTime < OpeningTime)
        {
            m_CurrentTime += Time.deltaTime;
            Curtain.material.SetFloat("_CurtainTransitionProgress", (m_CurrentTime / OpeningTime)*2);

            yield return new WaitForEndOfFrame();
        }
        Debug.Log(Curtain.material.GetFloat("_CurtainTransitionProgress"));
        Curtain.gameObject.SetActive(false);
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


        AudioListener.volume = PlayerSettings.VOLUME;
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