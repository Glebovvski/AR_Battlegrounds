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
    private ScriptableDefense Defense { get; set; }

    [Inject]
    private void Construct(CurrencyModel currencyModel)
    {
        CurrencyModel = currencyModel;
    }

    public DefenseViewModel(ScriptableDefense defense, DefenseView view)
    {
        Defense = defense;
        DefenseView = view;
        CurrencyModel.OnGoldAmountChanged += UpdateDefenseAffordable;
    }

    private void UpdateDefenseAffordable()
    {
        DefenseView.UpdateButton(Defense.Price < CurrencyModel.Gold);
    }
}
