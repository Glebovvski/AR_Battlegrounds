using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;

public class DefencesViewModel : MonoBehaviour
{
    [SerializeField] private DefencesModel defencesModel;

    public List<Defence> GetDefencesList() => defencesModel.List;

    public event Action<Defence> OnDefenseSelected;
    public void DefenseSelected(Defence defence) => OnDefenseSelected?.Invoke(defence);

}
