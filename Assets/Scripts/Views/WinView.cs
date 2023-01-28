using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.UI;

public class WinView : MonoBehaviour
{
    private WinViewModel WinViewModel { get; set; }

    [SerializeField] private StarView starCentre;
    [SerializeField] private StarView starLeft;
    [SerializeField] private StarView starRight;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI timer;

    [Inject]
    private void Construct(WinViewModel winViewModel)
    {
        WinViewModel = winViewModel;
    }

    public void UpdateStars(int stars)
    {
        Reset();
        starCentre.Activate();
        if (stars > 1)
            starLeft.Activate();
        if (stars == 3)
            starRight.Activate();
    }

    public void Reset()
    {
        starCentre.Reset();
        starLeft.Reset();
        starRight.Reset();
    }

    // private void Start()
    // {
    //     WinViewModel.OnTimerChange += UpdateData;
    // }

    // private void UpdateData(float data)
    // {
    //     timer.text = data.ToString();
    // }
}
