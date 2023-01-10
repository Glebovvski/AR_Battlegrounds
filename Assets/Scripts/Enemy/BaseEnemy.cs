using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : Enemy
    {
        public override void StartAttack()
        {
            animationController.Attack();
        }

        public void Attack()
        {
            if (AttackTarget == null)
                return;

            AttackTarget.Defense.TakeDamage(AttackForce);
        }
    }
}