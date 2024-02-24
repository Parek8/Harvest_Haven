using System;
using UnityEngine;

internal class PlayerStats : Character_Stats
{
    [field: Header("Player Movement Variables: ")]
    [field: SerializeField] internal float jump_force { get; private set; } = 100f;
    [field: SerializeField] internal float pick_up_distance { get; private set; } = 1.2f;
    [field: SerializeField] internal LayerMask destroyable_layers { get; private set; }
    [field: SerializeField] internal LayerMask highlightable_layers { get; private set; }
    [field: SerializeField] internal LayerMask interactable_layers { get; private set; }
    [field: SerializeField] internal LayerMask plot_layers { get; private set; }

    [field: Header("Player Health Variables: ")]
    [field: SerializeField] internal float food_delay { get; private set; } = 10f;
    [field: SerializeField] internal float on_hunger_hit_delay { get; private set; } = 10f;
    [field: SerializeField] internal float max_food_points { get; private set; } = 10f;
    [field: SerializeField] internal OnHit OnHit { get; private set; }
    internal float current_food_points { get; private set; } = 10f;

    private PlayerState _state = PlayerState.normal;
    private Action<PlayerState> OnPlayerStateChange;
    private new void Start()
    {
        base.Start();
        current_health_points = max_health_points;
        current_food_points = max_food_points;
        GameManager.game_manager.PlayerManagerInstance.LoadPlayer();
    }
    internal override void Reduce_Health(float reduce_hp)
    {
        base.Reduce_Health(reduce_hp);
        OnHit.StartAnimation();
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

    internal void LoadPlayer(float movementSpeed, float jumpForce, float pickUpDistance, float attackDistance, float attackDamage, float foodDelay, float onHungerHitDelay, float maxHelthPoints, float maxFoodPoints, float currentHealthPoint, float currentFoodPoints)
    {
        this.movement_speed = movementSpeed;
        this.jump_force = jumpForce;
        this.pick_up_distance = pickUpDistance;
        this.attack_distance = attackDistance;
        this.attack_damage = attackDamage;
        this.food_delay = foodDelay;
        this.on_hunger_hit_delay = onHungerHitDelay;
        this.max_health_points = maxHelthPoints;
        this.max_food_points = maxFoodPoints;
        this.current_health_points = currentHealthPoint;
        this.current_food_points = currentFoodPoints;
    }

    internal void AddPlayerStateListener(Action<PlayerState> _listener) => this.OnPlayerStateChange += _listener;
    internal void RemovePlayerStateListener(Action<PlayerState> _listener) => this.OnPlayerStateChange -= _listener;
}