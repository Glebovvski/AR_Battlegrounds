using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DefensesView : MonoBehaviour
{
    [SerializeField] private DefencesViewModel defencesViewModel;
    [SerializeField] private DefenseView buttonPrefab;
    [SerializeField] private Transform content;

    List<DefenseViewModel> defenseViews = new List<DefenseViewModel>();

    private void Start()
    {
        foreach (var defense in defencesViewModel.GetDefencesList())
        {
            var button = Instantiate(buttonPrefab, content);
            button.Init(defense, defencesViewModel);
            defenseViews.Add(new DefenseViewModel(defense, button));
        }
    }

    public void CancelSelection()
    {
        defencesViewModel.DefenseDeselected();
    }
}
