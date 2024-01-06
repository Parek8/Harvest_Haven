using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEditor.Build.Reporting;
using UnityEngine.Rendering.UI;

internal class Scene_Loader : MonoBehaviour
{
    internal static Scene_Loader scene_loader;

    static float cooldown = 5f;
    [SerializeField] UI_Behaviour current_loading_screen;
    [SerializeField] Loading_Percentage percentage;

    private void Awake()
    {
        if (scene_loader == null)
            scene_loader = this;
        else
            Destroy(gameObject);
        
    }
    IEnumerator Load_Scenes(Scenes scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        float delay = 0.1f;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        asyncLoad.allowSceneActivation = false;

        current_loading_screen.Show();

        while (!asyncLoad.isDone)
        {
            cooldown -= delay;
            float prog = asyncLoad.progress;
            if (prog is >= 0.9f && cooldown is <= 0)
            {
                asyncLoad.allowSceneActivation = true;
                //StartCoroutine(Unload_Scenes(SceneManager.GetActiveScene().buildIndex));
            }
            percentage.Fill(prog);

            yield return new WaitForSecondsRealtime(delay);
        }
    }
    IEnumerator Unload_Scenes(Scenes scene, UnloadSceneOptions mode = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
    {
        float delay = 0.1f;
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(scene.ToString());
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            cooldown -= delay;
            if (asyncLoad.progress is >= 0.9f && cooldown is <= 0)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return new WaitForSecondsRealtime(delay);
        }
    }
    IEnumerator Unload_Scenes(int index, UnloadSceneOptions mode = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
    {
        float delay = 0.1f;
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(index);
        asyncLoad.allowSceneActivation = false;


        while (!asyncLoad.isDone)
        {
            cooldown -= delay;
            if (asyncLoad.progress is >= 0.9f && cooldown is <= 0)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return new WaitForSecondsRealtime(delay);
        }
    }
    internal void Load_Scene(Scenes scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        StartCoroutine(Load_Scenes(scene, mode));
    }
}