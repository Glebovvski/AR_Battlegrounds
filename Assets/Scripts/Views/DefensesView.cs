using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class DefensesView : MonoBehaviour
{
    [SerializeField] private DefensesViewModel DefensesViewModel;
    [SerializeField] private GameObject cancelBtn;

    public void CancelSelection()
    {
        DefensesViewModel.DeselectDefense();
        ToggleCancelBtn(false);
    }

    public void ToggleCancelBtn(bool active) => cancelBtn.SetActive(active);
}
