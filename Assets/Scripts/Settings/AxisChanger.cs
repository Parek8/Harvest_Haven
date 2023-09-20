using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AxisChanger : MonoBehaviour
{
    void Start()
    {
        // Z�sk�n� odkazu na InputManager
        SerializedObject inputManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = inputManager.FindProperty("m_Axes");

        // Proch�zen� v�ech os a hled�n� osy "Horizontal"
        for (int i = 0; i < axesProperty.arraySize; i++)
        {
            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(i);
            SerializedProperty nameProperty = axisProperty.FindPropertyRelative("m_Name");

            // Porovn�n� jm�na osy s "Horizontal"
            if (nameProperty.stringValue == "Horizontal")
            {
                //SerializedProperty axisProperty = axisProperty.FindPropertyRelative("axis");

                // Zm�na osy na novou hodnotu
                axisProperty.intValue = 2; // 2 reprezentuje t�et� osu, m��ete zde pou��t jakoukoliv osu dle sv�ch pot�eb
                break;
            }
        }

        // Ulo�en� zm�n v InputManageru
        inputManager.ApplyModifiedProperties();
    }
}
