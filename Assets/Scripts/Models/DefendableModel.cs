using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Defendable
{
    public class DefendableModel : MonoBehaviour
    {
        private List<Defense> defences = new List<Defense>();
        private List<GridCell> defencePositions = new List<GridCell>();
        public int Health => defences.Sum(x=>x.Health);
        public int TotalDefences => defences.Count;
    }
}