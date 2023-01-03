using System;
using System.Linq;
using UnityEngine;

namespace Defendable
{
    public abstract class Defence : MonoBehaviour
    {
        [field: SerializeField] public ScriptableDefence SO { get; set; }
        // [field: SerializeField] public GameGrid grid { get; set; }

        public int Health { get; private set; }
        public bool IsActiveDefence { get; private set; }
        public int AttackRange { get; private set; }
        public int AttackForce { get; private set; }
        public Vector2Int Size { get; private set; }
        protected int RelaodTime { get; set; }
        protected float ProjectileSpeed { get; set; }
        protected abstract bool IsReady { get; set; }
        protected Time ReloadStart { get; private set; }
        public Predicate<GridCell> ConditionToPlace => SO.GetCondition();
        public Predicate<GridCell[]> CanFit = (GridCell[] cells) => cells.All(x => x.IsUpper) || cells.All(x => !x.IsUpper);
        public bool IsActionAvailable() => IsActiveDefence && IsReady;
        public Vector2Int GetSize() => SO.Size;
        public DefenseType Type => SO.Type;

        private void GetData()
        {
            IsActiveDefence = SO.IsActiveDefence;
            AttackRange = SO.AttackRange;
            AttackForce = SO.AttackForce;
            Health = SO.Health;
            RelaodTime = SO.RelaodTime;
            ProjectileSpeed = SO.ProjectileSpeed;
            Size = SO.Size;
        }

        protected void Start()
        {
            GetData();
        }
    }
}