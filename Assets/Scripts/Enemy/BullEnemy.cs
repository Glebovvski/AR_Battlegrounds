using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class BullEnemy : Enemy
    {
        public override void StartAttack()
        {
            base.StartAttack();
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