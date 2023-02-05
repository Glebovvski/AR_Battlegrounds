using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuView : MonoBehaviour
{
    private MenuViewModel MenuViewModel { get; set; }
    private AdManager AdManager { get; set; }

    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Button startBtn;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button donateBtn;
    [SerializeField] private Button noAdsBtn;

    [Inject]
    private void Construct(MenuViewModel menuViewModel, AdManager adManager)
    {
        MenuViewModel = menuViewModel;
        AdManager = adManager;
    }

    private void Start()
    {
        MenuViewModel.OnOpen += Show;
        AdManager.OnCanShowAdValueChanged+=SetNoAdsButtonVisibility;
    }

    public void Close()
    {
        menuPanel.SetActive(false);
        MenuViewModel.Close();
    }

    public void Show()
    {
        menuPanel.SetActive(true);
    }

    public void BuyCoins() => MenuViewModel.BuyCoins();
    public void Donation() => MenuViewModel.Donation();
    public void NoAds()
    {
        MenuViewModel.NoAds();
    }

    private void SetNoAdsButtonVisibility()
    {
        noAdsBtn.gameObject.SetActive(AdManager.CanShowAd);
    }

    private void OnDestroy()
    {
        MenuViewModel.OnOpen -= Show;
    }
}
