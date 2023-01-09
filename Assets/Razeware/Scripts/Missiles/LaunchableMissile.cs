using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Missiles
{
    public class LaunchableMissile : Missile
    {
        public Vector3 Target { get; private set; }
        private Vector3 InitPos { get; set; }
        private Vector3 middle;
        public override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.GetMask("Grid") || other.gameObject.layer == LayerMask.GetMask("Enemy"))
            {
                Explode();
            }
        }

        private void Explode()
        {
            var colliders = Physics.OverlapSphere(this.transform.position, HitRadius, LayerMask.GetMask("Enemy"));
            foreach (var damagable in colliders)
            {
                if (damagable.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(Damage);
                }
                PoolManager.Instance.ReturnToPool(this.gameObject, PoolObjectType.LaunchableMissile);
            }
        }

        public void Launch(Vector3 target)
        {
            middle = InitPos + (target - InitPos) / 2f + new Vector3(0, 10, 0);
            InitPos = this.transform.position;
            Target = target;
            StartCoroutine(LaunchRoutine());
        }

        private IEnumerator LaunchRoutine()
        {
            float t = 0;
            var time = 1;
            while (t < time)
            {
                t += Time.deltaTime*Speed;
                var position = CalculatePosition(t, Target);
                transform.position = Vector3.Slerp(transform.position, position, t/time);

                yield return null;
            }
        }

        private Vector3 CalculatePosition(float t, Vector3 target)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            return uu * this.transform.position + 2 * u * t * middle + tt * target;

        }
    }
}