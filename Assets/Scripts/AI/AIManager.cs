using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Defendable;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager Instance;

        private CurrencyModel CurrencyModel { get; set; }
        private CastleDefense Castle { get; set; }
        public GameGrid Grid { get; private set; }
        private LoseModel LoseModel { get; set; }
        private StatManager StatManager { get; set; }

        [SerializeField] private PlaneManager planeManager;

        [Header("Battle settings")]
        [Range(2, 25)]
        [SerializeField] private int maxWaves;
        [Range(1, 50)]
        [SerializeField] private int maxBaseEnemies;
        [Range(1, 5)]
        [SerializeField] private int maxBullEnemies;
        [Range(1, 5)]
        [SerializeField] private int maxHealerEnemies;
        [Range(1, 5)]
        [SerializeField] private int maxSpyEnemies;
        [Range(1, 10)]
        [SerializeField] private int maxKamikazeEnemies;
        [Range(1, 5)]
        [SerializeField] private int maxFlamerEnemies;

        [SerializeField] private Transform[] spawnPoints;


        public List<Observation> Observations = new List<Observation>();
        [SerializeField] private List<Enemy> Enemies = new List<Enemy>();

        private Dictionary<PoolObjectType, int> enemyCoefs = new Dictionary<PoolObjectType, int>();

        [Inject]
        private void Construct(CurrencyModel currencyModel, GameGrid grid, CastleDefense castle, LoseModel loseModel, StatManager statManager)
        {
            CurrencyModel = currencyModel;
            Grid = grid;
            Castle = castle;
            LoseModel = loseModel;
            StatManager = statManager;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Castle.OnLose += ReturnAllEnemiesToPool;
            LoseModel.OnRestart += SpawnEnemies;
            planeManager.OnGridSet += SpawnEnemies;

            enemyCoefs.Add(PoolObjectType.Enemy, maxBaseEnemies);
            enemyCoefs.Add(PoolObjectType.SpyEnemy, maxSpyEnemies);
            enemyCoefs.Add(PoolObjectType.FlamerEnemy, maxFlamerEnemies);
            enemyCoefs.Add(PoolObjectType.BullEnemy, maxBullEnemies);
            enemyCoefs.Add(PoolObjectType.HealerEnemy, maxHealerEnemies);
            enemyCoefs.Add(PoolObjectType.KamikazeEnemy, maxKamikazeEnemies);
        }

        private void SpawnEnemies()
        {
            int maxEnemies = enemyCoefs.Select(x => x.Value).Sum();
            if (maxEnemies - Enemies.Count > maxBaseEnemies / 2)
                return;

            foreach (var enemy in enemyCoefs)
            {
                var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];
                var enemiesOnMap = Enemies.Where(x => x.Type == enemy.Key).Count();
                if (enemiesOnMap < enemy.Value)
                {
                    int enemiesToSpawn = UnityEngine.Random.Range(0, enemy.Value);
                    for (int i = 0; i < enemiesToSpawn; i++)
                        RegisterEnemy(enemy.Key, spawnPoint);
                }
            }

            if (Enemies.Where(x => x.Type != PoolObjectType.SpyEnemy || x.Type != PoolObjectType.HealerEnemy).Count() == 0)
                SpawnEnemies();
        }

        public void AddObservation(Observation observation)
        {
            if (Observations.Any(x => x.Equals(observation))) return;

            Observations.Add(observation);
        }

        public void AddObservation(List<Observation> observations)
        {
            foreach (var observation in observations)
            {
                AddObservation(observation);
            }
        }

        public void RemoveObservation(Observation observation)
        {
            if (Observations.Any(x => x.Equals(observation)))
                Observations.Remove(observation);
        }

        public Observation GetClosest(IEnemy enemy) => GetClosest(enemy, Observations);

        public Enemy GetClosestEnemyByType(IEnemy enemy, EnemyType type) => Enemies.Where(x => (x.Position - enemy.Position).sqrMagnitude < enemy.ScanRange * enemy.ScanRange && x.EnemyType == type).FirstOrDefault();


        public Observation GetClosestObservationByType(IEnemy enemy, DefenseType type)
        {
            var observations = Observations.Where(x => x.Defense.Type == type).ToList();
            if (observations.Count == 0)
                return null;

            return GetClosest(enemy, observations);
        }

        public List<Observation> GetObservationsOfType(DefenseType type) => Observations.Where(x => x.Defense.Type == type).ToList();

        public List<Observation> GetActiveDefenses(DefenseType except = DefenseType.None) => Observations.Where(x => x.Defense.IsActiveDefense && x.Defense.Type != except).ToList();

        public Enemy RegisterEnemy(PoolObjectType enemyType, Transform parent)
        {
            var enemy = PoolManager.Instance.GetFromPool<Enemy>(enemyType);
            enemy.gameObject.SetActive(false);
            planeManager.AttachChild(enemy.transform);
            enemy.Init();
            enemy.transform.position = parent.position;
            enemy.OnDeath += GetGoldFromEnemy;
            Enemies.Add(enemy);
            enemy.gameObject.SetActive(true);
            return enemy;
        }

        public void UnregisterEnemy(Enemy enemy)
        {
            Enemies.Remove(enemy);
            PoolManager.Instance.ReturnToPool(enemy.GameObject, enemy.Type);
            StatManager.AddEnemiesKilled();
            SpawnEnemies();
        }

        public IEnumerable<Enemy> GetEnemiesAttackingObservation(Observation observation) => Enemies.Where(x => x.AttackTarget == observation);

        public IEnumerable<Enemy> GetClosestEnemiesWithSameTarget(Enemy enemy) => Enemies.Where(x => (x.Position - enemy.Position).sqrMagnitude < enemy.ScanRange * enemy.ScanRange && x.AttackTarget == enemy.AttackTarget);

        private void GetGoldFromEnemy(Enemy enemy)
        {
            CurrencyModel.AddGold(enemy.GoldToDrop);
        }

        public List<Enemy> GetEnemiesInRangeWithHealthLowerThan(Enemy enemy, float percent) => Enemies.Where(x => (x.Position - enemy.Position).sqrMagnitude < enemy.ScanRange * enemy.ScanRange
        && (x.CurrentHealth / x.Health) <= percent && x.CurrentHealth > 0)
        .ToList();

        public Enemy GetClosest(Enemy enemy, List<Enemy> selectedEnemies) => selectedEnemies.OrderBy(x => (x.Position - enemy.Position).sqrMagnitude).FirstOrDefault();

        private Observation GetClosest(IEnemy enemy, List<Observation> observations)
        {
            Vector3 position = enemy.GameObject.transform.position;
            float distance = 1000;
            Observation closest = null;
            for (int i = 0; i < observations.Count; i++)
            {
                var currDistance = (observations[i].Position - position).sqrMagnitude;
                if (currDistance < distance)
                {
                    distance = currDistance;
                    closest = observations[i];
                }
            }
            return closest;
        }

        private void ReturnAllEnemiesToPool()
        {
            foreach (var enemy in Enemies)
            {
                PoolManager.Instance.ReturnToPool(enemy.GameObject, enemy.Type);
            }
        }

        private void OnDestroy()
        {
            Castle.OnLose -= ReturnAllEnemiesToPool;
            LoseModel.OnRestart -= SpawnEnemies;
            planeManager.OnGridSet -= SpawnEnemies;
        }
    }
}