using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Defendable
{
    public class ActiveDefense : Defense
    {
        [SerializeField] protected DetectionRadius Detection;

        protected override bool IsReady => Time.time - LastAttackTime > ReloadTime;
        protected float LastAttackTime;

        public override void Init(ScriptableDefense so)
        {
            base.Init(so);
            LastAttackTime = Time.time;
            Detection.SetAttackRange(AttackRange);
        }

        // public bool IsEnemyInRange(Enemy enemy) => 
    }
}