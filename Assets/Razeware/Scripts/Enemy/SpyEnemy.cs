using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class SpyEnemy : Enemy
    {
        [SerializeField] private float scanRadius;
        public float ScanRadius => scanRadius;
    }
}