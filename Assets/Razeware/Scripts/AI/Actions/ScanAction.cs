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
            
            List<Defence> defences = new List<Defence>();
            var colliders = Physics.OverlapSphere(Enemy.transform.position, Enemy.ScanRadius, LayerMask);
            foreach (var defence in colliders)
            {
                defences.Add(defence.GetComponent<Defence>());
            }

            c.SetDefences(defences);
        }
    }
}