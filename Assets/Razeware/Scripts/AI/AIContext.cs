using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;
using Defendable;
using Enemies;

namespace AI
{
    public class AIContext : IAIContext
    {
        public IEnemy Enemy { get; private set; }
        public AIContext(IEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}