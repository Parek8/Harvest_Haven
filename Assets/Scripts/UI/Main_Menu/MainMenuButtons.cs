using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class MainMenuButtons : UIBehaviour
{
    [field: SerializeField] private UIBehaviour Settings;
    [field: SerializeField] private UIBehaviour MainMenu;
    [field: SerializeField] private UIBehaviour Audio;
    [field: SerializeField] private UIBehaviour Video;
    [field: SerializeField] private TMP_InputField FOV;
    [field: SerializeField] private TMP_InputField FPS;
    [field: SerializeField] private TMP_Dropdown RES;
    [field: SerializeField] private Toggle FULL;
    [field: SerializeField] private PlayerSettings PlayerSettings;

    public void ShowSettings(bool show)
    {
        if (show)
        {
            Settings.Show();
            MainMenu.Hide();
        }
        else
        {
            MainMenu.Show();
            Settings.Hide();
        }
    }

    public void ShowAudio(bool show)
    {
        if (show)
        {
            Audio.Show();
            Settings.Hide();
        }
        else
        {
            Settings.Show();
            Audio.Hide();
        }
    }

    public void ShowVideo(bool show)
    {
        if (show)
        {
            Video.Show();
            Settings.Hide();
        }
        else
        {
            Settings.Show();
            Video.Hide();
        }
    }
    public void ApplySettings()
    {
        // fps (30-240)
        uint _fpsv = 20;
        uint.TryParse(FPS.text, out _fpsv);
        _fpsv = (_fpsv < 30) ? (uint)30 : (_fpsv > 240) ? (uint)240 : _fpsv;
        // fov (60-120)
        short _fovv = 60;
        Int16.TryParse(FOV.text, out _fovv);
        _fovv = (_fovv < 60) ? (short)60 : (_fovv > 120) ? (short)120 : _fovv;
        // fullscreen
        bool _full = FULL.isOn;
        // res
        short _resXv = 1920;
        short _resYv = 1080;

        switch (RES.value)
        {
            case 0:
                _resXv = 1920;
                _resYv = 1080;
                break;
            case 1:
                _resXv = 2560;
                _resYv = 1440;
                break;
            case 2:
                _resXv = 3840;
                _resYv = 2160;
                break;
            case 3:
                _resXv = 2560;
                _resYv = 1080;
                break;
            case 4:
                _resXv = 3440;
                _resYv = 1440;
                break;
            default: 
                break;
        }

        //Debug.Log($"FOV: {fovv} | FPS: {fpsv} | RES: {resXv}x{resYv} | FULLSCREEN: {full}");
        PlayerSettings.SetValues(_fovv, _fpsv, _resXv, _resYv, _full);
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
        SceneManager.LoadScene(Scenes.Overworld.ToString());
    }
}
