using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GoldViewModel
{
    private CurrencyModel CurrencyModel { get; set; }

    public event Action<int> OnGoldChanged;

    public GoldViewModel(CurrencyModel currencyModel)
    {
        CurrencyModel = currencyModel;
        CurrencyModel.OnGoldAmountChanged += GoldChanged;
    }

    private void GoldChanged(int value) => OnGoldChanged?.Invoke(value);
}
