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
        public override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.GetMask("Default") || other.gameObject.layer == LayerMask.GetMask("Enemy"))
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
            }
        }

        public void Launch(Vector3 target)
        {
            InitPos = this.transform.position;
            Target = target;
            StartCoroutine(LaunchRoutine());
        }

        private IEnumerator LaunchRoutine()
        {
            float t = 0;
            var time = Vector3.Distance(this.transform.position, Target) / Speed;
            while (t < time)
            {
                float x = Speed * t * Mathf.Cos(60);
                float y = Speed * t * Mathf.Sin(60) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
                transform.position = new Vector3(x, y, 0);

                t += Time.deltaTime;
                yield return null;
            }
        }

        private Vector3 CalculatePosition(float t, Vector3 target)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 middle = (target - InitPos) / 2 + new Vector3(0, 10, 0);

            return uu * this.transform.position + 2 * u * t * middle + tt * target;

        }
    }
}