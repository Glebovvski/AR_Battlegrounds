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

    [Inject]
    private void Construct(DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
    }

    private void Start()
    {
        foreach (var defense in DefensesModel.List)
        {
            DefenseViewFactory.CreateDefenseView(defense, prefab, viewParent, DefensesModel);
        }
    }
}
