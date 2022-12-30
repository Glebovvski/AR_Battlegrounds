using UnityEngine;

namespace Defendable
{
    public abstract class Defence : MonoBehaviour
    {
        [field: SerializeField] public ScriptableDefence SO { get; set; }
        public bool IsActiveDefence { get; private set; }
        public int AttackRange { get; private set; }
        public int AttackForce { get; private set; }
        public int Health { get; private set; }
        protected int RelaodTime { get; set; }
        protected abstract bool IsReady { get; set; }
        protected Time ReloadStart { get; private set; }

        public bool IsActionAvailable() => IsActiveDefence && IsReady;

        private void GetData()
        {
            IsActiveDefence = SO.IsActiveDefence;
            AttackRange = SO.AttackRange;
            AttackForce = SO.AttackForce;
            Health = SO.Health;
            RelaodTime = SO.RelaodTime;
        }

        private void Start()
        {
            GetData();
        }
    }
}