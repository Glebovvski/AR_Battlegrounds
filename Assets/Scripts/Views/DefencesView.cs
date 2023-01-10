using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DefensesView : MonoBehaviour
{
    private CurrencyModel CurrencyModel { get; set; }

    [SerializeField] private DefencesViewModel defencesViewModel;
    [SerializeField] private DefenceView buttonPrefab;
    [SerializeField] private Transform content;

    List<DefenceView> buttons = new List<DefenceView>();

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
            buttons.Add(button);
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
