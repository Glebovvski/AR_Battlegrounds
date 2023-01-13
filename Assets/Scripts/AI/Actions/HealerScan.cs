using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public class HealerScan : ActionBase
{
    [ApexSerialization] private float healthPercent;
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var enemiesInRange = Enemies.AIManager.Instance.GetEnemiesInRangeWithHealthLowerThan(enemy, healthPercent);
        if (enemiesInRange.Count == 0)
            return;
        var closestEnemy = Enemies.AIManager.Instance.GetClosest(enemy, enemiesInRange);
        enemy.FollowTarget = closestEnemy;
        enemy.MoveTo(enemy.FollowTarget.Position);
    }
}
