using System.Collections.Generic;
using Apex.AI;
using Enemies;
using UnityEngine;
using Defendable;
using Apex.AI.Serialization;

namespace AI
{
    public class ScanAction : ActionBase
    {
        private LayerMask LayerMask;
        private SpyEnemy Enemy;
        public override void Execute(IAIContext context)
        {
            LayerMask = LayerMask.GetMask("Defense");
            var c = (AIContext)context;
            
            List<Observation> defences = new List<Observation>();
            var colliders = Physics.OverlapSphere(Enemy.transform.position, Enemy.ScanRadius, LayerMask);
            foreach (var defence in colliders)
            {
                defences.Add(new Observation(defence.GetComponent<Defence>()));
            }
            Enemies.AIManager.Instance.AddObservation(defences);
        }
    }
}