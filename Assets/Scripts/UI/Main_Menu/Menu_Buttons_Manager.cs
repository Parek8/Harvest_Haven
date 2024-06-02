using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class Menu_Buttons_Manager : MonoBehaviour
{
    [field: SerializeField] Image Curtain;
    [field: SerializeField] float OpeningTime;

    private float m_CurrentTime = 0;
    private void Start()
    {
        StartCoroutine(OpenCurtain());
    }

    IEnumerator OpenCurtain()
    {
        while (m_CurrentTime < OpeningTime)
        {
            m_CurrentTime += Time.deltaTime;
            Curtain.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), m_CurrentTime/OpeningTime);

            yield return new WaitForEndOfFrame();
        }
    }
    public void Play()
    {
        // load world mechanics yet to be implemented
        SceneManager.LoadScene(Scenes.Overworld.ToString());
    }

    public void Exit()
    {
        Application.Quit();
        //Exit the editor playmode -> checking, if you're using UNITY_EDITOR
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        //EditorApplication.Exit(200);
        #endif
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
}
