using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Defendable
{
    public class DefendableModel : MonoBehaviour
    {
        private List<Defence> defences = new List<Defence>();
        private List<GridCell> defencePositions = new List<GridCell>();
        public int Health => defences.Sum(x=>x.Health);
        public int TotalDefences => defences.Count;
    }
}