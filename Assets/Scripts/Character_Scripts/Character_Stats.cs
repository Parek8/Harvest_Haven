using UnityEngine;

internal class Character_Stats : MonoBehaviour
{
    [field: SerializeField] public float movement_speed { get; protected set; } = 15f;
    [field: SerializeField] public float attack_distance { get; protected set; } = 1.2f;
    [field: SerializeField] public float attack_damage { get; protected set; } = 10f;
    [field: SerializeField] public float max_health_points { get; protected set; } = 10f;
    public float current_health_points { get; protected set; } = 10f;

    protected void Start()
    {
        GameManager.game_manager.PlayerManagerInstance.LoadPlayer();
    }

    internal virtual void Reduce_Health(float reduce_hp)
    {
        current_health_points -= reduce_hp;
        if(current_health_points <= 0)
        {
            current_health_points = 0;
            Destroy(gameObject);
        }
    }
}