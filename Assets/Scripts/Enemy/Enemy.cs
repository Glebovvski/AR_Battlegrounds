using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.AI.Components;
using Defendable;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using CartoonFX;
using cakeslice;
using Zenject;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IContextProvider, IEnemy
    {
        private InputManager InputManager { get; set; }

        [SerializeField] private ScriptableEnemy SO;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Animator animator;
        [SerializeField] private CFXR_Effect fx;
        [SerializeField] private Outline outline;

        public int Health { get; private set; }
        public float CurrentHealth => DamageReceiver.CurrentHealth;
        public int AttackForce { get; set; }
        public int Speed { get; set; }
        public int GoldToDrop => SO.GoldToDrop;
        public bool IsAlive => !DamageReceiver.IsDead;
        public AIContext Context { get; private set; }
        public GameObject GameObject => this.gameObject;
        public NavMeshAgent NavMeshAgent => navMeshAgent;
        protected Observation AttackTarget { get; set; }
        public Enemy FollowTarget { get; set; }
        public float ScanRange => SO.ScanRange;
        public Vector3 Position => this.gameObject.transform.position;
        public DamageReceiver DamageReceiver;
        public PoolObjectType Type => SO.Type;
        public float AttackRange => SO.AttackRange;
        public float AttackRadius => SO.AttackRadius;
        public EnemyType EnemyType => SO.EnemyType;
        protected bool IsReadyToAttack
        {
            get
            {
                var timedif = Time.time - LastAttackTime;
                return timedif > SO.TimeBetweenAttacks;
            }
        }
        public int AttackWallScore => SO.AttackWallScore;
        public int AttackCannonScore => SO.AttackCannonScore;
        public int AttackLaserScore => SO.AttackLaserScore;
        public int AttackCastleScore => SO.AttackCastleScore;
        public int AttackTrapScore => SO.AttackTrapScore;

        protected float LastAttackTime { get; set; }

        public event Action<Enemy> OnDeath;

        public Dictionary<DefenseType, int> DefenseTypeToScore = new Dictionary<DefenseType, int>();

        public IAIContext GetContext(Guid aiId) => Context;

        protected AnimationController animationController;

        [Inject]
        private void Construct(InputManager inputManager)
        {
            InputManager = inputManager;
        }

        private void Start()
        {
            outline.enabled = false;
            InputManager.OnActiveDefenseClick += OutlineEnemy;
            InputManager.OnEnemyClick += CancelOutline;
            PopulateDictionary();
        }

        protected virtual void Init()
        {
            GetData();
            DamageReceiver = new DamageReceiver(Health);
            Context = new AIContext(this);
            animationController = new AnimationController(animator, navMeshAgent, DamageReceiver);
            DamageReceiver.OnDeath += Death;
            NavMeshAgent.enabled = true;
            fx.OnFinish += RemoveEnemyFromField;
            fx.gameObject.SetActive(false);
        }


        private void PopulateDictionary()
        {
            DefenseTypeToScore.Add(DefenseType.Castle, AttackCastleScore);
            DefenseTypeToScore.Add(DefenseType.Cannon, AttackCannonScore);
            DefenseTypeToScore.Add(DefenseType.Laser, AttackLaserScore);
            DefenseTypeToScore.Add(DefenseType.Wall, AttackWallScore);
            DefenseTypeToScore.Add(DefenseType.Trap, AttackTrapScore);

            DefenseTypeToScore = DefenseTypeToScore.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public virtual void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            if (AttackTarget != null)
                Vector3.RotateTowards(this.transform.rotation.eulerAngles, new Vector3(AttackTarget.Defense.transform.rotation.x, 0, AttackTarget.Defense.transform.rotation.z), 5, 5);
            animationController.UpdateState();
        }

        public void SetAttackTarget(Observation observation)
        {
            AttackTarget = observation;
            NavMeshAgent.stoppingDistance = AttackTarget.Size.x;
            AttackTarget.Defense.OnDeath += ResetTarget;
            observation.SetAsAttackTarget();
        }

        public Observation GetAttackTarget() => AttackTarget;

        public void GetData()
        {
            Health = SO.Health;
            AttackForce = SO.AttackForce;
            Speed = SO.Speed;
            navMeshAgent.speed = SO.Speed;
        }

        public void MoveTo(Vector3 destination)
        {
            NavMeshAgent.destination = destination;
        }

        public void TakeDamage(float value) => DamageReceiver.TakeDamage(value);

        public NavMeshPath GetCalculatedPath(Observation observation)
        {
            var navMeshPath = new NavMeshPath();
            NavMeshAgent.CalculatePath(observation.Position, navMeshPath);
            return navMeshPath;
        }

        public virtual void StartAttack(out bool isReady)
        {
            isReady = IsReadyToAttack;
            if (isReady)
                LastAttackTime = Time.time;
        }

        private void OnTriggerEnter(Collider other)
        {
            DamageReceiver.OnCollision(other);
        }

        private void Death()
        {
            DamageReceiver.OnDeath -= Death;
            NavMeshAgent.enabled = false;
            fx.gameObject.SetActive(true);
        }

        private void RemoveEnemyFromField(GameObject effect)
        {
            OnDeath?.Invoke(this);
            fx.OnFinish -= RemoveEnemyFromField;
            AIManager.Instance.UnregisterEnemy(this);
        }

        private void ResetTarget()
        {
            AIManager.Instance.RemoveObservation(AttackTarget);
            AttackTarget.Defense.OnDeath -= ResetTarget;
            AttackTarget = null;
        }

        private void OutlineEnemy() => outline.enabled = true;
        private void CancelOutline() => outline.enabled = false;

        // private void OnDestroy()
        // {
        //     InputManager.OnActiveDefenseClick -= OutlineEnemy;
        //     InputManager.OnEnemyClick -= CancelOutline;
        // }
    }

    public enum EnemyType
    {
        Mono = 0, //for big enemies or spies
        Team = 1, //for small mobs
        Kamikaze = 2,
        Healer = 3,
        Any = 4
    }
}