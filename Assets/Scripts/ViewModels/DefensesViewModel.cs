using System;
using Defendable;
using UnityEngine;
using Zenject;

public class DefensesViewModel : MonoBehaviour
{
    [SerializeField] private DefenseView prefab;
    [SerializeField] private Transform viewParent;
    private DefensesModel DefensesModel { get; set; }
    private LoseModel LoseModel { get; set; }
    private DiContainer Container { get; set; }

    public event Action OnDefenseSelected;

    [Inject]
    private void Construct(DefensesModel defensesModel, LoseModel loseModel, DiContainer container)
    {
        Container = container;
        DefensesModel = defensesModel;
        LoseModel = loseModel;
    }

    private void Start()
    {
        DefensesModel.OnSelectDefenseClick += DefenseSelected;
        LoseModel.OnRestart += DefenseSelected;

        DefenseViewFactory factory = Container.Resolve<DefenseViewFactory>();
        foreach (var defense in DefensesModel.List)
        {
            factory.CreateDefenseView(defense, prefab, viewParent);
        }
    }

    public void DefenseSelected() => OnDefenseSelected?.Invoke();

    public void DeselectDefense()
    {
        DefensesModel.DefenseDeselected();
    }
}
