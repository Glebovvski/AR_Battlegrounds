using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;

namespace Enemies
{
    public class KamikazeEnemy : Enemy
    {
        private LayerMask defenseMask;
        public override void StartAttack()
        {
            Explode();
        }

        private void Explode()
        {
            defenseMask = LayerMask.GetMask("Defense");
            var colliders = Physics.OverlapSphere(this.transform.position, AttackRange, defenseMask);
            foreach (var collider in colliders)
            {
                if(collider.TryGetComponent<Defense>(out var defense))
                {
                    defense.TakeDamage(AttackForce);
                }
            }
        }
    }
}