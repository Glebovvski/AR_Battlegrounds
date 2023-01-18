using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : Enemy
    {
        public override void StartAttack(out bool isReady)
        {
            base.StartAttack(out isReady);
            if (isReady)
                animationController.Attack();
                else Debug.LogError("ENEMY NOT READY");
        }

        public void Attack()
        {
            if (AttackTarget == null)
                return;

            AttackTarget.Defense.TakeDamage(AttackForce);
        }
    }
}