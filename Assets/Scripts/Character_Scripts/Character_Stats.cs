using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stats : MonoBehaviour
{
    [field: Header("Player Movement Variables: ")]
    [field: SerializeField] public float movement_speed { get; private set; } = 15f; 
    [field: SerializeField] public float jump_force { get; private set; } = 100f;
    [field: SerializeField] public float pick_up_distance { get; private set; } = 1.2f;
    [field: SerializeField] public float attack_distance { get; private set; } = 1.2f;
    [field: SerializeField] public LayerMask destroyable_layers { get; private set; }
    [field: SerializeField] public float attack_damage { get; private set; } = 10f;

    [field: Header("Player Health Variables: ")]
    [field: SerializeField] public float food_delay { get; private set; } = 10f;
    [field: SerializeField] public float on_hunger_hit_delay { get; private set; } = 10f;
    [field: SerializeField] public float max_health_points { get; private set; } = 10f;
    [field: SerializeField] public float max_food_points { get; private set; } = 10f;
    public float current_health_points { get; private set; } = 10f;
    public float current_food_points { get; private set; } = 10f;

    private void Start()
    {
        current_health_points = max_health_points;
        current_food_points = max_food_points;
    }

    public void Reduce_Health(float reduce_hp)
    {
        current_health_points -= reduce_hp;
        if(current_food_points <= 0)
        {
            Debug.Log("You've died!");
        }
    }

    public void Reduce_Food(float reduce_food)
    {
        current_food_points -= reduce_food;
        if (current_food_points <= 0)
            StartCoroutine(Damage_On_Hunger());
    }

    private IEnumerator Damage_On_Hunger()
    {
        while(current_food_points <= 0) 
        {
            Reduce_Health(1.0f);
            yield return new WaitForSeconds(on_hunger_hit_delay);
        }
    }
}