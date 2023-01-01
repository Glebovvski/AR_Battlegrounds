using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private ScriptableEnemy SO;

        public int Health {get; private set; }
        public int AttackForce {get; set;}

        public void GetData()
        {
            Health = SO.Health;
            AttackForce = SO.AttackForce;
        }

    }
}