using System;
using System.Diagnostics;

public static class IADDelegate
{
    public static void ShowBanner()
    {
        _delegate.ShowBanner();
    }

    public static void HideBanner()
    {
        _delegate.HideBanner();
    }

    public static bool IsBannerShowing()
    {
        return _delegate.IsBannerShowing();
    }

    public static void ShowInterstitialAds(string pos, Action<bool> callBack = null)
    {
        _delegate.ShowInterstitialAds(pos, callBack);
    }

    public static void ShowRewardedAd(string pos, Action<bool> callBack = null)
    {
        _delegate.ShowRewardedAd(pos, callBack);
    }

    public static bool IsInterstitialReady()
    {
        return _delegate.IsInterstitialReady();
    }

    public static bool IsRewardedAdReady()
    {
        return _delegate.IsRewardedAdReady();
    }

    public static void ShowMRec()
    {
        _delegate.ShowMRec();
    }

    public static void HideMRec()
    {
        _delegate.HideMRec();
    }

    public static bool IsMRecShowing()
    {
        return _delegate.IsMRecShowing();
    }

    static IADDelegate()
    {
#if UNITY_EDITOR
        _delegate = new EditorADDelegate();
#else
        _delegate = new EditorADDelegate();
#endif
    }

    private static IAD _delegate;
}