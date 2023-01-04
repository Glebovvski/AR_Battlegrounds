using System.Collections;
using System.Collections.Generic;
using Apex.AI;
using Enemies;
using UnityEngine;

namespace AI
{
    public class MoveAroundGrid : ActionBase
    {
        public override void Execute(IAIContext context)
        {
            Debug.LogError("MOVING AROUND: ");
            var c = (AIContext)context;
            var enemy = c.Enemy;

            var corners = Enemies.AIManager.Instance.Grid.Corners();
            int closest = GetClosestCornerIndex(corners, enemy);
            enemy.MoveTo(corners[closest]);
        }

        private int GetClosestCornerIndex(List<Vector3> Corners, IEnemy enemy)
        {
            float distance = 1000;
            int cornerIndex = 0;
            for (int i = 0; i < Corners.Count; i++)
            {
                var newDistance = Vector3.Distance(Corners[i], enemy.Position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    cornerIndex = i;
                }
            }
            return cornerIndex + 1;
        }
    }
}