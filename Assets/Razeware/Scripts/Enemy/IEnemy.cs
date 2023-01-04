using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public interface IEnemy
    {
        GameObject GameObject { get; }
        NavMeshAgent NavMeshAgent { get; }
        Vector3? MoveTarget { get; set; }
        Observation AttackTarget { get; set; }
        float ScanRange { get; }
        Vector3 Position { get; }

        void MoveTo(Vector3 destination);
    }
}