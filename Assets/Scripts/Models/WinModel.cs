using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WinModel : ITickable, IInitializable
{
    private GameGrid Grid { get; set; }

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
    private void Construct(GameGrid grid)
    {
        Grid = grid;
    }

    public void Initialize()
    {
        Grid.OnGridCreated += StartTimer;
    }

    public event Action OnWin;

    //TODO: ADD STARS BASED ON RESULTS

    public void Win() => OnWin?.Invoke();

    public void StartTimer() => timerActive = true;

    public void Tick()
    {
        if (timerActive)
            Timer += Time.deltaTime * Time.timeScale;
    }
}
