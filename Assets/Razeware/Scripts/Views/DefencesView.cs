using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesView : MonoBehaviour
{
    [SerializeField] private DefencesViewModel defencesViewModel;
    [SerializeField] private DefenceView buttonPrefab;
    [SerializeField] private Transform content;

    private void Start() 
    {
        foreach (var defense in defencesViewModel.GetDefencesList())
        {
            var button = Instantiate(buttonPrefab, content);
            button.Init(defense.Type, defencesViewModel);
        }
    }

    public void CancelSelection()
    {
        defencesViewModel.DefenseDeselected();
    }
}
