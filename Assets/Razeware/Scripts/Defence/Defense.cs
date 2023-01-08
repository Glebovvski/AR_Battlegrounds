using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Defendable
{
    public abstract class Defense : MonoBehaviour
    {
        [field: SerializeField] public ScriptableDefence SO { get; set; }

        public int Health { get; private set; }
        public float CurrentHealth => DamageReceiver.CurrentHealth;
        protected bool IsActiveDefence { get; private set; }
        protected int AttackRange { get; private set; }
        protected int AttackForce { get; private set; }
        public Vector2Int Size { get; private set; }
        protected int RelaodTime { get; set; }
        protected abstract bool IsReady { get; set; }
        protected Time ReloadStart { get; private set; }
        public Predicate<GridCell> ConditionToPlace => SO.GetCondition();
        public Predicate<GridCell[]> CanFit = (GridCell[] cells) => cells.All(x => x.IsUpper) || cells.All(x => !x.IsUpper);
        public bool IsActionAvailable() => IsActiveDefence && IsReady;
        public Vector2Int GetSize() => SO.Size;
        public DefenseType Type => SO.Type;
        public BoxCollider Collider { get; private set; }
        protected DamageReceiver DamageReceiver;
        public event Action OnDefenseSet;

        public event Action OnDeath;

        private void GetData()
        {
            IsActiveDefence = SO.IsActiveDefence;
            AttackRange = SO.AttackRange;
            AttackForce = SO.AttackForce;
            Health = SO.Health;
            RelaodTime = SO.RelaodTime;
            Size = SO.Size;
        }

        private void OnEnable()
        {
            DamageReceiver = new DamageReceiver(Health);
            DamageReceiver.OnDeath += Death;
        }

        protected void Awake()
        {
            GetData();
            Collider = GetComponent<BoxCollider>();
        }
        public void TakeDamage(float value) => DamageReceiver.TakeDamage(value);

        private void Death()
        {
            StartCoroutine(WaitForDeathAnimationFinish());
        }

        private PoolObjectType DefenseTypeToPoolType(DefenseType type)
        {
            switch (type)
            {
                case DefenseType.Castle:
                    return PoolObjectType.CastleTower;
                case DefenseType.Wall:
                    return PoolObjectType.WallTower;
                case DefenseType.Cannon:
                    return PoolObjectType.CannonTower;
                case DefenseType.Laser:
                    return PoolObjectType.LaserTower;
                case DefenseType.Trap:
                    return PoolObjectType.TrapTower;
                default:
                    return PoolObjectType.None;
            }
        }

        IEnumerator WaitForDeathAnimationFinish()
        {
            yield return new WaitForSeconds(1);
            OnDeath?.Invoke();
            DamageReceiver.OnDeath -= OnDeath;
            PoolManager.Instance.ReturnToPool(this.gameObject, DefenseTypeToPoolType(Type));
        }

        public void DefenseSet() => OnDefenseSet?.Invoke();
    }
}