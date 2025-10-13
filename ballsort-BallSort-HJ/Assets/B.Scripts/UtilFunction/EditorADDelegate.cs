using System;
using UnityEngine;

public class EditorADDelegate : IAD
{
    public void ShowBanner()
    {
        EditorADManager.Instance.ShowBanner();
        _isBannerShow = true;
    }

    public void HideBanner()
    {
        EditorADManager.Instance.HideBanner();
        _isBannerShow = false;
    }

    public bool IsBannerShowing()
    {
        return _isBannerShow;
    }

    public void ShowInterstitialAds(string pos, Action<bool> callBack = null)
    {
        EditorADManager.Instance.ShowInterstitial(() => { callBack?.Invoke(true); });
    }

    public void ShowRewardedAd(string pos, Action<bool> callBack = null)
    {
        EditorADManager.Instance.ShowRewarded((isSuccess) => { callBack?.Invoke(isSuccess); });
    }

    public bool IsInterstitialReady()
    {
        return true;
    }

    public bool IsRewardedAdReady()
    {
        return true;
    }

    public void ShowMRec()
    {
        _isMRecShow = true;
        Debug.Log("Show MRec");
    }

    public void HideMRec()
    {
        _isMRecShow = false;
        Debug.Log("Hide MRec");
    }

    public bool IsMRecShowing()
    {
        return _isMRecShow;
    }

    private bool _isBannerShow = false;
    private bool _isMRecShow = false;
}