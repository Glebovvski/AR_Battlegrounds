using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class HealerEnemy : Enemy
    {
        public void Heal()
        {
            var colliders = Physics.OverlapSphere(Position, AttackRadius, LayerMask.GetMask("Enemy"));

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
        }
    }
}