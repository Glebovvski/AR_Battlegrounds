using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using UnityEngine;

public class MoveToTarget : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;

        if (c.Enemy.AttackTarget != null && c.Enemy.AttackTarget.IsAlive && c.Enemy.IsNewDestination(c.Enemy.AttackTarget.Position))
        {
            c.Enemy.MoveTo(c.Enemy.AttackTarget.Position);
        }
    }
}