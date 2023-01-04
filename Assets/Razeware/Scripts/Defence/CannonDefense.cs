using System.Collections;
using System.Collections.Generic;
using Enemies;
using Missiles;
using UnityEngine;

namespace Defendable
{
    public class CannonDefense : Defense
    {
        [SerializeField] private DetectionRadius Detection;
        [SerializeField] private Transform tower;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float angleThreshold = 5f;
        [SerializeField] private Transform cannon;
        private float lastShotTime;
        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }

        private void Start()
        {
            base.Start();
            Detection.SetAttackRange(AttackRange);
        }

        private void Update()
        {
            RotateAndAttack(Detection.Enemy);
        }

        private void RotateAndAttack(Enemy enemy)
        {
            if(enemy == null) return;
            
            if (RotateToEnemy(enemy) && IsReady)
                Attack(enemy);
        }

        private void Attack(Enemy enemy)
        {
            var bullet = PoolManager.Instance.GetFromPool<CannonBullet>(PoolObjectType.CannonBullet);
            bullet.transform.position = cannon.position;
            bullet.transform.rotation = tower.rotation;
            bullet.Fire((enemy.transform.position - cannon.transform.position).normalized);
            lastShotTime = Time.time;
        }

        private bool RotateToEnemy(Enemy enemy)
        {
            var direction = (enemy.transform.position - tower.transform.position);
            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), tower.up);
            tower.rotation = Quaternion.RotateTowards(tower.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(targetRotation, tower.rotation) <= angleThreshold) return true;
            return false;
        }
    }
}