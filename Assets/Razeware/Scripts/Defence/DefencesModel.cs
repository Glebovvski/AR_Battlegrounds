using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defendable
{
    public class DefencesModel : MonoBehaviour
    {
        [SerializeField] private List<Defence> defences = new List<Defence>();
        public List<Defence> List => defences;
    }
}