using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AxisChanger : MonoBehaviour
{
    void Start()
    {
        // Získání odkazu na InputManager
        SerializedObject inputManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = inputManager.FindProperty("m_Axes");

        // Procházení všech os a hledání osy "Horizontal"
        for (int i = 0; i < axesProperty.arraySize; i++)
        {
            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(i);
            SerializedProperty nameProperty = axisProperty.FindPropertyRelative("m_Name");

            // Porovnání jména osy s "Horizontal"
            if (nameProperty.stringValue == "Horizontal")
            {
                //SerializedProperty axisProperty = axisProperty.FindPropertyRelative("axis");

                // Zmìna osy na novou hodnotu
                axisProperty.intValue = 2; // 2 reprezentuje tøetí osu, mùžete zde použít jakoukoliv osu dle svých potøeb
                break;
            }
        }

        // Uložení zmìn v InputManageru
        inputManager.ApplyModifiedProperties();
    }
}
