using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public sealed class HasAttackTargetInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;

        if (c.Enemy.AttackTarget == null)
        {
            return 100f;
        }
        if(Vector3.Distance(c.Enemy.AttackTarget.transform.position, c.Enemy.Position) <= c.Enemy.AttackRange)
            return this.score;

        return 100f;
    }
}
