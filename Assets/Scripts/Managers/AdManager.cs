using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private InterstitialAd interstitialAd;

    public string myGameIdAndroid = "5145689";
    public string myGameIdIOS = "5145688";

    public string adUnitIdAndroid = "Interstitial_Android";
    public string adUnitIdIOS = "Interstitial_iOS";

    public string myAdUnitId;
    public bool adStarted;

    private bool testMode = true;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS
            Advertisement.Initialize(myGameIdIOS, testMode, this);
            myAdUnitId = adUnitIdIOS;
#else
        Advertisement.Initialize(myGameIdAndroid, testMode, this);
        myAdUnitId = adUnitIdAndroid;
#endif

    }

    // Update is called once per frame
    void Update()
    {
        if (Advertisement.isInitialized && !adStarted)
        {
            Advertisement.Load(myAdUnitId, this);
            Advertisement.Show(myAdUnitId, this);
            adStarted = true;
        }

    }

    public void OnInitializationComplete()
    {
        Debug.LogError("INIT COMPLETE");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError("INIT FAILED");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.LogError("AD LOADED");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError("AD FAILED TO LOAD");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError("FAILED TO SHOW AD");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.LogError("STARTED TO SHOW AD");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.LogError("CLICKED ON AD");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.LogError("COMPLETED SHOWING AD");
    }
}
