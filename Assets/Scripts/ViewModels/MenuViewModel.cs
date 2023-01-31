using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuViewModel : IInitializable
{
    private DefensesViewModel DefensesViewModel { get; set; }

    [Inject]
    private void Construct(DefensesViewModel defensesViewModel)
    {
        DefensesViewModel = defensesViewModel;
    }

    public void Initialize()
    {
        
    }

    public event Action OnOpen;
    public void OpenMenu()
    {
        OnOpen?.Invoke();
        DefensesViewModel.Close();
    }

    public event Action OnClose;
    public void Close()=>OnClose?.Invoke();
}
