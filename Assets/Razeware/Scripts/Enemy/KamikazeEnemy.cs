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
        private LayerMask defenseMask;
        public override void StartAttack()
        {
            Explode();
        }

        private void Explode()
        {
            var explosionFX = PoolManager.Instance.GetFromPool<CFXR_Effect>(PoolObjectType.KamikazeExplosionFX);
            explosionFX.OnFinish += (GameObject go) => PoolManager.Instance.ReturnToPool(go, PoolObjectType.KamikazeExplosionFX);
            defenseMask = LayerMask.GetMask("Defense");
            var colliders = Physics.OverlapSphere(this.transform.position, AttackRadius, defenseMask);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Defense>(out var defense))
                {
                    defense.TakeDamage(AttackForce);
                }
            }
            PoolManager.Instance.ReturnToPool(this.gameObject, PoolObjectType.KamikazeEnemy);
        }
    }
}