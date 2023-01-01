using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

namespace Defendable
{
    public class DetectionRadius : MonoBehaviour
    {
        [SerializeField] private SphereCollider Radius;
        private List<Enemy> enemiesInRange = new List<Enemy>();
        public List<Enemy> Enemies => enemiesInRange;

        public void SetAttackRange(int radius) => Radius.radius = radius;


        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Enemy>(out var enemy)) return;

Debug.LogError("SEE ENEMY");
            enemiesInRange.Add(enemy);
        }

        private void OnTriggerExit(Collider other) 
        {
            if(!other.TryGetComponent<Enemy>(out var enemy)) return;

            enemiesInRange.Remove(enemy);
        }
    }
}