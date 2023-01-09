using System.Collections;
using System.Collections.Generic;
using Enemies;
using Missiles;
using UnityEngine;

namespace Defendable
{
    public class MissileLauncher : CannonDefense
    {
        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }

        protected override void Attack(Enemy enemy)
        {
            var bullet = PoolManager.Instance.GetFromPool<LaunchableMissile>(PoolObjectType.LaunchableMissile);
            bullet.transform.position = cannon.position;
            bullet.transform.rotation = tower.rotation;
            var target = enemy.transform.position;
            bullet.Launch(target);
            lastShotTime = Time.time;
        }

        protected override bool RotateToEnemy(Enemy enemy)
        {
            var direction = (enemy.transform.position - tower.transform.position);
            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), tower.up);
            tower.rotation = Quaternion.RotateTowards(tower.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(targetRotation, tower.rotation) <= angleThreshold) return true;
            return false;
        }
    }
}