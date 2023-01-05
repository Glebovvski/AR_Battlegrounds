using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public sealed class HasNoAttackTargetInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;

        if (c.Enemy.GetAttackTarget() == null || !c.Enemy.GetAttackTarget().IsAlive)
        {
            return 100f;
        }
        if(Vector3.Distance(c.Enemy.GetAttackTarget().Position, c.Enemy.Position) <= c.Enemy.AttackRange)
            return this.score;

        return 100f;
    }
}
