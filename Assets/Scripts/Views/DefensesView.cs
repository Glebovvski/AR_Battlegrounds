using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class DefensesView : MonoBehaviour
{
    private DefensesModel DefensesModel { get; set; }

    [Inject]
    private void Construct(DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
    }

    public void CancelSelection()
    {
        DefensesModel.DefenseDeselected();
    }
}
