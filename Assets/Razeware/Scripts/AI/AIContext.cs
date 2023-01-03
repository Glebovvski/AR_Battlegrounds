using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;
using Defendable;

namespace AI
{
    public class AIContext : IAIContext
    {
        public GameGrid Grid {get; private set; }
        public List<Defence> Defenses {get; private set;}
        public AIContext(GameGrid grid)
        {
            Defenses = new List<Defence>();
            Grid = grid;
        }

        public void SetDefences(List<Defence> defences)
        {
            Defenses.Clear();
            Defenses = defences;
        }
    }
}