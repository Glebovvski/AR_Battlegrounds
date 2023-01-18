using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using UnityEngine;

public class IsTargetClose : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        if(enemy.AttackTarget == null)
            return 0;
        return (enemy.AttackTarget.Position - enemy.Position).sqrMagnitude - (enemy.ScanRange*enemy.ScanRange);
    }
}
