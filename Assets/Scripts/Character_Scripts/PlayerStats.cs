using System;
using UnityEngine;

internal class PlayerStats : Character_Stats
{
    [field: Header("Player Movement Variables: ")]
    [field: SerializeField] internal float JumpForce { get; private set; } = 100f;
    [field: SerializeField] internal float PickUpDistance { get; private set; } = 1.2f;
    [field: SerializeField] internal LayerMask DestroyableLayers { get; private set; }
    [field: SerializeField] internal LayerMask HighlightableLayers { get; private set; }
    [field: SerializeField] internal LayerMask InteractableLayers { get; private set; }
    [field: SerializeField] internal LayerMask PlotLayers { get; private set; }

    [field: Header("Player Health Variables: ")]
    [field: SerializeField] internal float FoodDelay { get; private set; } = 10f;
    [field: SerializeField] internal float OnHungerHitDelay { get; private set; } = 10f;
    [field: SerializeField] internal float MaxFoodPoints { get; private set; } = 10f;
    [field: SerializeField] internal OnHit OnHit { get; private set; }
    internal float CurrentFoodPoints { get; private set; } = 10f;

    private PlayerState _state = PlayerState.normal;
    private Action<PlayerState> OnPlayerStateChange;
    private new void Start()
    {
        base.Start();
        CurrentHealthPoints = MaxHealthPoints;
        CurrentFoodPoints = MaxFoodPoints;
        GameManager.game_manager.PlayerManagerInstance.LoadPlayer();
    }
    internal override void Reduce_Health(float reduce_hp)
    {
        OnHit.StartAnimation();
        base.Reduce_Health(reduce_hp);
    }


    internal void Starve(float starve)
    {
        if (CurrentFoodPoints > 0)
            CurrentFoodPoints -= starve;
        if (CurrentFoodPoints <= 0)
            CurrentFoodPoints = 0;
        if (CurrentFoodPoints <= 0)
            Damage_On_Hunger();
    }

    internal void Saturate(Item food)
    {
        float saturate = food.FoodRegen;
        CurrentFoodPoints += saturate;
        if (CurrentFoodPoints > MaxFoodPoints)
            CurrentFoodPoints = MaxFoodPoints;
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
        this.MovementSpeed = movementSpeed;
        this.JumpForce = jumpForce;
        this.PickUpDistance = pickUpDistance;
        this.AttackDistance = attackDistance;
        this.AttackDamage = attackDamage;
        this.FoodDelay = foodDelay;
        this.OnHungerHitDelay = onHungerHitDelay;
        this.MaxHealthPoints = maxHelthPoints;
        this.MaxFoodPoints = maxFoodPoints;
        this.CurrentHealthPoints = currentHealthPoint;
        this.CurrentFoodPoints = currentFoodPoints;
    }

    internal void AddPlayerStateListener(Action<PlayerState> _listener) => this.OnPlayerStateChange += _listener;
    internal void RemovePlayerStateListener(Action<PlayerState> _listener) => this.OnPlayerStateChange -= _listener;
}