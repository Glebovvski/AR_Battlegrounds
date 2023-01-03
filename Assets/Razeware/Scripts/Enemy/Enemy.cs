using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Apex.AI;
using Apex.AI.Components;
using Defendable;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IContextProvider, IEnemy
    {
        [SerializeField] private ScriptableEnemy SO;
        [SerializeField] private NavMeshAgent navMeshAgent;

        public int Health {get; private set; }
        public int AttackForce {get; set;}
        public int Speed {get;set;}
        public bool IsAlive => true; //TODO: add check
        public AIContext Context {get; private set; }
        public GameObject GameObject => this.gameObject;
        public NavMeshAgent NavMeshAgent => navMeshAgent;
        public Vector3? MoveTarget { get; set; }
        public Defence AttackTarget { get; set; }
        public float ScanRange => SO.ScanRange;
        public Vector3 Position => this.gameObject.transform.position;

        public IAIContext GetContext(Guid aiId) => Context;

        public void Awake() 
        {
            Context = new AIContext(this);
            GetData();
        }

        public void GetData()
        {
            Health = SO.Health;
            AttackForce = SO.AttackForce;
            Speed = SO.Speed;
            navMeshAgent.speed = SO.Speed;
        }

        public void MoveTo(Vector3 destination)
        {
            NavMeshAgent.destination = destination;
        }
    }
}