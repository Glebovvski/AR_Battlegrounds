using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using UnityEngine;

public class IsTankInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var tank = Enemies.AIManager.Instance.GetClosestEnemyByType(enemy, Enemies.EnemyType.Mono);
        if (tank != null)
            return 100;
        else
            return 0;
    }
}
