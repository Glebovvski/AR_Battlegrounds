using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

public class AdManager : IInitializable, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private GameControlModel GameControlModel { get; set; }


    BannerPosition _bannerPosition = BannerPosition.TOP_CENTER;

    private string myGameIdAndroid = "5145689";
    private string myGameIdIOS = "5145688";

    private string interstitial_ANDROID = "Interstitial_Android";
    private string interstitial_IOS = "Interstitial_iOS";

    private string banner_ANDROID = "Banner_Android";
    private string banner_IOS = "Banner_IOS";

    private string interstitial;
    private string banner;

    private bool testMode = true;

    [Inject]
    private void Construct(GameControlModel gameControlModel)
    {
        GameControlModel = gameControlModel;
    }

    public void Initialize()
    {
#if UNITY_IOS
            Advertisement.Initialize(myGameIdIOS, testMode, this);
            myAdUnitId = adUnitIdIOS;
            banner = banner_ANDROID;
#else
        Advertisement.Initialize(myGameIdAndroid, testMode, this);
        interstitial = interstitial_ANDROID;
        banner = banner_ANDROID;
#endif

        LoadAndShowBanner();
        GameControlModel.OnRestart += ShowInterstitialAd;
    }

    private void ShowInterstitialAd()
    {
        if (Advertisement.isInitialized)
        {
            Advertisement.Show(interstitial, this);
        }
    }

    private void LoadAndShowBanner()
    {
        Advertisement.Banner.SetPosition(_bannerPosition);
        Advertisement.Banner.Load(banner);
        Advertisement.Banner.Show(banner);
    }

    public void OnInitializationComplete()
    {
        // Debug.LogError("INIT COMPLETE");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        // Debug.LogError("INIT FAILED");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        // Debug.LogError("AD LOADED");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        // Debug.LogError("AD FAILED TO LOAD");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        // Debug.LogError("FAILED TO SHOW AD");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        // Debug.LogError("STARTED TO SHOW AD");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        // Debug.LogError("CLICKED ON AD");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        // Debug.LogError("COMPLETED SHOWING AD");
        Advertisement.Load(interstitial, this);
    }
}
