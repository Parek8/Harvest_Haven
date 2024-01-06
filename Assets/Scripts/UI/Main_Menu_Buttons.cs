using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Main_Menu_Buttons : UI_Behaviour
{
    [SerializeField] private UI_Behaviour settings;
    [SerializeField] private UI_Behaviour pause_menu;

    static Action On_Exit;
    private void Start()
    {

    }
    internal void Show_Settings(bool show)
    {
        if (show)
        {
            pause_menu.Hide();
            settings.Show();
        }
        else
        {
            pause_menu.Show();
            settings.Hide();
        }
    }
    internal void Pause_Game(bool pause)
    {
        if (pause)
        {
            pause_menu.Show();
        }
        else
        {
            pause_menu.Hide();
            settings.Hide();
        }
    }
    internal void Back_To_Menu()
    {
        Scene_Loader.scene_loader.Load_Scene(Scenes.Main_Menu, LoadSceneMode.Additive);
        On_Exit?.Invoke();
    }

    internal static void Subscribe_To_On_Exit(Action ac)
    {
        On_Exit += ac;
    }
    internal static void Unsubscribe_To_On_Exit(Action ac)
    {
        On_Exit = ac;
    }
    internal void Exit()
    {
        Application.Quit();
        //Exit the editor playmode -> checking, if you're using UNITY_EDITOR
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        //EditorApplication.Exit(200);
#endif
    }
    internal void Single_Player()
    {
        Scene_Loader.scene_loader.Load_Scene(Scenes.Overworld);
    }
}
