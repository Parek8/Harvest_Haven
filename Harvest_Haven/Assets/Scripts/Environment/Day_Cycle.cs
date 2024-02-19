using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
internal class Day_Cycle : Custom_Update_Subscriber
{
    [field: SerializeField] TMP_Text time_text;
    [field: SerializeField] Image clock;
    [field: SerializeField] AnimationCurve time_curve;
    [field: SerializeField] Color day_color;
    [field: SerializeField] Color night_color;
    [field: SerializeField] Light global_light;
    [field: SerializeField] float starting_seconds = 14400f;
    [field: SerializeField] Material SunriseSkybox;
    [field: SerializeField] Material DaySkybox;
    [field: SerializeField] Material SunsetSkybox;
    [field: SerializeField] Material NightSkybox;

    private static float seconds = 0;
    private static float seconds_in_a_day = 24 * 60 * 60; // H * M * S
    private static int days = 1;

    private static Action On_New_Day;
    static float s_sec = 0;
    private void Start()
    {
        seconds = starting_seconds;
        s_sec = starting_seconds;
        Update_Time_HUD();
        Update_Light();
    }
    internal override void Custom_Update()
    {
        seconds += My_Update.ingame_seconds;

        if (seconds >= seconds_in_a_day)
        {
            seconds = 0;
            Next_Day();
        }
        Update_Time_HUD();
        Update_Light();
    }
    private void Update_Time_HUD()
    {
        time_text.text = $"Day: {days}\nTime: " + Return_Hours().ToString("00") + ":" + Return_Minutes().ToString("00");
        clock.fillAmount = seconds / seconds_in_a_day;
    }
    private void Update_Light()
    {
        float v = time_curve.Evaluate(seconds / 3600);
        Color c = Color.Lerp(day_color, night_color, v);
        global_light.color = c;

        if ((seconds / 3600) > 4 && (seconds / 3600) < 5)
            RenderSettings.skybox = SunriseSkybox;
        else if ((seconds / 3600) >= 5 && (seconds / 3600) < 21)
            RenderSettings.skybox = DaySkybox;
        else if ((seconds / 3600) >= 21 && (seconds / 3600) < 22)
            RenderSettings.skybox = SunsetSkybox;
        else
            RenderSettings.skybox = NightSkybox;
    }
    internal static int Return_Hours()
    {
        return (int)Mathf.Floor(seconds / 3600);
    }
    internal static int Return_Minutes()
    {
        return (int)Mathf.Floor((seconds - (Return_Hours() * 3600)) / 60);
    }
    internal static void Next_Day()
    {
        days++;
        On_New_Day?.Invoke();
        Debug.Log("New day " + days);
    }

    internal static void On_New_Day_Subscribe(Action action)
    {
        On_New_Day += action;
    }
    internal static void On_New_Day_Unsubscribe(Action action)
    {
        On_New_Day -= action;
    }
    internal static void Sleep()
    {
        Debug.Log("You slept well");
        Next_Day();
        seconds = s_sec;
    }
}