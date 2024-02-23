using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

internal class SceneLoader : MonoBehaviour
{
    internal static SceneLoader SceneLoaderInstance => _sceneLoaderInstance;

    static SceneLoader _sceneLoaderInstance;
    static float _cooldown = 5f;
    [SerializeField] UIBehaviour _currentLoadingScreen;
    [SerializeField] Loading_Percentage _percentage;

    private void Awake()
    {
        if (_sceneLoaderInstance == null)
            _sceneLoaderInstance = this;
        else
            Destroy(gameObject);
    }

    IEnumerator LoadScenes(Scenes scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        float _delay = 0.1f;
        AsyncOperation _asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        _asyncLoad.allowSceneActivation = false;

        _currentLoadingScreen.Show();

        while (!_asyncLoad.isDone)
        {
            _cooldown -= _delay;
            float prog = _asyncLoad.progress;
            if (prog is >= 0.9f && _cooldown is <= 0)
            {
                _asyncLoad.allowSceneActivation = true;
                //StartCoroutine(Unload_Scenes(SceneManager.GetActiveScene().buildIndex));
            }
            _percentage.Fill(prog);

            yield return new WaitForSecondsRealtime(_delay);
        }
    }
    IEnumerator UnloadScenes(Scenes scene, UnloadSceneOptions mode = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
    {
        float _delay = 0.1f;
        AsyncOperation _asyncLoad = SceneManager.UnloadSceneAsync(scene.ToString());
        _asyncLoad.allowSceneActivation = false;

        while (!_asyncLoad.isDone)
        {
            _cooldown -= _delay;
            if (_asyncLoad.progress is >= 0.9f && _cooldown is <= 0)
            {
                _asyncLoad.allowSceneActivation = true;
            }

            yield return new WaitForSecondsRealtime(_delay);
        }
    }
    IEnumerator UnloadScenes(int index, UnloadSceneOptions mode = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
    {
        float _delay = 0.1f;
        AsyncOperation _asyncLoad = SceneManager.UnloadSceneAsync(index);
        _asyncLoad.allowSceneActivation = false;


        while (!_asyncLoad.isDone)
        {
            _cooldown -= _delay;
            if (_asyncLoad.progress is >= 0.9f && _cooldown is <= 0)
            {
                _asyncLoad.allowSceneActivation = true;
            }

            yield return new WaitForSecondsRealtime(_delay);
        }
    }
    internal void LoadScene(Scenes scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadScenes(scene, mode));
    }
}