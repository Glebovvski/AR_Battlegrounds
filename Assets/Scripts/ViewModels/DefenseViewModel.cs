using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class DefenseViewModel
{
    private CurrencyModel CurrencyModel { get; set; }

    private DefenseView DefenseView { get; set; }
    private ScriptableDefence Defence { get; set; }

    [Inject]
    private void Construct(CurrencyModel currencyModel)
    {
        CurrencyModel = currencyModel;
    }

    public DefenseViewModel(DefenseView view)
    {
        DefenseView = view;
        CurrencyModel.OnGoldAmountChanged += UpdateDefenseAffordable;
    }

    private void UpdateDefenseAffordable()
    {

    }
}
