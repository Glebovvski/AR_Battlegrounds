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
        [SerializeField] private GameObject goblinMesh;
        private LayerMask defenseMask;

        public override void OnEnable()
        {
            base.OnEnable();
            goblinMesh.SetActive(true);
            explosionFX.gameObject.SetActive(false);
            explosionFX.OnFinish += ReturnToPool;
        }
        public override void StartAttack()
        {
            Explode();
        }

        private void Explode()
        {
            goblinMesh.SetActive(false);
            explosionFX.transform.localScale = new Vector3(AttackRadius, AttackRadius, AttackRadius);
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
            goblinMesh.SetActive(true);
            explosionFX.gameObject.SetActive(false);
            explosionFX.OnFinish -= ReturnToPool;
        }
    }
}