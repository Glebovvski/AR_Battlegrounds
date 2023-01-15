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
        public bool InDefenseSelectionMode { get; private set; } = false;

        public DefensesModel()
        {
            GetDefensesInfo();
        }

        private void GetDefensesInfo()
        {
            defenses = Resources.LoadAll<ScriptableDefense>("SO/Defense").ToList();
        }

        public event Action<ScriptableDefense> OnDefenseSelected;
        public event Action OnSelectDefenseClick;
        public void DefenseSelected(ScriptableDefense info)
        {
            InDefenseSelectionMode = true;
            OnDefenseSelected?.Invoke(info);
            OnSelectDefenseClick?.Invoke();
        }

        public event Action OnDefenseDeselected;
        public void DefenseDeselected()
        {
            InDefenseSelectionMode = false;
            OnDefenseDeselected?.Invoke();
        }
    }
}