using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DefensesView : MonoBehaviour
{
    private CurrencyModel CurrencyModel { get; set; }

    [SerializeField] private DefencesViewModel defencesViewModel;
    [SerializeField] private DefenseView buttonPrefab;
    [SerializeField] private Transform content;

    List<DefencesViewModel> defenseViews = new List<DefencesViewModel>();

    [Inject]
    private void Construct(CurrencyModel currencyModel)
    {
        CurrencyModel = currencyModel;
    }


    private void Start()
    {
        CurrencyModel.OnGoldAmountChanged += UpdateButtons;
        foreach (var defense in defencesViewModel.GetDefencesList())
        {
            var button = Instantiate(buttonPrefab, content);
            button.Init(defense, defencesViewModel);
            defenseViews.Add(new DefencesViewModel())
        }
    }

    private void UpdateButtons()
    {
        foreach (var button in buttons)
        {
            button.UpdateButton()
        }
    }

    public void CancelSelection()
    {
        defencesViewModel.DefenseDeselected();
    }

    private void OnDestroy()
    {
        CurrencyModel.OnGoldAmountChanged -= UpdateButtons;
    }
}
