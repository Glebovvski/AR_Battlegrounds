using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class DefencesViewModel : MonoBehaviour
{
    private DefensesModel DefensesModel { get; set; }

    [Inject]
    private void Construct(DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
    }

    public List<ScriptableDefence> GetDefencesList() => DefensesModel.List;

    public event Action<PoolObjectType> OnDefenseSelected;
    public void DefenseSelected(PoolObjectType type) => OnDefenseSelected?.Invoke(type);

    public event Action OnDefenseDeselected;
    public void DefenseDeselected() => OnDefenseDeselected?.Invoke();

}
