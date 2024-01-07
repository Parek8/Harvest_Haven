using UnityEditor;
using UnityEngine;

internal class Main_Menu_Buttons : UI_Behaviour
{
    [SerializeField] private UI_Behaviour settings;
    [SerializeField] private UI_Behaviour mainMenu;

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
