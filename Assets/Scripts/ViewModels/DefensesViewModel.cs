using System;
using Defendable;
using UnityEngine;
using Zenject;

public class DefensesViewModel : MonoBehaviour
{
    [SerializeField] private DefenseView prefab;
    [SerializeField] private Transform viewParent;
    private DefensesModel DefensesModel { get; set; }
    private GameControlModel GameModel { get; set; }
    private DiContainer Container { get; set; }

    public event Action OnDefenseSelected;

    [Inject]
    private void Construct(DefensesModel defensesModel, GameControlModel gameControlModel, DiContainer container)
    {
        Container = container;
        DefensesModel = defensesModel;
        GameModel = gameControlModel;
    }

    private void Start()
    {
        DefensesModel.OnSelectDefenseClick += DefenseSelected;
        GameModel.OnRestart += DefenseSelected;

        DefenseViewFactory factory = Container.Resolve<DefenseViewFactory>();
        foreach (var defense in DefensesModel.List)
        {
            factory.CreateDefenseView(defense, prefab, viewParent);
        }
    }

    public event Action OnClose;
    public void Close() => OnClose?.Invoke();

    public event Action OnOpen;
    public void Open() => OnOpen?.Invoke();

    public void DefenseSelected() => OnDefenseSelected?.Invoke();

    public void DeselectDefense()
    {
        DefensesModel.DefenseDeselected();
    }
}
