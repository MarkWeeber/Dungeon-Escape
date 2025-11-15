using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class RewardedVideoAd : IDisposable
{
    public Action OnAdLoadSuccess;
    public Action OnAdLoadFail;
    public Action<int> OnAdFullyShown;

    private RewardedAd _rewardedAd;
    private AdRequest _adRequest;

    public void LoadRewardedAd(string adUnitId)
    {
        if (_adRequest == null) _adRequest = new AdRequest();
        if (_rewardedAd != null) _rewardedAd.Destroy();
        Debug.Log("Loading Rewarded video ad");
        RewardedAd.Load(adUnitId, _adRequest, (RewardedAd rewardedAd, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.Log("Rewarded video add loaded error: " + error.GetMessage());
                OnAdLoadFail?.Invoke();
                return;
            }
            _rewardedAd = rewardedAd;
            AssignCallbacks();
            OnAdLoadSuccess?.Invoke();
            Debug.Log("Rewarded video add loaded successfully");
        });
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("The reward video ad fully shown, player receives: " + reward.Amount + " of " + reward.Type + " !");
                OnAdFullyShown?.Invoke((int)reward.Amount);
            });
        }
    }

    private void AssignCallbacks()
    {
        _rewardedAd.OnAdPaid += (AdValue adValue) =>
        {
            // Raised when the ad is estimated to have earned money.
        };
        _rewardedAd.OnAdImpressionRecorded += () =>
        {
            // Raised when an impression is recorded for an ad.
        };
        _rewardedAd.OnAdClicked += () =>
        {
            // Raised when a click is recorded for an ad.
        };
        _rewardedAd.OnAdFullScreenContentOpened += () =>
        {
            // Raised when the ad opened full screen content.
        };
        _rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            // Raised when the ad closed full screen content.
        };
        _rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // Raised when the ad failed to open full screen content.
        };
    }

    public void Dispose()
    {
        if(_rewardedAd != null) _rewardedAd.Destroy();
        OnAdLoadSuccess.ClearInvocations();
        OnAdFullyShown.ClearInvocations();
        OnAdLoadFail.ClearInvocations();
    }
}