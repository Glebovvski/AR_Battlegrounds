using Defendable;
using UnityEngine;
using Zenject;

public class GameTimeModel : IInitializable, ITickable
{
    private DefensesModel DefensesModel { get; set; }
    private GameGrid Grid { get; set; }
    private InputManager InputManager { get; set; }
    private CastleDefense Castle { get; set; }
    private CurrencyModel CurrencyModel { get; set; }
    private WinModel WinModel { get; set; }

    private float lastDropTime = 0;
    private float secondsToDropGold = 5f;
    private float goldPercent = 2f;

    public bool IsPaused = Time.timeScale != 1;
    private bool isWon = false;
    private bool gridCreated = false;

    [Inject]
    private void Construct(DefensesModel defensesModel, GameGrid grid, InputManager inputManager, CastleDefense castle, CurrencyModel currencyModel, WinModel winModel)
    {
        DefensesModel = defensesModel;
        Grid = grid;
        InputManager = inputManager;
        Castle = castle;
        CurrencyModel = currencyModel;
        WinModel = winModel;
    }

    private void Pause()
    {
        Time.timeScale = 0.005f;
        gridCreated = true;
    }

    private void Resume()
    {
        Time.timeScale = 1;
    }

    public void Initialize()
    {
        DefensesModel.OnSelectDefenseClick += Pause;
        Grid.OnGridCreated += Pause;
        InputManager.OnActiveDefenseClick += Pause;

        WinModel.OnWin += Win;

        InputManager.OnEnemyClick += Resume;
        DefensesModel.OnDefenseDeselected += Resume;
        Castle.OnLose += Resume;
    }

    private void Win() => isWon = true;

    public void Tick()
    {
        if (!gridCreated) return;
        if (IsPaused || !Castle.IsAlive || isWon) return;

        if (Time.time - lastDropTime > secondsToDropGold)
        {
            CurrencyModel.AddGold(Mathf.RoundToInt(CurrencyModel.Gold * goldPercent / 100f));
            lastDropTime = Time.time;
        }
    }
}
