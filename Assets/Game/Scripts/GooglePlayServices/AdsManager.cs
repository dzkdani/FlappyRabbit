using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : CustomMonobehavior
{
    public enum BANNER_SIZE {BANNER, SMART_BANNER};
    private const string BANNER_ID = "ca-app-pub-7143337953873107/4053560034";
    private const string INTERSTITIAL_ID = "ca-app-pub-7143337953873107/2548906671";
    private const string REWARDED_INTERSTITIAL_ID = "ca-app-pub-7143337953873107/5422482028";
    private const string REWARDED_ID = "ca-app-pub-7143337953873107/4983498320";
    private const string NATIVE_ADVANCED_ID = "ca-app-pub-7143337953873107/2628280800";
    private const string APP_OPEN_ID = "ca-app-pub-7143337953873107/8731171648";

    private const string NO_FILL = "No fill";

    #region events
    public event EventHandler EVENT_HandleUserEarnedReward;
    #endregion

    //Ads Object
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    public bool RewardedVideoReady;

    [SerializeField]
    private AdPosition bannerPosition;
    [SerializeField]
    private BANNER_SIZE bannerSize;
    AdSize[] bannerSizArray = { AdSize.Banner, AdSize.SmartBanner};

    public RewardedAd RewardedAd { get => rewardedAd; set => rewardedAd = value; }

    public void AdsInit()
    {
        RewardedVideoReady = false;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        //RequestAds
        RequestBanner();
        RequestInterstitial();
        RequestRewardedVideo();
    }


    /// <summary>
    /// Banner ads are rectangular image or text ads that occupy a spot within an app's layout.
    /// hey stay on screen while users are interacting with the app
    /// and can refresh automatically after a certain period of time.
    /// </summary>
    private void RequestBanner()
    {
        //Test Id
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(BANNER_ID, bannerSizArray[((int)bannerSize)], bannerPosition);

        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoadedBanner;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoadBanner;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpenedBanner;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosedBanner;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    public void ShowBannerAds()
    {
        if (bannerView != null)
        {
            bannerView.Show();
        }
        else
        {
            Debug.Log("Banner not loaded");
        }
    }

    public void HideBannerAds()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
        else
        {
            Debug.Log("Banner not loaded");
        }
    }

    /// <summary>
    /// Interstitials are full-screen ads that cover the interface of an app until closed by the user.
    /// </summary>
    private void RequestInterstitial()
    {
        //TestID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(INTERSTITIAL_ID);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoadedinterstitial;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoadinterstitial;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpenedinterstitial;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosedinterstitial;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void ShowInterstitialAds()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("Interstitial not loaded");
        }
    }

    /// <summary>
    /// Rewarded Interstitial, user must wait the video until end.
    /// </summary>
    private void RequestRewardedVideo()
    {
        //TestID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";

        RewardedAd = new RewardedAd(REWARDED_ID);

        // Called when an ad request has successfully loaded.
        this.RewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.RewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.RewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.RewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.RewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.RewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    public void ShowRewardedAds()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.RewardedAd.LoadAd(request);
        if (RewardedAd.IsLoaded())
        {
            RewardedAd.Show();
            Debug.Log("Rewarded Video loaded");
        }
        else
        {
            Debug.Log("Rewarded Video not loaded");
        }
    }

    #region CallBack
    /// <summary>
    /// CallBack Banner
    /// </summary>
    private void HandleOnAdLoadedBanner(object sender, EventArgs e)
    {
        Debug.Log("HandleAdLoadedBanner event received");
    }

    private void HandleOnAdFailedToLoadBanner(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + e.Message);
    }

    private void HandleOnAdOpenedBanner(object sender, EventArgs e)
    {
        Debug.Log("HandleAdLoadedBanner event Opened");
    }

    private void HandleOnAdClosedBanner(object sender, EventArgs e)
    {
        Debug.Log("HandleAdLoadedBanner event Closed");
    }


    /// <summary>
    /// CallBack Interstital
    /// </summary>
    private void HandleOnAdLoadedinterstitial(object sender, EventArgs e)
    {
        Debug.Log("HandleAdLoadedinterstitial event received");
    }

    private void HandleOnAdFailedToLoadinterstitial(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + e.Message);
    }

    private void HandleOnAdOpenedinterstitial(object sender, EventArgs e)
    {
        Debug.Log("HandleAdLoadedinterstitial event Opened");
    }

    private void HandleOnAdClosedinterstitial(object sender, EventArgs e)
    {
        Debug.Log("HandleAdLoadedinterstitial event Closed");
    }


    /// <summary>
    /// Callback Rewarded
    /// </summary>

    private void HandleRewardedAdLoaded(object sender, EventArgs e)
    {
        Debug.Log("HandleRewardedAdLoaded event Opened");
    }
    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        Debug.Log("HandleRewardedAdFailedToLoad event Opened : " + e.Message);
        if (e.Message == NO_FILL)
        {
            RewardedVideoReady = false;
        }
        else
        {
            RewardedVideoReady = true;
        }
    }
    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        Debug.Log("HandleRewardedAdOpening event Opened");
    }
    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        Debug.Log("HandleRewardedAdFailedToShow event Opened");
    }
    private void HandleUserEarnedReward(object sender, Reward e)
    {
        Debug.Log("HandleUserEarnedReward");
    }
    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        Debug.Log("HandleUserCloseRewardAds");
        DispatchEvent(EVENT_HandleUserEarnedReward, this.gameObject, EventArgs.Empty);
    }

    #endregion callback
}
