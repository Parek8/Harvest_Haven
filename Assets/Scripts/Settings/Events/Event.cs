using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    void Invoke();
    void Stop();
    float GetChance();
}

public class BloodyMoon : IEvent
{
    public float Chance = -1;
    public GameObject Enemy;
    public bool IsActive = false;
    public List<GameObject> SpawnedMobs = new List<GameObject>();

    public BloodyMoon(float chance, GameObject enemy)
    {
        Chance = chance;
        Enemy = enemy;
        Day_Cycle.On_New_Day_Subscribe(Stop);
    }

    public float GetChance() => Chance;

    public void Invoke()
    {
        int _rnd = Random.Range(10, 50);
        for (int i = 0; i < _rnd; i++)
        {
            SpawnedMobs.Add(Object.Instantiate(Enemy, new Vector3(Random.Range(10, 118), 1, Random.Range(10, 118)), Quaternion.identity));
        }
        IsActive = true;
    }

    public void Stop()
    {
        if (IsActive)
        {
            foreach (GameObject enemy in SpawnedMobs)
                Object.Destroy(enemy);

            SpawnedMobs.Clear();
            IsActive = false;
        }
    }
}