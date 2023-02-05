using System;
using UnityEngine;

public class CurrencyModel
{
    public float SecondsToDropGold { get; private set; } = 5f;
    public float GoldPercent { get; private set; } = 2f;

    public event Action<int> OnGoldAmountChanged;
    private int _gold;
    public int Gold
    {
        get => _gold;
        private set
        {
            _gold = value;
            PlayerPrefs.SetInt("Gold", value);
            OnGoldAmountChanged?.Invoke(_gold);
        }
    }

    public CurrencyModel()
    {
        // PlayerPrefs.DeleteAll();
        Gold = PlayerPrefs.GetInt("Gold", 1000000);
    }

    public void Buy(int price)
    {
        Gold -= price;
    }

    public void AddGold(int value)
    {
        Gold += value;
    }
}
