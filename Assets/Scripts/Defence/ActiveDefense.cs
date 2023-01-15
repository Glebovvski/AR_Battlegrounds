using System.Collections;
using System.Collections.Generic;
using cakeslice;
using Enemies;
using UnityEngine;

namespace Defendable
{
    public class ActiveDefense : Defense
    {
        [SerializeField] protected DetectionRadius Detection;
        [SerializeField] protected List<Outline> outline = new List<Outline>();

        protected override bool IsReady => Time.time - LastAttackTime > ReloadTime;
        protected float LastAttackTime;

        public override void Init(ScriptableDefense so)
        {
            base.Init(so);
            LastAttackTime = Time.time;
            Detection.SetAttackRange(AttackRange);
            ToggleOutline(false);
        }

        public bool IsEnemyInRange(Enemy enemy) => Detection.IsEnemyInRange(enemy);

        public void SetAttackTarget(Enemy enemy) => Detection.SetAttackTarget(enemy);

        public void ToggleOutline(bool value) => outline.ForEach(x => x.enabled = value);
    }
}