using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class My_Update : MonoBehaviour
{
    [field: Tooltip("How often should My_Update() call. Set the delay in seconds.")]
    [field: SerializeField] float deltaTime = 1f;

    [field: Tooltip("How much In-Game seconds is gonna be each iteration of My_Update().")]
    [field: SerializeField] float in_game_seconds = 600f;

    [field: Tooltip("How fast will the time pass. The smaller the scale, the faster the time. Ex.: 0.5f = 2x the speed.")]
    [field: SerializeField] float time_scale = 1f;

    [field: Tooltip("How long is each tick.")]
    [field: SerializeField] internal float tick { get; private set; } = 0.5f;

    internal static My_Update instance;
    internal static float ingame_seconds { get; private set; }
    // Subscribers, for which will the Custom_Update be called
    private List<Custom_Update_Subscriber> subscribers = new List<Custom_Update_Subscriber>();

    //Add to the subscribers list
    internal void Subscribe(Custom_Update_Subscriber subscriber)
    {
        subscribers.Add(subscriber);
    }
    //Remove from the subscribers list
    internal void Unsubscribe(Custom_Update_Subscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }
    // Find all objects with Custom_Update
    private void Awake()
    {
        if(instance == null)
            instance = this;

        ingame_seconds = in_game_seconds;
        Custom_Update_Subscriber[] subs = FindObjectsOfType<Custom_Update_Subscriber>();
        foreach(Custom_Update_Subscriber sub in subs)
        {
            if (sub != null)
            {
                this.Subscribe(sub);
            }
        }
        
    }
    private void Start()
    {
        StartCoroutine(_Custom_Update());
        Change_Scale(this.time_scale);
    }
    internal void Change_Scale(float scale)
    {
        this.deltaTime = 1/scale;
    }
    private IEnumerator _Custom_Update()
    {
        while (true)
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.Custom_Update();
            }
            yield return new WaitForSeconds(deltaTime);
        }
    }
}