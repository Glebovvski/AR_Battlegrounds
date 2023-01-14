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
        c.Enemy.MoveTo(c.Enemy.GetAttackTarget().Position);
        // Debug.LogError(string.Format("{0} goes to {1}", c.Enemy.name, c.Enemy.GetAttackTarget().Defense.name));
    }
}
