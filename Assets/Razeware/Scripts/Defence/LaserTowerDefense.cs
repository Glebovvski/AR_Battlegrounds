using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Defendable
{
    public class LaserTowerDefense : Defence
    {
        [SerializeField] private DetectionRadius Detection;
        [SerializeField] private Transform laserStartPos;

        private float lastShotTime;
        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }

        [SerializeField] LineRenderer laser;
        private bool isAttacking = false;

        private void Start()
        {
            base.Start();
            lastShotTime = Time.time;
            Detection.SetAttackRange(AttackRange);
        }

        private void Update()
        {
            if (!IsReady || Detection.Enemies.Count == 0) return;

            if (!isAttacking)
                Attack();
        }

        public void Attack()
        {
            isAttacking = true;
            var enemy = Detection.Enemies[0];
            StartCoroutine(LaserShot(enemy));
        }

        IEnumerator LaserShot(Enemy enemy)
        {
            float t = 0;
            float time = 5;
            Vector3 endLaserPos = laser.transform.position;
            laser.SetPosition(0, laserStartPos.position);
            for (; t < time; t += Time.deltaTime)
            {
                endLaserPos = Vector3.Lerp(laserStartPos.position, enemy.transform.position, t/time);
                laser.SetPosition(1, endLaserPos);
                yield return null;
            }

            while (enemy.IsAlive)// && Detection.IsEnemyInRange(enemy))
            {
                Debug.LogError("LASER SHOOT");
                UpdateLaser(enemy);
                yield return new WaitForFixedUpdate();
            }
            ResetLaser();
        }

        private void ResetLaser()
        {
            lastShotTime = Time.time;
            isAttacking = false;
            laser.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        }

        private void UpdateLaser(Enemy enemy)
        {
            laser.SetPosition(0, laserStartPos.position);
            laser.SetPosition(1, enemy.transform.position);
        }
    }
}