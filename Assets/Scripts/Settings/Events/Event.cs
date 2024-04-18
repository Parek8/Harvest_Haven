using UnityEngine;

public interface IEvent
{
    void Invoke();
    float GetChance();
}

public class BloodyMoon : MonoBehaviour, IEvent
{
    public float Chance = -1;
    public GameObject Enemy;
    public BloodyMoon(float chance, GameObject enemy)
    {
        Chance = chance;
        Enemy = enemy;
    }

    public float GetChance() => Chance;

    public void Invoke()
    {
        int _rnd = Random.Range(10, 50);
        for (int i = 0; i < _rnd; i++)
        {
            Instantiate(Enemy, new Vector3(Random.Range(10, 118), 1, Random.Range(10, 118)), Quaternion.identity);
        }
    }
}