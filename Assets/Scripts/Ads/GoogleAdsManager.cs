using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoogleAdsManager : SingletonBehaviour<GoogleAdsManager>
{
    public enum BannerSizeOption { Banner, MediumRectangle, IABBanner, Leaderboard, Adaptive }
    [SerializeField] private bool _testingMode = true;
    [Header("Banner Ad settings")]
    [SerializeField] private AdPosition _adPosition = AdPosition.Bottom;
    [SerializeField] private BannerSizeOption _bannerSizeOption = BannerSizeOption.Adaptive;

    private GoogleAdsInitializer _mInitializer;
    private BannerAd _bannerAd;
    private RewardedVideoAd _rewardedVideoAd;
    private string _bannerAdUnitId;
    private string _rewardedAdUnitId;
    private Button _showRewardedAdButton;

    protected override void Initialize()
    {
        dontDestroyOnload = true;
        GetAdUnitIds();
        InitializeAdsAndLoadBanner();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void GetAdUnitIds()
    {
        _bannerAdUnitId = AdConfig.GetBannerAdId(_testingMode);
        _rewardedAdUnitId = AdConfig.GetRewardedVideoAdId(_testingMode);
    }

    private void InitializeAdsAndLoadBanner()
    {
        if (_bannerAd == null) _bannerAd = new BannerAd();
        _mInitializer = new GoogleAdsInitializer();
        _mInitializer.OnInitSuccess += () => { CreateAndShowBannerAd(_bannerAdUnitId, _adPosition, _bannerSizeOption); };
        _mInitializer.InitializeGoogleAds();

    }

    private void CreateAndShowBannerAd(string bannerAdUnitId, AdPosition adPosition, BannerSizeOption bannerSizeOption)
    {
        _bannerAd.Create(bannerAdUnitId, adPosition, bannerSizeOption);
        _bannerAd.OnBannerAdLoadSuccess += () => { _bannerAd.ShowBannerAd(); };
        _bannerAd.LoadBannerAd();

    }

    public void AssignButtonToShowRewardedAd(Button button)
    {
        _showRewardedAdButton = button;
        _showRewardedAdButton.interactable = true;
        _showRewardedAdButton.onClick.AddListener(LoadAndShowRewardedVideoAd);
    }

    private void LoadAndShowRewardedVideoAd()
    {
        _showRewardedAdButton.interactable = false;
        if (_rewardedVideoAd == null) _rewardedVideoAd = new RewardedVideoAd();
        _rewardedVideoAd.OnAdLoadSuccess += () => { _rewardedVideoAd.ShowRewardedAd(); };
        _rewardedVideoAd.OnAdFullyShown += RewardedAdFullyShown;
        _rewardedVideoAd.LoadRewardedAd(_rewardedAdUnitId);
    }

    private void RewardedAdFullyShown(int rewardAmount)
    {
        LogUI.Instance.SendLogInformation("Weldone! Here's your Reward", LogUI.MessageType.SUCCESS);
        Player.Instance.Diamonds += rewardAmount;
        _showRewardedAdButton.interactable = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene was loaded");
        CreateAndShowBannerAd(_bannerAdUnitId, _adPosition, _bannerSizeOption);
    }
}
