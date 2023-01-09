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

        protected override bool RotateToEnemy(Enemy enemy)
        {
            var direction = (enemy.transform.position - tower.transform.position);
            var targetRotation = Quaternion.LookRotation(new Vector3(-69f, direction.y, direction.z), tower.up);
            tower.rotation = Quaternion.RotateTowards(tower.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(targetRotation, tower.rotation) <= angleThreshold) return true;
            return false;
        }
    }
}