using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : Enemy
    {
        public override void StartAttack()
        {
            base.StartAttack();
            animationController.Attack();
        }

        public void Attack()
        {
            if (AttackTarget == null)
            {
                // NavMeshAgent.enabled = true;
                return;
            }

            AttackTarget.Defense.TakeDamage(AttackForce);
        }
    }
}