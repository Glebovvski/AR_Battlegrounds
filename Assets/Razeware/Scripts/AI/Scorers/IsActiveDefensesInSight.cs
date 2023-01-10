using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public class IsActiveDefensesInSight : ContextualScorerBase
{
    [ApexSerialization] private bool not;
    [ApexSerialization] private DefenseType except;
    [ApexSerialization] private int ExceptTypeThreshold;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var exceptDefenses = Enemies.AIManager.Instance.GetObservationsOfType(except);
        var activeDefenses = Enemies.AIManager.Instance.GetActiveDefenses(except);

        if (exceptDefenses.Count < ExceptTypeThreshold)
        {
            float score = activeDefenses.Count * 100;
            return not ? -score : score;
        }
        else return not ? 100 : 0;
    }
}
