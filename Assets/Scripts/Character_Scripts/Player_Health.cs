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
    private void Update()
    {
        Update_UI();
    }
    private IEnumerator Hunger()
    {
        while(true)
        {
            yield return new WaitForSeconds(current_food_delay);
            Reduce_Hunger();
        }
    }

    private void Reduce_Hunger()
    {
        stats.Starve(1.0f);
    }
    public void Hit()
    {
        // todo
    }
    public void Update_UI()
    {
        float _hunger = stats.current_food_points;
        for (int _i = 0; _i < stats.max_health_points; _i++)
        {
            hunger_sprites[(int)_i].fillAmount = 1f;
            hp_sprites[(int)_i].fillAmount = 1f;

        }

        for (float _i = _hunger; _i < stats.max_food_points; _i++) 
        {
            hunger_sprites[(int)_i].fillAmount = 0;
        }


        float _hp = stats.current_health_points;

        for (float _i = _hp; _i < stats.max_health_points; _i++)
        {
            hp_sprites[(int)_i].fillAmount = 0;
        }
    }
}
