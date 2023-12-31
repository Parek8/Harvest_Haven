using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Character_Stats : MonoBehaviour
{
    [field: Header("Player Movement Variables: ")]
    [field: SerializeField] internal float movement_speed { get; private set; } = 15f; 
    [field: SerializeField] internal float jump_force { get; private set; } = 100f;
    [field: SerializeField] internal float pick_up_distance { get; private set; } = 1.2f;
    [field: SerializeField] internal float attack_distance { get; private set; } = 1.2f;
    [field: SerializeField] internal LayerMask destroyable_layers { get; private set; }
    [field: SerializeField] internal LayerMask highlightable_layers { get; private set; }
    [field: SerializeField] internal LayerMask interactable_layers { get; private set; }
    [field: SerializeField] internal LayerMask plot_layers { get; private set; }
    [field: SerializeField] internal float attack_damage { get; private set; } = 10f;

    [field: Header("Player Health Variables: ")]
    [field: SerializeField] internal float food_delay { get; private set; } = 10f;
    [field: SerializeField] internal float on_hunger_hit_delay { get; private set; } = 10f;
    [field: SerializeField] internal float max_health_points { get; private set; } = 10f;
    [field: SerializeField] internal float max_food_points { get; private set; } = 10f;
    internal float current_health_points { get; private set; } = 10f;
    internal float current_food_points { get; private set; } = 10f;

    private PlayerState _state = PlayerState.normal;
    private Action<PlayerState> OnPlayerStateChange;

    private void Start()
    {
        current_health_points = max_health_points;
        current_food_points = max_food_points;
    }

    internal void Reduce_Health(float reduce_hp)
    {
        current_health_points -= reduce_hp;
        if(current_health_points <= 0)
        {
            current_health_points = 0;
            Debug.Log("You've died!");
        }
    }

    internal void Starve(float starve)
    {
        if (current_food_points > 0)
            current_food_points -= starve;
        if (current_food_points <= 0)
            current_food_points = 0;
        if (current_food_points <= 0)
            Damage_On_Hunger();
    }

    internal void Saturate(Item food)
    {
        float saturate = food.FoodRegen;
        current_food_points += saturate;
        if (current_food_points > max_food_points)
            current_food_points = max_food_points;
    }

    private void Damage_On_Hunger()
    {
        Reduce_Health(1.0f);
    }

    internal void Change_State(Item item)
    {
        if (item.IsPlantable)
            _state = PlayerState.seeding;
        else
            _state = PlayerState.normal;

        OnPlayerStateChange?.Invoke(_state);
    }
    internal void AddPlayerStateListener(Action<PlayerState> _listener) => this.OnPlayerStateChange += _listener;
    internal void RemovePlayerStateListener(Action<PlayerState> _listener) => this.OnPlayerStateChange -= _listener;
}