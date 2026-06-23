using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
	public static AdsManager Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	[Header("App key")]
	[SerializeField] private string androidAppKey;
	private string appKey
	{
		get
		{
#if UNITY_ANDROID
			return androidAppKey;
#else
	return string.Empty;
#endif
		}
	}

	[Header("Banner Ad Unit Id")]
	[SerializeField] private string androidBannerAdUnitId;
	private string bannerAdUnitId
	{
		get
		{
#if UNITY_ANDROID
			return androidBannerAdUnitId;
#else
	return string.Empty;
#endif
		}
	}

	[Header("Interstitial Ad Unit Id")]
	[SerializeField] private string androidInterstitialAdUnitId;
	private string interstitialAdUnitId
	{
		get
		{
#if UNITY_ANDROID
			return androidInterstitialAdUnitId;
#else
	return string.Empty;
#endif
		}
	}

	[Header("Reward Ad Unit Id")]
	[SerializeField] private string androidRewardAdUnitId;
	private string rewardAdUnitId
	{
		get
		{
#if UNITY_ANDROID
			return androidRewardAdUnitId;
#else
	return string.Empty;
#endif
		}
	}

	private void Start()
	{
		Debug.Log("LevelPlay Start");

		LevelPlay.ValidateIntegration();

		LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
		LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

		Debug.Log("Init with AppKey = " + appKey);

		LevelPlay.Init(appKey);
	}

	private void SdkInitializationCompletedEvent(LevelPlayConfiguration configuration)
	{
		Debug.Log("LevelPlay Init Successfully");

		//CreateBannerAd();
		//CreateInterstitialAds();
		CreateRewardedAd();
	}

	private void SdkInitializationFailedEvent(LevelPlayInitError error)
	{
		Debug.LogError("LevelPlay Init Failed: " + error);
	}


	private LevelPlayBannerAd bannerAd;
	#region Banner Ads
	private void CreateBannerAd()
	{
		var adConfig = new LevelPlayBannerAd.Config.Builder()
			.SetPosition(LevelPlayBannerPosition.BottomCenter)
			.Build();
		bannerAd = new LevelPlayBannerAd(bannerAdUnitId, adConfig);

		// Register to the events
		bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
		bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
		bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
		bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
		bannerAd.OnAdClicked += BannerOnAdClickedEvent;
		bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
		bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
		bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
	}
	public void ShowBanner()
	{
		bannerAd.LoadAd();
	}
	public void DestroyBanner()
	{
		bannerAd.DestroyAd();
	}

	// Implement the events
	void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
	void BannerOnAdLoadFailedEvent(LevelPlayAdError error) { }
	void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
	{
		Debug.Log("Click Banner Ad");
	}
	void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
	void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
	void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo) { }
	void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo) { }
	void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo) { }

	#endregion

	private LevelPlayInterstitialAd interstitialAd;
	#region InterstitialAds
	private void CreateInterstitialAds()
	{
		interstitialAd = new LevelPlayInterstitialAd(interstitialAdUnitId);

		// Register to interstitial events
		interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
		interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
		interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
		interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
		interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
		interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
		interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

		//LoadInterstitialAd();
	}

	public void LoadInterstitialAd()
	{
		interstitialAd.LoadAd();
	}
	public void ShowInterstitialAd()
	{
		if (interstitialAd.IsAdReady())
		{
			interstitialAd.ShowAd();
			Debug.Log("show InterstitialAds");
		}
	}
	// Implement the events
	void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
	void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
	{
		//LoadInterstitialAd();
	}
	void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
	void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
	void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
	void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
	{
		//LoadInterstitialAd();
	}
	void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
	#endregion

	private LevelPlayRewardedAd RewardedAd;
	private Action onRewardSuccess;
	#region Rewarded Ads
	private void CreateRewardedAd()
	{
		RewardedAd = new LevelPlayRewardedAd(rewardAdUnitId);

		// Register to Rewarded events
		RewardedAd.OnAdLoaded += RewardedOnAdLoadedEvent;
		RewardedAd.OnAdLoadFailed += RewardedOnAdLoadFailedEvent;
		RewardedAd.OnAdDisplayed += RewardedOnAdDisplayedEvent;
		RewardedAd.OnAdDisplayFailed += RewardedOnAdDisplayFailedEvent;
		RewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
		RewardedAd.OnAdClosed += RewardedOnAdClosedEvent;
		// Optional
		RewardedAd.OnAdClicked += RewardedOnAdClickedEvent;
		RewardedAd.OnAdInfoChanged += RewardedOnAdInfoChangedEvent;

		RewardedAd.LoadAd();
	}
	public void ShowRewardedAd(Action rewardCallback)
	{
		Debug.Log("Rewarded IsReady = " + RewardedAd.IsAdReady());

		if (RewardedAd.IsAdReady())
		{
			onRewardSuccess = rewardCallback;
			RewardedAd.ShowAd();
			Debug.Log("Show Rewarded");
		}
		else
		{
			Debug.LogWarning("Rewarded not ready");
		}
	}	


	void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo)
	{
		Debug.Log("Rewarded Loaded");
	}
	void RewardedOnAdLoadFailedEvent(LevelPlayAdError error)
	{
		Debug.LogError("Rewarded Load Failed: " + error);
	}
	void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
	void RewardedOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
	void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward)
	{
		string rewardName = adReward.Name;
		int rewardAmount = adReward.Amount;
		Debug.Log("Reward : " + rewardName + " Value " + rewardAmount);

		onRewardSuccess?.Invoke();
	}
	void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo) { }
	void RewardedOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
	void RewardedOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
	#endregion
}
