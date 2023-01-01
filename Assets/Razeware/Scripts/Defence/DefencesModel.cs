using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defendable
{
    public class DefencesModel : MonoBehaviour
    {
        [SerializeField] private List<DefenceInfo> defences = new List<DefenceInfo>();
        public List<DefenceInfo> List => defences;
    }

    [Serializable]
    public class DefenceInfo
    {
        [field:SerializeField] public PoolObjectType Type {get;set;}
        [field:SerializeField] public Defence Defence {get;set;}
    }
}