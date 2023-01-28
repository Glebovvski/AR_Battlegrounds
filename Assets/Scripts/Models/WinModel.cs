using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Zenject;

public class WinModel : ITickable, IInitializable
{
    private const string timerKey = "Timer";

    private GameGrid Grid { get; set; }
    private StatManager StatManager { get; set; }

    private bool timerActive = false;

    public event Action OnTimerChange;
    private float timer = 0;
    public float Timer
    {
        get => timer;
        private set
        {
            timer = value;
            OnTimerChange?.Invoke();
        }
    }

    [Inject]
    private void Construct(GameGrid grid, StatManager statManager)
    {
        Grid = grid;
        StatManager = statManager;
    }

    public void Initialize()
    {
        Grid.OnGridCreated += StartTimer;
        AIManager.Instance.OnEnemyDestroyed += CheckIsWon;
    }

    private void CheckIsWon(int enemies)
    {
        if (enemies > 0) return;
        Win();
    }

    public event Action OnWin;
    private void Win()
    {
        GetStars();
        OnWin?.Invoke();
    }

    public int GetStars()
    {
        int stars = 1;
        stars += Timer < PlayerPrefs.GetFloat(timerKey, 0) ? 0 : 1;
        stars += StatManager.EnemiesKilled > StatManager.DefensesDestroyed ? 1 : 0;
        return stars;
    }

    public void StartTimer() => timerActive = true;

    public void Tick()
    {
        if (timerActive)
            Timer += Time.deltaTime * Time.timeScale;
    }
}
