using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [field: SerializeField] List<uint> times = new List<uint>();
    [field: SerializeField] List<GameObject> stages = new List<GameObject>();

    private void Start()
    {
        Day_Cycle.On_New_Day_Subscribe(OnDayChange);    
    }

    public void OnDayChange()
    {
        for (int i = 0; i < times.Count; i++)
        {
            times[i]--;
        }

        if (times[0] <= 0)
            SpawnNewStage();
    }
    private void SpawnNewStage()
    {
        Destroy(transform.GetChild(0));

        Instantiate(stages[0]);

        times.RemoveAt(0);
        stages.RemoveAt(0);
    }
}