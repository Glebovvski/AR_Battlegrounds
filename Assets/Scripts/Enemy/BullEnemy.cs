using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class BullEnemy : Enemy
    {
        public override void StartAttack(out bool isReady)
        {
            base.StartAttack(out isReady);
            if (isReady)
                animationController.Attack();
            else Debug.LogError("BULL NOT READY");
        }

        public void Attack()
        {
            if (AttackTarget == null)
                return;

            AttackTarget.Defense.TakeDamage(AttackForce);
        }
    }
}