using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Defendable
{
    public class DefensesModel
    {
        private List<ScriptableDefense> defenses = new List<ScriptableDefense>();
        public List<ScriptableDefense> List => defenses;

        public DefensesModel()
        {
            GetDefensesInfo();
        }

        private void GetDefensesInfo()
        {
            defenses = Resources.LoadAll<ScriptableDefense>("SO/Defense").ToList();
        }

        public event Action<ScriptableDefense> OnDefenseSelected;
        public void DefenseSelected(ScriptableDefense info)
        {
            OnDefenseSelected?.Invoke(info);
        }

        public event Action OnDefenseDeselected;
        public void DefenseDeselected() => OnDefenseDeselected?.Invoke();
    }
}