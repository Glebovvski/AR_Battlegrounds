using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defendable
{
    public class CastleDefense : Defense
    {
        protected override bool IsReady => true;

        public event Action OnLose;

        protected override void ReturnToPool(GameObject fx)
        {
            DamageReceiver.OnDeath -= OnDeath;
            OnDeath?.Invoke();
        }

        protected override void Death()
        {
            base.Death();
            OnLose?.Invoke();
        }
    }
}