using System;
using System.Collections;
using System.Collections.Generic;
using Apex.AI;
using Apex.AI.Components;
using UnityEngine;

namespace AI
{
    public class AIContextProvider : IContextProvider
    {
        private AIContext Context { get; set; }
        public IAIContext GetContext(Guid aiId) => Context;
    }
}