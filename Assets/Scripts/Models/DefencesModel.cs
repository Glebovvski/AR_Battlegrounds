using System.Collections.Generic;
using UnityEngine;

namespace Defendable
{
    public class DefensesModel
    {
        private List<ScriptableDefence> defenses = new List<ScriptableDefence>();
        public List<ScriptableDefence> List => defenses;

        public DefensesModel()
        {
            GetDefensesInfo();
        }

        private void GetDefensesInfo()
        {
            var defences = Resources.LoadAll<ScriptableDefence>("SO/Defense");
        }
    }

    public class DefenseInfo
    {
        public DefenseInfo(PoolObjectType type, Defense defense, int price)
        {
            Type = type;
            Defence = defense;
            Price = price;    
        }

        public PoolObjectType Type { get; set; }
        public Defense Defence { get; set; }
        public int Price { get; set; }
        public bool IsAffordable { get; private set; }
    }
}