using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Character_Stats))]
internal class Player_Health : MonoBehaviour
{
    [field: SerializeField] List<Image> hp_sprites = new List<Image>();
    [field: SerializeField] List<Image> hunger_sprites = new List<Image>();
    PlayerStats stats;
    float current_FoodDelay = 20;
    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        current_FoodDelay = stats.FoodDelay;
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
            yield return new WaitForSeconds(current_FoodDelay);
            Reduce_Hunger();
        }
    }

    private void Reduce_Hunger()
    {
        stats.Starve(1.0f);
    }
    internal void Hit()
    {
        // todo
    }
    internal void Update_UI()
    {
        float _hunger = stats.CurrentFoodPoints;
        for (int _i = 0; _i < stats.MaxHealthPoints; _i++)
        {
            hunger_sprites[(int)_i].fillAmount = 1f;
            hp_sprites[(int)_i].fillAmount = 1f;

        }

        for (float _i = _hunger; _i < stats.MaxFoodPoints; _i++) 
        {
            hunger_sprites[(int)_i].fillAmount = 0;
        }


        float _hp = stats.CurrentHealthPoints;

        for (float _i = _hp; _i < stats.MaxHealthPoints; _i++)
        {
            hp_sprites[(int)_i].fillAmount = 0;
        }
    }
}
