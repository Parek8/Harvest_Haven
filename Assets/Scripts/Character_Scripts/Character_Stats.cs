using UnityEngine;

internal class Character_Stats : MonoBehaviour
{
    [field: SerializeField] public float MovementSpeed { get; protected set; } = 15f;
    [field: SerializeField] public float MaxHealthPoints { get; protected set; } = 10f;

    [field: Header("Attack")]
    [field: SerializeField] public float AttackDistance { get; protected set; } = 1.2f;
    [field: SerializeField] public float AttackDamage { get; protected set; } = 10f;
    [field: SerializeField] public float AttackDelay { get; protected set; } = 1f;
    public float CurrentHealthPoints { get; protected set; } = 10f;

    protected void Start()
    {
    }

    internal virtual void Reduce_Health(float reduce_hp)
    {
        CurrentHealthPoints -= reduce_hp;
        if(CurrentHealthPoints <= 0)
        {
            CurrentHealthPoints = 0;
            Destroy(gameObject);
        }
    }
}