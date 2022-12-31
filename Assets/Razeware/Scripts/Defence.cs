using System;
using System.Linq;
using UnityEngine;

namespace Defendable
{
    public abstract class Defence : MonoBehaviour
    {
        [field: SerializeField] public ScriptableDefence SO { get; set; }
        [field: SerializeField] public GameGrid grid { get; set; }
        public bool IsActiveDefence { get; private set; }
        public int AttackRange { get; private set; }
        public int AttackForce { get; private set; }
        public int Health { get; private set; }
        public Vector2 Size { get; private set; }
        protected int RelaodTime { get; set; }
        protected abstract bool IsReady { get; set; }
        protected Time ReloadStart { get; private set; }
        public Predicate<GridCell> ConditionToPlace;
        public Predicate<GridCell> IsEmptyCell = (GridCell cell) => cell.IsFree; 
        public bool IsActionAvailable() => IsActiveDefence && IsReady;

        public void CheckCondition()
        {
            var match = grid.GridList.FindAll(ConditionToPlace).FindAll(IsEmptyCell);
            match.ForEach(x => x.SetSelected());
        }

        private void GetData()
        {
            IsActiveDefence = SO.IsActiveDefence;
            AttackRange = SO.AttackRange;
            AttackForce = SO.AttackForce;
            Health = SO.Health;
            RelaodTime = SO.RelaodTime;
            Size = SO.Size;
            ConditionToPlace = SO.GetCondition(SO.Condition);
        }

        private void Start()
        {
            GetData();
        }
    }
}