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

        if(enemy.GetAttackTarget() == null)
            return 0;
        return (enemy.GetAttackTarget().Position - enemy.Position).sqrMagnitude - enemy.ScanRange;
    }
}
