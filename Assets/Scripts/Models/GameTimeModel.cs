using Defendable;
using UnityEngine;
using Zenject;

public class GameTimeModel : IInitializable
{
    private DefensesModel DefensesModel { get; set; }

    public bool IsPaused = Time.timeScale == 0;

    [Inject]
    private void Construct(DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Resume()
    {
        Time.timeScale = 1;
    }

    public void Initialize()
    {
        DefensesModel.OnDefenseDeselected += Resume;
        DefensesModel.OnSelectDefense += Pause;
    }
}
