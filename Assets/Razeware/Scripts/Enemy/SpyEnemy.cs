using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class SpyEnemy : Enemy
    {
        public bool IsScanFinished { get; private set; } = false;
        public void SetIsScanFinished(bool value) => IsScanFinished = value;
    }
}