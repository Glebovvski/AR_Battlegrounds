using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.UI;
using System;

public class WinView : MonoBehaviour
{
    private WinViewModel WinViewModel { get; set; }

    [SerializeField] private GameObject WinPanel;

    [Space(10)]
    [SerializeField] private StarView starCentre;
    [SerializeField] private StarView starLeft;
    [SerializeField] private StarView starRight;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI bestScore;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI enemiesDestroyed;
    [SerializeField] private TextMeshProUGUI defensesDestroyed;

    [Inject]
    private void Construct(WinViewModel winViewModel)
    {
        WinViewModel = winViewModel;
    }

    private void Start()
    {
        WinViewModel.OnShowWinScreen += ShowWinScreen;
    }

    private void ShowWinScreen(int stars)
    {
        WinPanel.SetActive(true);
        UpdateStars(stars);
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

    private void OnDestroy()
    {
        WinViewModel.OnShowWinScreen -= ShowWinScreen;
    }
}
