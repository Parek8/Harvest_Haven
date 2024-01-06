using UnityEngine;

internal class PlayerPrefsController : MonoBehaviour
{

    #region Add
    internal void AddTestStrings()
    {
        PlayerPrefs.SetString("Runtime_String", "boing");
        PlayerPrefs.SetString("Runtime_String2", "foo");
        PlayerPrefs.Save();
    }

    internal void AddTestInt()
    {
        PlayerPrefs.SetInt("Runtime_Int", 1234);
        PlayerPrefs.Save();
    }

    internal void AddTestFloat()
    {
        PlayerPrefs.SetFloat("Runtime_Float", 3.14f);
        PlayerPrefs.Save();
    }
    #endregion

    #region Remove
    internal void RemoveTestStrings()
    {
        PlayerPrefs.DeleteKey("Runtime_String");
        PlayerPrefs.DeleteKey("Runtime_String2");
        PlayerPrefs.Save();
    }

    internal void RemoveTestInt()
    {
        PlayerPrefs.DeleteKey("Runtime_Int");
        PlayerPrefs.Save();
    }

    internal void RemoveTestFloat()
    {
        PlayerPrefs.DeleteKey("Runtime_Float");
        PlayerPrefs.Save();
    }

    internal void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    #endregion
}