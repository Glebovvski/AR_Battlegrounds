using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class DefensesView : MonoBehaviour
{
    [SerializeField] private DefensesViewModel DefensesViewModel;

    public void CancelSelection()
    {
        DefensesViewModel.DeselectDefense();
    }
}
