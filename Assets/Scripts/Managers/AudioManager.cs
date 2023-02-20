using Defendable;
using Enemies;
using Grid;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        private AIManager AIManager { get; set; }
        private GameGrid Grid { get; set; }

        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip ui;
        [SerializeField] private AudioClip explosion;
        [SerializeField] private AudioClip fire;
        [SerializeField] private AudioClip destroy_enemy;
        [SerializeField] private AudioClip destroy_defense;

        [Inject]
        private void Construct(AIManager aiManager, GameGrid grid)
        {
            AIManager = aiManager;
            Grid = grid;
        }

        private void Start()
        {
            AIManager.OnEnemySpawn += SetEnemySounds;
            AIManager.OnEnemyKilled += ResetEnemySounds;

            Grid.OnDefenseSet += SetDefenseSound;
            Grid.OnResetDefense += ResetDefenseSound;
        }

        public void PlayUI() => Play(ui);

        private void SetDefenseSound(Defense defense)
        {
            defense.OnDeath += () => Play(destroy_defense);
        }

        private void ResetDefenseSound(Defense defense)
        {
            defense.OnDeath -= () => Play(destroy_defense);
        }

        private void SetEnemySounds(Enemy enemy, PoolObjectType type)
        {
            enemy.OnDeath += (enemy) => Play(destroy_enemy);
            if (type == PoolObjectType.KamikazeEnemy)
                ((KamikazeEnemy)enemy).OnExplode += () => Play(explosion);
            if (type == PoolObjectType.FlamerEnemy)
                ((FlamerEnemy)enemy).OnStartAttack += () => Play(fire);
        }

        private void ResetEnemySounds(Enemy enemy, PoolObjectType type)
        {
            enemy.OnDeath -= (enemy) => Play(destroy_enemy);
            if (type == PoolObjectType.KamikazeEnemy)
                ((KamikazeEnemy)enemy).OnExplode -= () => Play(explosion);
            if (type == PoolObjectType.FlamerEnemy)
                ((FlamerEnemy)enemy).OnStartAttack -= () => Play(fire);
        }

        private void Play(AudioClip clip) => source.PlayOneShot(clip);

        private void OnDestroy()
        {
            AIManager.OnEnemySpawn -= SetEnemySounds;
            AIManager.OnEnemyKilled -= ResetEnemySounds;
        }
    }
}