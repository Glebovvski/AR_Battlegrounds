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
        public override event Action OnDeath;

        public override void Init(ScriptableDefense so)
        {
            base.Init(so);
            UpdateHealthBar();
        }

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

        private void Update()
        {
            Debug.LogError("HEALTH: " + CurrentHealth);
        }
    }
}