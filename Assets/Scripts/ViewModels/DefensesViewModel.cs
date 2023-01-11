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

    DefenseViewFactory factory;

    [Inject]
    private void Construct(DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
    }

    private void Start()
    {
        factory = new DefenseViewFactory();
        foreach (var defence in DefensesModel.List)
        {
            factory.CreateDefenseView(defence, prefab, viewParent, DefensesModel);
        }
    }
}
