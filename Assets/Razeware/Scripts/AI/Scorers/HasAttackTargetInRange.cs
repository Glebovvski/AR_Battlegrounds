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

        if (enemy.GetAttackTarget() == null || !enemy.GetAttackTarget().IsAlive) return 0;
        if(IsNotInAttackRange(enemy)) return 0;

        return 100;
    }

    private bool IsNotInAttackRange(Enemy enemy) => Vector3.Distance(enemy.GetAttackTarget().Position, enemy.Position) > enemy.AttackRange;
}
