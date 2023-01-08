using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using UnityEngine;

public class IsActiveDefensesInSight : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var activeDefenses = Enemies.AIManager.Instance.GetActiveDefenses();
        return activeDefenses.Count * 100;
    }
}
