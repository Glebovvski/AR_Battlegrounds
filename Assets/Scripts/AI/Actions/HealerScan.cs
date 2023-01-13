using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using UnityEngine;

public class HealerScan : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var enemiesInRange = Enemies.AIManager.Instance.GetEnemiesInRangeInCurrentHealthOrder(enemy);
    }
}
