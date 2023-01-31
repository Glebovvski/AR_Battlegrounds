using System;
using Zenject;

public class WinViewModel : IInitializable
{
    public event Action<float> OnTimerChange;
    private WinModel WinModel { get; set; }
    private StatManager StatManager { get; set; }
    private GameControlModel GameModel { get; set; }

    public event Action<int> OnShowWinScreen;
    public event Action OnCloseWinScreen;

    [Inject]
    private void Construct(WinModel winModel, StatManager statManager, GameControlModel gameModel)
    {
        WinModel = winModel;
        StatManager = statManager;
        GameModel = gameModel;
    }

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
    public string GetEnemiesKilled() => StatManager.EnemiesKilled.ToString();
    public string GetDefensesDestroyed() => StatManager.DefensesDestroyed.ToString();

    public void NextLevel()
    {
        GameModel.Restart();
        OnCloseWinScreen?.Invoke();
    }

    private void TimerDataChanged() => OnTimerChange?.Invoke(WinModel.Timer);

}
