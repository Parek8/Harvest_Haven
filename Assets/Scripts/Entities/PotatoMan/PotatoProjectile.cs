using UnityEngine;

internal class PotatoProjectile : MonoBehaviour
{
    [field: SerializeField] internal Transform Target { get; private set; }
    [field: SerializeField] internal float MaxHeight { get; private set; } = 5f;
    [field: SerializeField] internal float Gravity { get; private set; } = -9.81f;
    [field: SerializeField] internal ParticleSystem Particles { get; private set; }
    private float radius;
    private float damage;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Target = GameManager.game_manager.player_transform;

        if (Target != null)
        {
            Vector3 initialVelocity = CalculateLaunchVelocity();

            rb.velocity = initialVelocity;
            transform.rotation = Quaternion.LookRotation(initialVelocity);
        }
    }

    internal void Setup(float Radius, float Damage)
    {
        this.radius = Radius;
        this.damage = Damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Particles.Play();
        Collider[] collisions = Physics.OverlapSphere(transform.position, radius);
        PlayerStats _tmpStats;
        foreach (Collider col in collisions)
        {
            if (col.TryGetComponent(out _tmpStats))
                _tmpStats.Reduce_Health(damage);
        }

        Destroy(gameObject);
    }

    private Vector3 CalculateLaunchVelocity()
    {
        Vector3 displacement = Target.position - transform.position;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);

        float time = Mathf.Sqrt(-2 * MaxHeight / Gravity) + Mathf.Sqrt(2 * (displacement.y - MaxHeight) / Gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Gravity * MaxHeight);
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY * -Mathf.Sign(Gravity);
    }
}