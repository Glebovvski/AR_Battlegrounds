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

    private float lastDropTime = 0;
    private float secondsToDropGold = 5f;
    private float goldPercent = 2f;

    public bool IsPaused = Time.timeScale != 1;

    [Inject]
    private void Construct(DefensesModel defensesModel, GameGrid grid, InputManager inputManager, CastleDefense castle, CurrencyModel currencyModel)
    {
        DefensesModel = defensesModel;
        Grid = grid;
        InputManager = inputManager;
        Castle = castle;
        CurrencyModel = currencyModel;
    }

    private void Pause()
    {
        Time.timeScale = 0.05f;
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

        InputManager.OnEnemyClick += Resume;
        DefensesModel.OnDefenseDeselected += Resume;
        Castle.OnLose += Resume;
    }

    public void Tick()
    {
        if (IsPaused || !Castle.IsAlive) return;

        if (Time.time - lastDropTime > secondsToDropGold)
        {
            CurrencyModel.AddGold(Mathf.RoundToInt(CurrencyModel.Gold * goldPercent / 100f));
            lastDropTime = Time.time;
        }
    }
}
