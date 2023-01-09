using System.Collections;
using System.Collections.Generic;
using Enemies;
using Missiles;
using UnityEngine;

namespace Defendable
{
    public class MissileLauncher : CannonDefense
    {
        [SerializeField] private DetectionRadius Detection;
        [SerializeField] private Transform cannon;


        private float lastShotTime;
        protected override bool IsReady { get => Time.time - lastShotTime > RelaodTime; set => IsReady = value; }

        protected override bool RotateToEnemy(Enemy enemy)
        {
            return false;
        }
    }
}