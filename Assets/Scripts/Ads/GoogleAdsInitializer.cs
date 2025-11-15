using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;

public class GoogleAdsInitializer : IDisposable
{
    public Action OnInitSuccess;
    public Action OnInitError;

    public void InitializeGoogleAds()
    {
        MobileAds.Initialize(InitializeGoogleMobileAds);
    }

    private void InitializeGoogleMobileAds(InitializationStatus initStatus)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (initStatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                OnInitializationFailed("Null Status");
                return;
            }
            var adapterStatusMap = initStatus.getAdapterStatusMap();
            bool allReady = true;
            Debug.Log("=== AdMob Initialization Status ===");
            foreach (var adapterStatus in adapterStatusMap)
            {
                string adapterName = adapterStatus.Key;
                AdapterStatus status = adapterStatus.Value;

                string stateMessage = $"Adapter: {adapterName} - State: {status.InitializationState}";

                if (status.InitializationState == AdapterState.Ready)
                {
                    Debug.Log(stateMessage);
                }
                else
                {
                    Debug.LogWarning($"{stateMessage} - Reason: {status.Description}");
                    allReady = false;
                }
            }
            if (allReady)
            {
                Debug.Log("Google Mobile Ads initialized successfully!");
                OnInitializationSuccess();
            }
            else
            {
                Debug.LogWarning("Google Mobile Ads initialized with warnings");
                OnInitializationPartialSuccess();
            }
        });

        if (initStatus == null)
        {
            Debug.LogError("Google Mobile Ads initialization failed.");
            return;
        }
        Debug.Log("Google Mobile Ads initialization complete.");
    }

    private void OnInitializationSuccess()
    {
        Debug.Log("Initialization Success");
        OnInitSuccess?.Invoke();
    }

    private void OnInitializationPartialSuccess()
    {
        Debug.Log("Initialization Partial Success");
        OnInitSuccess?.Invoke();
    }

    private void OnInitializationFailed(string error)
    {
        Debug.LogError($"AdMob initialization completely failed: {error}");
        OnInitError?.Invoke();
    }

    public void Dispose()
    {
        OnInitSuccess.ClearInvocations();
        OnInitError.ClearInvocations();
    }
}
