using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal class AxisChanger : MonoBehaviour
{
    void Start()
    {
        // Získání odkazu na Input_Manager
        SerializedObject Input_Manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Input_Manager.asset")[0]);
        SerializedProperty axesProperty = Input_Manager.FindProperty("m_Axes");

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

        // Uložení zmìn v Input_Manageru
        Input_Manager.ApplyModifiedProperties();
    }
}
