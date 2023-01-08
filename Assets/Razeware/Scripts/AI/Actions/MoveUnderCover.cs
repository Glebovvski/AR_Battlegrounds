using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Enemies;
using UnityEngine;

public class MoveUnderCover : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var closeEnemies = Enemies.AIManager.Instance.GetClosestEnemies(enemy);
    }
}
