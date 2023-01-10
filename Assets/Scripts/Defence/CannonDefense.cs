using System.Collections;
using System.Collections.Generic;
using Enemies;
using Missiles;
using UnityEngine;

namespace Defendable
{
    public class CannonDefense : Defense
    {
        [SerializeField] protected DetectionRadius Detection;
        [SerializeField] protected Transform tower;
        [SerializeField] protected float rotationSpeed = 10f;
        [SerializeField] protected float angleThreshold = 5f;
        [SerializeField] protected Transform cannon;
        protected float lastShotTime;
        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }

        public void Awake()
        {
            base.Awake();
            Detection.SetAttackRange(AttackRange);
        }

        public void Update()
        {
            RotateAndAttack(Detection.Enemy);
        }

        protected void RotateAndAttack(Enemy enemy)
        {
            if(enemy == null) return;
            
            if (RotateToEnemy(enemy) && IsReady)
                Attack(enemy);
        }

        protected virtual void Attack(Enemy enemy)
        {
            var bullet = PoolManager.Instance.GetFromPool<CannonBullet>(PoolObjectType.CannonBullet);
            bullet.transform.position = cannon.position;
            bullet.transform.rotation = tower.rotation;
            bullet.Fire((enemy.transform.position - cannon.transform.position).normalized);
            lastShotTime = Time.time;
        }

        protected virtual bool RotateToEnemy(Enemy enemy)
        {
            var direction = (enemy.transform.position - tower.transform.position);
            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), tower.up);
            tower.rotation = Quaternion.RotateTowards(tower.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(targetRotation, tower.rotation) <= angleThreshold) return true;
            return false;
        }
    }
}