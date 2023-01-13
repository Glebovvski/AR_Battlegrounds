using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class DefensesViewModel : MonoBehaviour
{
    [SerializeField] private DefenseView prefab;
    [SerializeField] private Transform viewParent;
    private DefensesModel DefensesModel { get; set; }
    private DiContainer Container { get; set; }

    public event Action OnDefenseSelected;

    [Inject]
    private void Construct(DefensesModel defensesModel, DiContainer container)
    {
        Container = container;
        DefensesModel = defensesModel;
    }

    private void Start()
    {
        DefensesModel.OnSelectDefenseClick += DefenseSelected;
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
