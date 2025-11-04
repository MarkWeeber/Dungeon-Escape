using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private bool _testMode = true;
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    private string _gameId;

    private void Start()
    {
        // correctly set game and ad Ids
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                _gameId = _androidGameId;
                break;
            case RuntimePlatform.IPhonePlayer:
                _gameId = _iOSGameId;
                break;
            default:
                _gameId = _androidGameId;
                break;
        }
        // initialize advertisement
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
