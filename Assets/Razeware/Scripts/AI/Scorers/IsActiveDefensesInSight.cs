using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public class IsActiveDefensesInSight : ContextualScorerBase
{
    [ApexSerialization] private bool not;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var activeDefenses = Enemies.AIManager.Instance.GetActiveDefenses();
        float score = activeDefenses.Count * 100;
        return not ? -score : score;
    }
}
