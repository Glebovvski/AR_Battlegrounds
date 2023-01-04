using System;
using System.Linq;
using UnityEngine;

namespace Defendable
{
    public abstract class Defense : MonoBehaviour
    {
        [field: SerializeField] public ScriptableDefence SO { get; set; }

        public int Health { get; private set; }
        public float CurrentHealth => DamageReceiver.CurrentHealth;
        public bool IsActiveDefence { get; private set; }
        public int AttackRange { get; private set; }
        public int AttackForce { get; private set; }
        public Vector2Int Size { get; private set; }
        protected int RelaodTime { get; set; }
        protected abstract bool IsReady { get; set; }
        protected Time ReloadStart { get; private set; }
        public Predicate<GridCell> ConditionToPlace => SO.GetCondition();
        public Predicate<GridCell[]> CanFit = (GridCell[] cells) => cells.All(x => x.IsUpper) || cells.All(x => !x.IsUpper);
        public bool IsActionAvailable() => IsActiveDefence && IsReady;
        public Vector2Int GetSize() => SO.Size;
        public DefenseType Type => SO.Type;
        protected DamageReceiver DamageReceiver;

        private void GetData()
        {
            IsActiveDefence = SO.IsActiveDefence;
            AttackRange = SO.AttackRange;
            AttackForce = SO.AttackForce;
            Health = SO.Health;
            RelaodTime = SO.RelaodTime;
            Size = SO.Size;
        }

        protected void Start()
        {
            GetData();
            DamageReceiver = new DamageReceiver(Health);
        }
    }
}