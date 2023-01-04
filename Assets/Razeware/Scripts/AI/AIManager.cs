using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager Instance;

        [field: SerializeField] public GameGrid Grid { get; private set; }
        [field: SerializeField] public List<Observation> Observations { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            Observations = new List<Observation>();
        }

        public void AddObservation(Observation observation)
        {
            if(Observations.Any(x=> x.Equals(observation))) return;

            Observations.Add(observation);
        }

        public void AddObservation(List<Observation> observations)
        {
            foreach (var observation in observations)
            {
                AddObservation(observation);
            }
        }

        public void RemoveObservation(Observation observation)
        {
            if (Observations.Contains(observation))
                Observations.Remove(observation);
        }

        public Observation GetClosest(IEnemy enemy) => GetClosest(enemy, Observations);

        public Observation GetClosestByType(IEnemy enemy, DefenseType type)
        {
            var observations = Observations.Where(x => x.Defence.Type == type).ToList();
            return GetClosest(enemy, observations);
        }

        private Observation GetClosest(IEnemy enemy, List<Observation> observations)
        {
            Vector3 position = enemy.GameObject.transform.position;
            float distance = 1000;
            Observation closest = null;
            for (int i = 0; i < Observations.Count; i++)
            {
                var currDistance = Vector3.Distance(position, Observations[i].Position);
                if (currDistance < distance)
                {
                    distance = currDistance;
                    closest = Observations[i];
                }
            }
            return closest;
        }
    }
}