using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class Day_Cycle : Custom_Update_Subscriber
{
    [field: SerializeField] TMP_Text time_text;
    [field: SerializeField] Image clock;
    [field: SerializeField] AnimationCurve time_curve;
    [field: SerializeField] Color day_color;
    [field: SerializeField] Color night_color;
    [field: SerializeField] Light global_light;
    [field: SerializeField] float starting_seconds = 14400f;

    private static float seconds = 0;
    private static float seconds_in_a_day = 24 * 60 * 60; // H * M * S
    private static int days = 1;

    private static Action On_New_Day;
    static float countdown = 24 * 60 * 60;
    static float s_sec = 0;
    private void Start()
    {
        seconds = starting_seconds;
        s_sec = starting_seconds;
        Update_Time_HUD();
        Update_Light();
    }
    public override void Custom_Update()
    {
        countdown -= My_Update.ingame_seconds;
        seconds += My_Update.ingame_seconds;

        if (seconds >= seconds_in_a_day)
            seconds = 0;
        if (countdown <= 0)
            Next_Day();
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
    }
    public static int Return_Hours()
    {
        return (int)Mathf.Floor(seconds / 3600);
    }
    public static int Return_Minutes()
    {
        return (int)Mathf.Floor((seconds - (Return_Hours() * 3600)) / 60);
    }
    public static void Next_Day()
    {
        days++;
        countdown = seconds_in_a_day;
        On_New_Day?.Invoke();
        Debug.Log("New day " + days);
    }

    public static void On_New_Day_Subscribe(Action action)
    {
        On_New_Day += action;
    }
    public static void On_New_Day_Unsubscribe(Action action)
    {
        On_New_Day -= action;
    }
    public static void Sleep()
    {
        Debug.Log("You slept well");
        Next_Day();
        seconds = s_sec;
    }
}