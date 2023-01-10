using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;

public class DefencesViewModel : MonoBehaviour
{
    [SerializeField] private DefencesModel defencesModel;

    public List<ScriptableDefence> GetDefencesList() => defencesModel.List;

    public event Action<PoolObjectType> OnDefenseSelected;
    public void DefenseSelected(PoolObjectType type) => OnDefenseSelected?.Invoke(type);

    public event Action OnDefenseDeselected;
    public void DefenseDeselected() => OnDefenseDeselected?.Invoke();

}
