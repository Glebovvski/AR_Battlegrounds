using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyModel : MonoBehaviour
{
    public int Gold { get; private set; }

    private void Awake()
    {
        Gold = PlayerPrefs.GetInt("Gold", 1000);
    }

    public void Buy(int price)
    {
        
    }
}
