using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class DefencesViewModel : MonoBehaviour
{
    private DefensesModel DefensesModel { get; set; }

    public event Action OnGoldChanged;

    [Inject]
    private void Construct(DefensesModel defensesModel, CurrencyModel currencyModel)
    {
        DefensesModel = defensesModel;
    }

    public List<ScriptableDefense> GetDefencesList() => DefensesModel.List;

    public event Action<ScriptableDefense> OnDefenseSelected;
    public void DefenseSelected(ScriptableDefense info)
    {
        OnDefenseSelected?.Invoke(info);
    }

    public event Action OnDefenseDeselected;
    public void DefenseDeselected() => OnDefenseDeselected?.Invoke();
}
