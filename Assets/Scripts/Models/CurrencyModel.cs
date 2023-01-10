using System;
using UnityEngine;

public class CurrencyModel
{
    public event Action OnGoldAmountChanged;
    public int Gold
    {
        get => Gold; 
        private set
        {
            Gold = value;
            PlayerPrefs.SetInt("Gold", value);
            OnGoldAmountChanged?.Invoke();
        }
    }

    private void Awake()
    {
        Gold = PlayerPrefs.GetInt("Gold", 1000);
    }

    public void Buy(int price)
    {
        Gold -= price;
    }
}
