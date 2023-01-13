using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class HealerEnemy : Enemy
    {
        [SerializeField] private ParticleSystem healFx;
        public void Heal()
        {
            var colliders = Physics.OverlapSphere(Position, AttackRadius, LayerMask.GetMask("Enemy"));
            healFx.Play();
            foreach (var collider in colliders)
            {
                if(collider.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(Mathf.Clamp(-AttackForce, 0, enemy.Health));
                }
            }
        }

        public override void StartAttack()
        {
            Heal();
        }
    }
}