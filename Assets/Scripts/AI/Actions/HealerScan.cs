using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.Serialization;
using Enemies;
using UnityEngine;

public class HealerScan : ActionBase
{
    [ApexSerialization] private float healthPercent;
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        Enemy closestEnemy = null;
        enemy.FollowTarget = null;
        var enemiesInRange = Enemies.AIManager.Instance.GetEnemiesInRangeWithHealthLowerThan(enemy, healthPercent);
        if (enemiesInRange.Count == 0)
            return;
        closestEnemy = Enemies.AIManager.Instance.GetClosest(enemy, enemiesInRange);

        if (enemy.IsNewDestination(enemy.FollowTarget.Position))
        {
            enemy.FollowTarget = closestEnemy;
            enemy.MoveTo(enemy.FollowTarget.Position);
        }
    }
}
