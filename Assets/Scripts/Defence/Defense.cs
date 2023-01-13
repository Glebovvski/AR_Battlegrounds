using System;
using System.Collections;
using System.Linq;
using CartoonFX;
using UnityEngine;

namespace Defendable
{
    public abstract class Defense : MonoBehaviour
    {
        [SerializeField] private CFXR_Effect destroyFX;
        [SerializeField] private GameObject defenseMesh;
        [SerializeField] private HealthBarController healthBarController;

        [field: SerializeField] public ScriptableDefense SO { get; set; }

        public int Health { get; private set; }
        public float CurrentHealth => DamageReceiver.CurrentHealth;
        public bool IsActiveDefense { get; private set; }
        protected int AttackRange { get; private set; }
        protected int AttackForce { get; private set; }
        public Vector2Int Size { get; private set; }
        protected int ReloadTime { get; set; }
        protected abstract bool IsReady { get; set; }
        protected Time ReloadStart { get; private set; }
        public Predicate<GridCell> ConditionToPlace => SO.GetCondition();
        public Predicate<GridCell[]> CanFit = (GridCell[] cells) => cells.All(x => x.IsUpper) || cells.All(x => !x.IsUpper);
        public bool IsActionAvailable() => IsActiveDefense && IsReady;
        public Vector2Int GetSize() => SO.Size;
        public DefenseType Type => SO.Type;
        public BoxCollider Collider { get; private set; }
        protected DamageReceiver DamageReceiver;
        public event Action OnDefenseSet;

        public event Action OnDeath;

        public virtual void Init(ScriptableDefense so)
        {
            SO = so;
            IsActiveDefense = SO.IsActiveDefense;
            AttackRange = SO.AttackRange;
            AttackForce = SO.AttackForce;
            Health = SO.Health;
            ReloadTime = SO.ReloadTime;
            Size = SO.Size;
            DamageReceiver = new DamageReceiver(Health);
            DamageReceiver.OnDeath += Death;
            DamageReceiver.OnTakeDamage += UpdateHealthBar;
            destroyFX.OnFinish += ReturnToPool;
        }

        private void UpdateHealthBar()
        {
            healthBarController.UpdateHealth(CurrentHealth / Health);
        }

        public virtual void OnEnable()
        {
            destroyFX.gameObject.SetActive(false);
            defenseMesh.SetActive(true);
        }

        protected void Awake()
        {
            Collider = GetComponent<BoxCollider>();
        }
        public void TakeDamage(float value) => DamageReceiver.TakeDamage(value);

        protected virtual void Death()
        {
            defenseMesh.SetActive(false);
            destroyFX.gameObject.SetActive(true);
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

        private void ReturnToPool(GameObject fx)
        {
            DamageReceiver.OnDeath -= OnDeath;
            PoolManager.Instance.ReturnToPool(this.gameObject, DefenseTypeToPoolType(Type));
            OnDeath?.Invoke();
        }

        public void DefenseSet() => OnDefenseSet?.Invoke();
    }
}