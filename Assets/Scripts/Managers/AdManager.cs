using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

public class AdManager : IInitializable
{
    private const string noAdsKey = "NoAds"; // 0 = show ads; 1 = no ads

    private GameControlModel GameControlModel { get; set; }

    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    public event Action OnCanShowAdValueChanged;
    private bool canShowAd;
    private bool CanShowAd
    {
        get => canShowAd;
        set
        {
            canShowAd = PlayerPrefs.GetInt(noAdsKey, 0) < 1;
            OnCanShowAdValueChanged?.Invoke();
        }
    }
    [Inject]
    private void Construct(GameControlModel gameControlModel)
    {
        GameControlModel = gameControlModel;
    }

    public void Initialize()
    {
        OnCanShowAdValueChanged += HideBanner;
        if (!CanShowAd) return;

        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();
        RequestBanner();
        GameControlModel.OnRestart += ShowInterstitialAd;
    }



    private void ShowInterstitialAd()
    {
        if (interstitialAd.CanShowAd() && CanShowAd)
            interstitialAd.Show();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
// #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        bannerView.Show();
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        AdRequest request = new AdRequest.Builder().Build();
        InterstitialAd.Load(adUnitId, request, InterstitialAdCallback);
    }

    private void InterstitialAdCallback(InterstitialAd ad, LoadAdError error)
    {
        interstitialAd = ad;
        interstitialAd.OnAdFullScreenContentClosed += RequestInterstitial;
        interstitialAd.OnAdFullScreenContentClosed += HandleInterstitialClosed;
    }

    public event Action OnInterstitialAdClosed;
    private void HandleInterstitialClosed()
    {
        interstitialAd.OnAdFullScreenContentClosed -= RequestInterstitial;
        interstitialAd.OnAdFullScreenContentClosed -= HandleInterstitialClosed;

        OnInterstitialAdClosed?.Invoke();
    }

    private void HideBanner()
    {
        if (bannerView == null) return;
        
        bannerView.Hide();
        bannerView.Destroy();
    }
}
