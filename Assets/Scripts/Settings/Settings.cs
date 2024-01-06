using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class Settings : MonoBehaviour
{
    //[SerializeField] private Settings_Button forward_button;
    //[SerializeField] private Settings_Button backward_button;
    //[SerializeField] private Settings_Button left_strafe_button;
    //[SerializeField] private Settings_Button right_strafe_button;
    //[SerializeField] private Settings_Button sprint_button;
    //[SerializeField] private Settings_Button mouse_sensitivity_slider;
    [SerializeField] List<Settings_Button> Keybinds; 

    //int mouse_sensitivity { get { return mouse_sensitivity.sensitivity; } }
    internal void RevertToDefault()
    {
        //DON'T FORGET TO IMPLEMENT!
    }
}