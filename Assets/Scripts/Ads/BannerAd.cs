using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;
using static GoogleAdsManager;

public class BannerAd : IDisposable
{
    public Action OnBannerAdLoadSuccess;
    public Action<string> OnBannerLoadFailed;
    public Action OnBannerAdOpening;
    public Action OnBannerAdClosed;
    public Action OnBannerAdPaid;
    public Action OnBannerAdImpression;
    private BannerView _bannerView;
    private AdSize _adSize;
    private AdRequest _adRequest;

    public void Create(string bannerAdUnitId, AdPosition adPosition, BannerSizeOption bannerSizeOption)
    {
        Debug.Log("Creating Banner");
        _adSize = GetAdsize(bannerSizeOption);
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
        _bannerView = new BannerView(bannerAdUnitId, _adSize, adPosition);
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded successfully!");
            MobileAdsEventExecutor.ExecuteInUpdate(() => { OnBannerAdLoadSuccess?.Invoke(); });
        };
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            string errorMessage = GetDetailedErrorMessage(error);
            Debug.LogError($"Banner ad failed to load: {errorMessage}");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                OnBannerLoadFailed?.Invoke(errorMessage);
            });
        };
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner ad clicked and opened full screen content");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                OnBannerAdOpening?.Invoke();
            });
        };
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner ad full screen content closed");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                OnBannerAdClosed?.Invoke();
            });
        };
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner ad impression recorded");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                OnBannerAdImpression?.Invoke();
            });
        };
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            string revenueInfo = $"Value: {adValue.Value} {adValue.CurrencyCode}, Precision: {adValue.Precision}";
            Debug.Log($"Banner ad paid event: {revenueInfo}");

            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                OnBannerAdPaid?.Invoke();

                // Here you can log revenue to your analytics
                LogAdRevenue(adValue);
            });
        };
    }

    public void LoadBannerAd()
    {
        if (_bannerView == null)
        {
            Debug.LogError("BannerView is null. Create banner first.");
            return;
        }
        Debug.Log("Loading banner ad");
        // Create ad request
        if(_adRequest == null) _adRequest = new AdRequest();
        // Load the banner with the request
        _bannerView.LoadAd(_adRequest);
    }

    public void ShowBannerAd()
    {
        if (_bannerView == null)
        {
            Debug.LogWarning("BannerView is null");
            return;
        }
        Debug.Log("Showing banner ad...");
        _bannerView.Show();
    }

    public void HideBannerAd()
    {
        if (_bannerView == null)
        {
            Debug.LogWarning("Banner ad is not currently visible.");
            return;
        }
        Debug.Log("Hiding banner ad");
        _bannerView.Hide();
    }

    private void LogAdRevenue(AdValue adValue)
    {
        // Implement your revenue logging here
        // Example: Log to analytics service
        Debug.Log($"Ad Revenue - Value: {adValue.Value}, Currency: {adValue.CurrencyCode}, Precision: {adValue.Precision}");
    }

    public void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    private AdSize GetAdsize(BannerSizeOption bannerSizeOptions)
    {
        switch (bannerSizeOptions)
        {
            case BannerSizeOption.Banner:
                return AdSize.Banner;
            case BannerSizeOption.MediumRectangle:
                return AdSize.MediumRectangle;
            case BannerSizeOption.IABBanner:
                return AdSize.IABBanner;
            case BannerSizeOption.Leaderboard:
                return AdSize.Leaderboard;
            case BannerSizeOption.Adaptive:
                return AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            default:
                return null;
        }
        ;
    }

    private string GetDetailedErrorMessage(LoadAdError error)
    {
        if (error == null) return "Unknown error";

        return $"Code: {error.GetCode()}, Message: {error.GetMessage()}, Domain: {error.GetDomain()}, Cause: {error.GetCause()}";
    }

    public void Dispose()
    {
        DestroyBannerAd();
        OnBannerAdLoadSuccess.ClearInvocations();
        OnBannerLoadFailed.ClearInvocations();
        OnBannerAdOpening.ClearInvocations();
        OnBannerAdClosed.ClearInvocations();
        OnBannerAdPaid.ClearInvocations();
        OnBannerAdImpression.ClearInvocations();
        OnBannerAdImpression.ClearInvocations();
    }
}