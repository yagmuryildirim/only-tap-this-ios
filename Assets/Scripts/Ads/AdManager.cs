using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using TMPro;

public class AdManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private GameManager gameManager;
    private int rewardedAdCount;
    private RewardedAd rewardedAd;
    private BannerView bannerView;
    private const int maxRewardedAdCount = 3;
    private const string firstTime = "only-tap-this-first-time";

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }
        rewardedAdCount = 0;
        //TestDeviceID();
        MobileAds.Initialize(initStatus => { });
        this.CreateAndLoadRewardedAd();
    }

    public void CheckAdCount()
    {
        DateTime dateTime = DateTime.UtcNow.Date;
        if (PlayerPrefs.GetInt(firstTime, 1) == 1)
        {
            PlayerPrefs.SetInt(firstTime, 0);
            ResetPlayerPrefs(dateTime);
        }
        else
        {
            //Not first time
            if (PlayerPrefs.GetString("date") == dateTime.ToString())
            {
                //Same day
                //Check ad count
                rewardedAdCount = PlayerPrefs.GetInt("ads");
            }
            else
            {
                //New day
                ResetPlayerPrefs(dateTime);
            }
        }
        ToggleContinueButton();
    }

    public void UserChoseToWatchAd()
    {
        if (this.rewardedAd.IsLoaded() && rewardedAdCount < maxRewardedAdCount)
        {
            rewardedAdCount++;
            PlayerPrefs.SetInt("ads", rewardedAdCount);
            this.rewardedAd.Show();
        }
    }

    public void CreateAndLoadRewardedAd()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }
#if UNITY_IPHONE
        // TEST
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
        // REAL
#else
        string adUnitId = "unexpected_platform";
#endif

        if (this.rewardedAd != null)
        {
            this.rewardedAd.Destroy();
        }

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void RequestBanner()
    {
#if UNITY_IPHONE
        //TEST
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        //REAL
#else
            string adUnitId = "unexpected_platform";
#endif

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    public void DestroyBanner()
    {
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
            this.bannerView = null;
        }
    }

    private void ToggleContinueButton()
    {
        if (this.rewardedAd.IsLoaded() && rewardedAdCount < maxRewardedAdCount)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
    private void ResetPlayerPrefs(DateTime dateTime)
    {
        PlayerPrefs.SetString("date", dateTime.ToString());
        PlayerPrefs.SetInt("ads", 0);
    }

    private void TestDeviceID()
    {
        List<string> deviceIds = new List<string>();
        deviceIds.Add("8db02c13c2c671084e878fe7f4488a43");
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        gameManager.ContinueGame();
    }


}
