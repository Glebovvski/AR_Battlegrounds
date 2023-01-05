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

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IContextProvider, IEnemy
    {
        [SerializeField] private ScriptableEnemy SO;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Animator animator;

        public int Health { get; private set; }
        public int AttackForce { get; set; }
        public int Speed { get; set; }
        public bool IsAlive => !DamageReceiver.IsDead;
        public AIContext Context { get; private set; }
        public GameObject GameObject => this.gameObject;
        public NavMeshAgent NavMeshAgent => navMeshAgent;
        public Vector3? MoveTarget { get; set; }
        public Observation AttackTarget { get; set; }
        public float ScanRange => SO.ScanRange;
        public Vector3 Position => this.gameObject.transform.position;
        public DamageReceiver DamageReceiver;
        public PoolObjectType Type => SO.Type;
        public float AttackRange => SO.AttackRange;
        public EnemyType EnemyType => SO.EnemyType;

        public int AttackWallScore => SO.AttackWallScore;
        public int AttackCannonScore => SO.AttackCannonScore;
        public int AttackLaserScore => SO.AttackLaserScore;
        public int AttackCastleScore => SO.AttackCastleScore;

        public event Action<Enemy> OnDeath;

        public Dictionary<DefenseType, int> DefenseTypeToScore = new Dictionary<DefenseType, int>();

        public IAIContext GetContext(Guid aiId) => Context;

        private AnimationController animationController;

        private void Awake()
        {
            PopulateDictionary();
        }

        private void Init()
        {
            Context = new AIContext(this);
            GetData();
            DamageReceiver = new DamageReceiver(Health);
            animationController = new AnimationController(animator, navMeshAgent, DamageReceiver);
            DamageReceiver.OnDeath += Death;
        }

        private void PopulateDictionary()
        {
            DefenseTypeToScore.Add(DefenseType.Castle, AttackCastleScore);
            DefenseTypeToScore.Add(DefenseType.Cannon, AttackCannonScore);
            DefenseTypeToScore.Add(DefenseType.Laser, AttackLaserScore);
            DefenseTypeToScore.Add(DefenseType.Wall, AttackWallScore);

            DefenseTypeToScore = DefenseTypeToScore.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            animationController.UpdateState();
        }

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

        public abstract void Attack();

        private void OnTriggerEnter(Collider other)
        {
            DamageReceiver.OnCollision(other);
        }

        private void Death()
        {
            DamageReceiver.OnDeath -= Death;
            StartCoroutine(WaitForDeathAnimationToFinish());
        }

        IEnumerator WaitForDeathAnimationToFinish()
        {
            yield return new WaitForSeconds(1);
            AIManager.Instance.UnregisterEnemy(this);
            OnDeath?.Invoke(this);
        }
    }

    public enum EnemyType
    {
        Mono = 0, //for big enemies or spies
        Team = 1, //for small mobs
        Any = 2
    }
}