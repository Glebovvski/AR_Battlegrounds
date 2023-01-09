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

        private List<EnemyTimePositionInfo> enemyPositions = new List<EnemyTimePositionInfo>();
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

            enemyPositions.Add(new EnemyTimePositionInfo(Time.time, enemy.Position));

            if (Quaternion.Angle(targetRotation, tower.rotation) <= angleThreshold) return true;
            return false;
        }
    }
}


public class EnemyTimePositionInfo
{
    public float Timestamp{get;private set;}
    public Vector3 Position {get;private set;}

    public EnemyTimePositionInfo(float time, Vector3 position)
    {
        Timestamp = time;
        Position = position;
    }
}