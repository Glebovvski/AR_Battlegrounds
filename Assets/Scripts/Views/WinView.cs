using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class WinView : MonoBehaviour
{
    private WinViewModel WinViewModel { get; set; }

    [SerializeField] private TextMeshProUGUI timer;

    [Inject]
    private void Construct(WinViewModel winViewModel)
    {
        WinViewModel = winViewModel;
    }

    private void Start()
    {
        WinViewModel.OnTimerChange += UpdateData;
    }

    private void UpdateData(float data)
    {
        timer.text = data.ToString();
    }
}
