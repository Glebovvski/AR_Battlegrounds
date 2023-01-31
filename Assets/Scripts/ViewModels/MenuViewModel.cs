using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuViewModel : MonoBehaviour
{
    public event Action OnOpen;
    public void OpenMenu()
    {
        OnOpen?.Invoke();
    }
}
