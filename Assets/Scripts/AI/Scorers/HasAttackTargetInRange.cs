using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Enemies;
using UnityEngine;

public class HasAttackTargetInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;
        var attackTarget = enemy.AttackTarget;
        if (attackTarget == null) return 0;
        if (!attackTarget.IsAlive) return 0;
        if (!enemy.IsAttackTargetInRange) return 0;

        return 100;
    }

    private bool IsNotInAttackRange(Enemy enemy)
    {
        var result = (enemy.AttackTarget.Position - enemy.Position).sqrMagnitude > enemy.AttackRange * enemy.AttackRange;
        return result;
    }
}
