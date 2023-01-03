using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public class IsCloseToGrid : QualifierBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        return Vector3.Distance(Enemies.AIManager.Instance.Grid.transform.position, enemy.Position) < enemy.ScanRange ? 100 : 0; 
    }
}
