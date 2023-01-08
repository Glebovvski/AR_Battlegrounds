using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Defendable
{
    public class TrapDefense : Defense
    {
        [SerializeField] private Animator animator;
        private float lastShotTime;

        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }

        private NavMeshSurface surface;

        private void OnEnable()
        {
            OnDefenseSet += UpdateNavMesh;
        }

        private void UpdateNavMesh()
        {
            transform.parent.TryGetComponent<NavMeshSurface>(out surface);
            if (surface)
            {
                Debug.LogError("TRAP REBUILD");
                surface.BuildNavMesh();
            }
        }

        private void OnDisable()
        {
            OnDefenseSet -= UpdateNavMesh;
        }
    }
}