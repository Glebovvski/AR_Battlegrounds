using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defendable
{
    public class MissileLauncher : Defense
    {
        private float lastShotTime;
        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }
    }
}