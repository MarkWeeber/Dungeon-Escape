using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidInterstitialAdId = "Interstitial_Android";
    [SerializeField] string _iOSInterstitialAdId = "Interstitial_iOS";
    [SerializeField] string _androidRewardedAdId = "Rewarded_Android";
    [SerializeField] string _iOSRewardedAdID = "Rewarded_iOS";
    private string _interstitialAdId;
    private string _rewardedAdId;
    private string _currentAdId;

    private void Start()
    {
        _showAdButton.interactable = false;
        // correctly set game and ad Ids
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                _interstitialAdId = _androidInterstitialAdId;
                _rewardedAdId = _androidRewardedAdId;
                break;
            case RuntimePlatform.IPhonePlayer:
                _interstitialAdId = _iOSInterstitialAdId;
                _rewardedAdId = _iOSRewardedAdID;
                break;
            default:
                _interstitialAdId = _androidInterstitialAdId;
                _rewardedAdId = _androidRewardedAdId;
                break;
        }
    }

    public void ShowInterstitialAd()
    {
        _currentAdId = _interstitialAdId;
        LoadAd();
        ShowAd();
    }

    //public void ShowRewardedAd()
    //{
    //    _currentAdId = _rewardedAdId;
    //    LoadAd();
    //    ShowAd();
    //}

    public void LoadRewardedAd()
    {
        _currentAdId = _rewardedAdId;
        LoadAd();
    }

    public void ShowRewardedAd()
    {
        _showAdButton.interactable = false;
        _currentAdId = _rewardedAdId;
        ShowAd();
    }

    private void LoadAd()
    {
        Advertisement.Load(_currentAdId, this);
    }

    private void ShowAd()
    {
        Advertisement.Show(_currentAdId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded");
        if (placementId.Equals(_rewardedAdId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowRewardedAd);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("OnUnityAdsShowStart");
    }
    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("OnUnityAdsShowClick");
    }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("OnUnityAdsShowComplete");
        // track only for rewarded ad
        if (placementId.Equals(_rewardedAdId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            LogUI.Instance.SendLogInformation("Weldone, here's your reward!", LogUI.MessageType.SUCCESS);
        }
    }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}
