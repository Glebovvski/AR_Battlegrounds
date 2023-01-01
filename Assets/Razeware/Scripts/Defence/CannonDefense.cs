using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Defendable
{
    public class CannonDefense : Defence
    {
        [SerializeField] private DetectionRadius Detection;
        [SerializeField] private Transform tower;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float angleThreshold = 5f;
        private float lastShotTime;
        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }

        private void Start()
        {
            base.Start();
            Detection.SetAttackRange(AttackRange);
        }

        private void Update()
        {
            foreach (var enemy in Detection.Enemies)
            {
                RotateAndAttack(enemy);
            }
        }

        private void RotateAndAttack(Enemy enemy)
        {
            if (RotateToEnemy(enemy) && IsReady)
                Attack();
        }

        private void Attack()
        {
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