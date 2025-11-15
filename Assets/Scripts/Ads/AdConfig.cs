using UnityEngine;

public static class AdConfig
{
    private enum PlatformType { NONE, Android, IOS }
    private static string appKeyAndroid = "ca-app-pub-9927260088180763~1380226979";
    private static string appKeyIOS = "none";
    private static string bannerAdIdAndroid = "ca-app-pub-9927260088180763/7754349043";
    private static string bannerAdIdAndroidTest = "ca-app-pub-3940256099942544/6300978111";
    private static string bannerAdIdIOS = "none";
    private static string bannerAdIdIOSTest = "ca-app-pub-3940256099942544/2934735716";
    private static string interstitialAdIdAndroid = "none";
    private static string interstitialAdIdAndroidTest = "ca-app-pub-3940256099942544/1033173712";
    private static string interstitialAdIdIOS = "none";
    private static string interstitialAdIdIOSTest = "ca-app-pub-3940256099942544/4411468910";
    private static string rewardedVideoAdIdAndroid = "ca-app-pub-9927260088180763/9319013474";
    private static string rewardedVideoAdIdAndroidTest = "ca-app-pub-3940256099942544/5354046379";
    private static string rewardedVideoAdIdIOS = "none";
    private static string rewardedVideoAdIdIOSTest = "ca-app-pub-3940256099942544/6978759866";
    private static PlatformType runtimePlatform = PlatformType.NONE;
    private static bool platformDefined = false;

    public static string GetAppKey()
    {
        if (!platformDefined) DefinePlatform();
        if (runtimePlatform == PlatformType.Android)
            return appKeyAndroid;
        else if (runtimePlatform == PlatformType.IOS)
            return appKeyIOS;
        else return "none";
    }

    public static string GetBannerAdId(bool testmode = true)
    {
        if (!platformDefined) DefinePlatform();
        if (runtimePlatform == PlatformType.Android)
            return (testmode) ? bannerAdIdAndroidTest : bannerAdIdAndroid;
        else if (runtimePlatform == PlatformType.IOS)
            return (testmode) ? bannerAdIdIOSTest: bannerAdIdIOS;
        else return "none";
    }

    public static string GetInterstitialAdId(bool testmode = true)
    {
        if (!platformDefined) DefinePlatform();
        if (runtimePlatform == PlatformType.Android)
            return (testmode) ? interstitialAdIdAndroidTest: interstitialAdIdAndroid;
        else if (runtimePlatform == PlatformType.IOS)
            return interstitialAdIdIOS;
        else return "none";
    }

    public static string GetRewardedVideoAdId(bool testmode = true)
    {
        if (!platformDefined) DefinePlatform();
        if (runtimePlatform == PlatformType.Android)
            return (testmode) ? rewardedVideoAdIdAndroidTest: rewardedVideoAdIdAndroid;
        else if (runtimePlatform == PlatformType.IOS)
            return (testmode) ? rewardedVideoAdIdIOSTest: rewardedVideoAdIdIOS;
        else return "none";
    }

    private static void DefinePlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                runtimePlatform = PlatformType.Android;
                break;
            case RuntimePlatform.IPhonePlayer:
                runtimePlatform = PlatformType.IOS;
                break;
            case RuntimePlatform.WindowsEditor:
                runtimePlatform = PlatformType.Android;
                break;
            case RuntimePlatform.OSXEditor:
                runtimePlatform = PlatformType.IOS;
                break;
            default:
                runtimePlatform = PlatformType.NONE;
                break;
        }
        platformDefined = true;
    }
}