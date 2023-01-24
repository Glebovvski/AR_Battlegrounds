using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WinViewModel : IInitializable
{
    public event Action<float> OnTimerChange;
    private WinModel WinModel { get; set; }

    public void Initialize()
    {
        WinModel.OnTimerChange += TimerDataChanged;
    }

    [Inject]
    private void Construct(WinModel winModel)
    {
        WinModel = winModel;
    }

    private void TimerDataChanged() => OnTimerChange?.Invoke(WinModel.Timer);

}
