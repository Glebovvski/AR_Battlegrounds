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
                defences.Add(new Observation(defence.GetComponent<Defence>()));
            }
            Enemies.AIManager.Instance.AddObservation(defences);
            defences.Clear();
        }
    }
}