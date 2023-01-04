using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using UnityEngine;

public class GetBestAttackTarget : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        foreach (var defense in enemy.DefenseTypeToScore)
        {
            var bestDefenseType = defense;
            var closestObservation = Enemies.AIManager.Instance.GetClosestByType(enemy, defense.Key);
            var path = enemy.GetCalculatedPath(closestObservation);
        }
    }
}
