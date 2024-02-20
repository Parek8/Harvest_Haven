using Unity.VisualScripting;
using UnityEngine;

internal class PotatoManBehaviour : EnemyBehaviour
{
    [field: SerializeField] PotatoProjectile Bullet;
    [field: SerializeField] Transform BulletSpawnPoint;
    [field: SerializeField] float DamageRadius;
    protected override void Attack()
    {
        if (_cooldown <= 0)
        {
            Instantiate(Bullet, BulletSpawnPoint.position, Quaternion.identity).Setup(DamageRadius, _stats.attack_damage);
            _cooldown = AttackCooldown;
        }
    }
}