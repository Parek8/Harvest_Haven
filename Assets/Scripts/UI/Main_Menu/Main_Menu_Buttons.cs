using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

internal class Main_Menu_Buttons : UI_Behaviour
{
    [field: SerializeField] private UI_Behaviour settings;
    [field: SerializeField] private UI_Behaviour mainMenu;
    [field: SerializeField] private UI_Behaviour audio;
    [field: SerializeField] private UI_Behaviour video;
    [field: SerializeField] private TMP_InputField fov;
    [field: SerializeField] private TMP_InputField fps;
    [field: SerializeField] private TMP_Dropdown res;
    [field: SerializeField] private Toggle fullscreen;
    [field: SerializeField] private PlayerSettings playerSettings;

    public void Show_Settings(bool show)
    {
        if (show)
        {
            settings.Show();
            mainMenu.Hide();
        }
        else
        {
            mainMenu.Show();
            settings.Hide();
        }
    }

    public void ShowAudio(bool show)
    {
        if (show)
        {
            audio.Show();
            settings.Hide();
        }
        else
        {
            settings.Show();
            audio.Hide();
        }
    }

    public void ShowVideo(bool show)
    {
        if (show)
        {
            video.Show();
            settings.Hide();
        }
        else
        {
            settings.Show();
            video.Hide();
        }
    }
    public void ApplySettings()
    {
        // fps (30-240)
        uint fpsv = 20;
        uint.TryParse(fps.text, out fpsv);
        fpsv = (fpsv < 30) ? (uint)30 : (fpsv > 240) ? (uint)240 : fpsv;
        // fov (60-120)
        short fovv = 60;
        Int16.TryParse(fov.text, out fovv);
        fovv = (fovv < 60) ? (short)60 : (fovv > 120) ? (short)120 : fovv;
        // fullscreen
        bool full = fullscreen.isOn;
        // res
        short resXv = 1920;
        short resYv = 1080;

        switch (res.value)
        {
            case 0:
                resXv = 1920;
                resYv = 1080;
                break;
            case 1:
                resXv = 2560;
                resYv = 1440;
                break;
            case 2:
                resXv = 3840;
                resYv = 2160;
                break;
            case 3:
                resXv = 2560;
                resYv = 1080;
                break;
            case 4:
                resXv = 3440;
                resYv = 1440;
                break;
            default: 
                break;
        }

        //Debug.Log($"FOV: {fovv} | FPS: {fpsv} | RES: {resXv}x{resYv} | FULLSCREEN: {full}");
        playerSettings.SetValues(400, fpsv, resXv, resYv, full);
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
    public void Single_Player()
    {
        Scene_Loader.scene_loader.Load_Scene(Scenes.Overworld);
    }
}
