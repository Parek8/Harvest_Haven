using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Menu_Buttons_Manager : MonoBehaviour
{
    internal void Play()
    {
        // load world mechanics yet to be implemented
        SceneManager.LoadScene(Scenes.Overworld.ToString());
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
}
