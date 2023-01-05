using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Defendable;
using UnityEngine;

public class BaseEnemyScan : ActionBase
{
    private LayerMask LayerMask;
        int Corner = 0;
        List<Observation> defences = new List<Observation>();
        public override void Execute(IAIContext context)
        {
            LayerMask = LayerMask.GetMask("Defense");
            var c = (AIContext)context;
            var Enemy = c.Enemy;

            var colliders = Physics.OverlapSphere(Enemy.Position, Enemy.ScanRange, LayerMask);
            foreach (var defence in colliders)
            {
                defences.Add(new Observation(defence.GetComponent<Defense>()));
            }
            Enemies.AIManager.Instance.AddObservation(defences);
            defences.Clear();


            Debug.LogError("SCAN FINISHED");
        }
}
