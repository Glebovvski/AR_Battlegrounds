using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public class HasObservations : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        return Enemies.AIManager.Instance.Observations.Count * 10;
    }
}
