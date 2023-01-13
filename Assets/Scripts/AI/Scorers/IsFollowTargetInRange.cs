using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Enemies;
using UnityEngine;

public class IsFollowTargetInRange : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;
        var attackTarget = enemy.GetAttackTarget();
        if (attackTarget == null) return 0;
        if (!attackTarget.IsAlive) return 0;
        if (IsNotInAttackRange(enemy)) return 0;

        return 100;
    }

    private bool IsNotInAttackRange(Enemy enemy)
    {
        var result = (enemy.GetAttackTarget().Position - enemy.Position).sqrMagnitude > enemy.AttackRange*enemy.AttackRange;
        return result;
    }
}
