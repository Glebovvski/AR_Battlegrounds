using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Missiles;
using UnityEngine;

namespace Defendable
{
    public class LaserTowerDefense : Defense
    {
        [SerializeField] private DetectionRadius Detection;
        [SerializeField] private Transform laserStartPos;
        [SerializeField] private Laser laser;

        private float lastShotTime;
        protected override bool IsReady { get => Time.time - lastShotTime > ReloadTime; set => IsReady = value; }

        [SerializeField] LineRenderer laserRenderer;
        private bool isAttacking = false;

        public override void Init(ScriptableDefense so)
        {
            base.Init(so);
            lastShotTime = Time.time;
            Detection.SetAttackRange(AttackRange);
        }

        private void Update()
        {
            if (!IsReady || Detection.Enemy == null)
            {
                ResetLaser();
                return;
            }

            if (!isAttacking)
                Attack();
        }

        public void Attack()
        {
            isAttacking = true;
            var enemy = Detection.Enemy;
            StartCoroutine(LaserShot(enemy));
        }

        IEnumerator LaserShot(Enemy enemy)
        {
            float t = 0;
            float time = 5;
            Vector3 endLaserPos = laser.transform.position;
            laserRenderer.SetPosition(0, laserStartPos.position);

            for (; t < time; t += Time.deltaTime)
            {
                endLaserPos = Vector3.Lerp(laserStartPos.position, enemy.transform.position, t / time);
                laserRenderer.SetPosition(1, endLaserPos);
                yield return null;
            }

            while (enemy.IsAlive && Detection.IsEnemyInRange(enemy))
            {
                Debug.LogError("LASER SHOOT");
                UpdateLaser(enemy);
                yield return new WaitForFixedUpdate();
            }
            ResetLaser();
        }

        private void ResetLaser()
        {
            if (IsReady) lastShotTime = Time.time;
            isAttacking = false;
            laserRenderer.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        }

        private void UpdateLaser(Enemy enemy)
        {
            laserRenderer.SetPosition(0, laserStartPos.position);
            laserRenderer.SetPosition(1, enemy.transform.position);
            enemy.TakeDamage(laser.Damage / Time.deltaTime);
        }
    }
}