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
        timerActive = false;
        CheckTimer();
        OnWin?.Invoke();
    }

    private void CheckTimer()
    {
        var bestScore = PlayerPrefs.GetFloat(timerKey, 0);
        if (bestScore < Timer)
            PlayerPrefs.SetFloat(timerKey, Timer);
    }

    public int GetStars()
    {
        int stars = 1;
        stars += Timer < PlayerPrefs.GetFloat(timerKey, 0) ? 0 : 1;
        stars += StatManager.EnemiesKilled > StatManager.DefensesDestroyed ? 1 : 0;
        return stars;
    }

    public string GetTimer() => ConvertFloatToTime(Timer);

    public string GetBestScore() => ConvertFloatToTime(PlayerPrefs.GetFloat(timerKey, 0));

    private string ConvertFloatToTime(float value)
    {
        int timeInSecondsInt = (int)value;
        int minutes = (int)value / 60;
        int seconds = timeInSecondsInt - (minutes * 60);
        return String.Format("{0}:{1}", minutes, seconds);
    }

    public void StartTimer() => timerActive = true;

    public void Tick()
    {
        if (timerActive)
            Timer += Time.deltaTime * Time.timeScale;
    }
}
