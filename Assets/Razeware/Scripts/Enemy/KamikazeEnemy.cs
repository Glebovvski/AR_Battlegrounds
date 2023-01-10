using System;
using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using Defendable;
using UnityEngine;

namespace Enemies
{
    public class KamikazeEnemy : Enemy
    {
        [SerializeField] private CFXR_Effect explosionFX;
        private LayerMask defenseMask;

        private void OnEnable()
        {
            explosionFX.gameObject.SetActive(false);
            explosionFX.OnFinish += ReturnToPool;
        }
        public override void StartAttack()
        {
            Explode();
        }

        private void Explode()
        {
            explosionFX.gameObject.SetActive(true);
            defenseMask = LayerMask.GetMask("Defense");
            var colliders = Physics.OverlapSphere(this.transform.position, AttackRadius, defenseMask);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Defense>(out var defense))
                {
                    defense.TakeDamage(AttackForce);
                }
            }
        }

        private void ReturnToPool(GameObject go)
        {
            PoolManager.Instance.ReturnToPool(this.gameObject, PoolObjectType.KamikazeEnemy);
        }

        private void OnDisable()
        {
            explosionFX.gameObject.SetActive(false);
            explosionFX.OnFinish -= ReturnToPool;
        }
    }
}