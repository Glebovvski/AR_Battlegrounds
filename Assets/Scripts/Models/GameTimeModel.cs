using Defendable;
using UnityEngine;
using Zenject;

public class GameTimeModel : IInitializable
{
    private DefensesModel DefensesModel { get; set; }
    private GameGrid Grid { get; set; }
    private InputManager InputManager { get; set; }
    private CastleDefense Castle { get; set; }

    public bool IsPaused = Time.timeScale < 1;

    [Inject]
    private void Construct(DefensesModel defensesModel, GameGrid grid, InputManager inputManager, CastleDefense castle)
    {
        DefensesModel = defensesModel;
        Grid = grid;
        InputManager = inputManager;
        Castle = castle;
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
}
