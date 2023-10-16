using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Character_Stats))]
public class Player_Health : MonoBehaviour
{
    [field: SerializeField] List<Image> hp_sprites = new List<Image>();
    [field: SerializeField] List<Image> hunger_sprites = new List<Image>();
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
        Update_UI();
    }
    public void Hit()
    {
        Update_UI();
    }
    public void Update_UI()
    {
        float hp = stats.current_health_points;
        float hunger = stats.current_food_points;

        
    }
}
