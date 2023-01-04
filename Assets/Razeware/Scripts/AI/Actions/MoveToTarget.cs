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
        c.Enemy.MoveTo(c.Enemy.AttackTarget.Position);
    }
}
