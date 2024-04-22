using System.Collections.Generic;
using UnityEngine;

public sealed class EventManager : MonoBehaviour
{
    private static EventManager _eventManagerInstance;
    public static EventManager EventManagerInstance => _eventManagerInstance;

    public GameObject Enemy;
    List<IEvent> Events = new();
    void Awake()
    {
        if (_eventManagerInstance == null)
            _eventManagerInstance = this;

        Events.Add(new BloodyMoon(10, Enemy));
    }
    private void Start()
    {
        Day_Cycle.On_New_Day_Subscribe(TriggerEvent);
    }

    void TriggerEvent()
    {
        foreach (var e in Events)
        {
            if (Random.Range(0, 101) <= e.GetChance())
            {
                e.Invoke();
                return;
            }
        }
    }
}