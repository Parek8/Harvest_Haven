using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character_Stats))]
public class Player_Health : MonoBehaviour
{
    //[field: SerializeField] 
    Character_Stats stats;
    float current_food_delay = 20;
    private void Start()
    {
        stats = GetComponent<Character_Stats>();
        current_food_delay = stats.food_delay;
        StartCoroutine(Hunger());
    }
    private IEnumerator Hunger()
    {
        while(true)
        {
            Reduce_Hunger();
            yield return new WaitForSeconds(current_food_delay);
        }
    }

    private void Reduce_Hunger()
    {
        stats.Reduce_Food(1.0f);
    }
    public void Hit()
    {

    }
    public void Update_UI()
    {
        int hp = (int)stats.current_health_points;
        int hunger = (int)stats.current_food_points;



    }
}
