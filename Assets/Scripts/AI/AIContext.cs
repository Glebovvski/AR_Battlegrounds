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
        public Enemy Enemy { get; private set; }
        public AIContext(Enemy enemy)
        {
            Enemy = enemy;
        }
    }
}