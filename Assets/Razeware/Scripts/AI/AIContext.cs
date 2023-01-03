using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;
using Defendable;

namespace AI
{
    public class AIContext : IAIContext
    {
        public List<Defence> Defenses {get; private set;}
        public AIContext()
        {
            Defenses = new List<Defence>();
        }

        public void SetDefences(List<Defence> defences)
        {
            Defenses.Clear();
            Defenses = defences;
        }
    }
}