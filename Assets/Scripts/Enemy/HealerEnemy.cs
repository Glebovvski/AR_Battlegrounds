using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class HealerEnemy : Enemy
    {
        [SerializeField] private ParticleSystem healFx;
        public void Heal()
        {
            Debug.LogError("HEALER FOLLOW: " + this.FollowTarget.name);
            var colliders = Physics.OverlapSphere(Position, AttackRadius, LayerMask.GetMask("Enemy"));
            HealFX();
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Enemy>(out var enemy))
                {
                    var healValue = Mathf.Clamp(AttackForce, 0, enemy.Health);
                    enemy.TakeDamage(-healValue);
                }
            }
        }

        public override void StartAttack(out bool isReady)
        {
            base.StartAttack(out isReady);
            if (isReady)
                Heal();
        }

        public void HealFX()
        {
            StartCoroutine(PlayHeal());
        }

        IEnumerator PlayHeal()
        {
            healFx.Play();
            for(float t = 0; t < 1;)
            {
                t+=Time.deltaTime;
                yield return null;
            }
            healFx.Stop();
        }
    }
}