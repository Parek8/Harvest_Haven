using System.Collections.Generic;
using UnityEngine;

namespace BgTools.PlayerPrefsEditor
{
    [System.Serializable]
    internal class PreferenceEntryHolder : ScriptableObject
    {
        internal List<PreferenceEntry> userDefList;
        internal List<PreferenceEntry> unityDefList;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontSave;
            if (userDefList == null)
                userDefList = new List<PreferenceEntry>();
            if (unityDefList == null)
                unityDefList = new List<PreferenceEntry>();
        }

        internal void ClearLists()
        {
            if (userDefList != null)
                userDefList.Clear();
            if (unityDefList != null)
                unityDefList.Clear();
        }
    }
}