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
    [field: SerializeField] public LayerMask plot_layers { get; private set; }
    [field: SerializeField] public float attack_damage { get; private set; } = 10f;

    [field: Header("Player Health Variables: ")]
    [field: SerializeField] public float food_delay { get; private set; } = 10f;
    [field: SerializeField] public float on_hunger_hit_delay { get; private set; } = 10f;
    [field: SerializeField] public float max_health_points { get; private set; } = 10f;
    [field: SerializeField] public float max_food_points { get; private set; } = 10f;
    public float current_health_points { get; private set; } = 10f;
    public float current_food_points { get; private set; } = 10f;

    private PlayerState _state = PlayerState.normal;
    private Action<PlayerState> OnPlayerStateChange;

    private void Start()
    {
        current_health_points = max_health_points;
        current_food_points = max_food_points;
    }

    public void Reduce_Health(float reduce_hp)
    {
        current_health_points -= reduce_hp;
        if(current_health_points <= 0)
        {
            current_health_points = 0;
            Debug.Log("You've died!");
        }
    }

    public void Starve(float starve)
    {
        if (current_food_points > 0)
            current_food_points -= starve;
        if (current_food_points <= 0)
            current_food_points = 0;
        if (current_food_points <= 0)
            Damage_On_Hunger();
    }

    public void Saturate(float saturate)
    {
        current_food_points += saturate;
        if (current_food_points > max_food_points)
            current_food_points = max_food_points;
    }

    private void Damage_On_Hunger()
    {
        Reduce_Health(1.0f);
    }

    public void Change_State()
    {
        _state = (_state == PlayerState.normal) ? PlayerState.seeding : PlayerState.normal;
        OnPlayerStateChange?.Invoke(_state);
    }
    public void AddPlayerStateListener(Action<PlayerState> _listener) => this.OnPlayerStateChange += _listener;
    public void RemovePlayerStateListener(Action<PlayerState> _listener) => this.OnPlayerStateChange -= _listener;
}