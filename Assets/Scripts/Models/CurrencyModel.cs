using System;
using UnityEngine;

public class CurrencyModel
{
    public event Action OnGoldAmountChanged;
    private int _gold;
    public int Gold
    {
        get => _gold;
        private set
        {
            _gold = value;
            PlayerPrefs.SetInt("Gold", value);
            OnGoldAmountChanged?.Invoke();
        }
    }

    public CurrencyModel()
    {
        PlayerPrefs.DeleteAll();
        Gold = PlayerPrefs.GetInt("Gold", 100000);
    }

    public void Buy(int price)
    {
        Gold -= price;
    }
}
