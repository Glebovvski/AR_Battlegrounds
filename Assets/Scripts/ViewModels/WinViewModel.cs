using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WinViewModel : IInitializable
{
    public event Action<float> OnTimerChange;
    private WinModel WinModel { get; set; }
    private StatManager StatManager { get; set; }

    public event Action<int> OnShowWinScreen;

    public void Initialize()
    {
        WinModel.OnTimerChange += TimerDataChanged;
        WinModel.OnWin += ShowWinScreen;
    }

    private void ShowWinScreen()
    {
        OnShowWinScreen?.Invoke(WinModel.GetStars());
    }

    public string GetTimer() => WinModel.GetTimer();
    public string GetBestScore() => WinModel.GetBestScore();
    public int GetEnemiesKilled() => StatManager.EnemiesKilled;
    public int GetDefensesDestroyed() => StatManager.DefensesDestroyed;

    [Inject]
    private void Construct(WinModel winModel, StatManager statManager)
    {
        WinModel = winModel;
        StatManager = statManager;
    }

    private void TimerDataChanged() => OnTimerChange?.Invoke(WinModel.Timer);

}
