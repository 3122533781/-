using System;
using System.Diagnostics;


public class MAXADDelegate : IAD
{
    public void ShowBanner()
    {
    }

    public void HideBanner()
    {
    }

    public bool IsBannerShowing()
    {
        return false;
    }

    public void ShowInterstitialAds(string pos, Action<bool> callBack = null)
    {
        UnityEngine.Debug.Log("插屏广告接口已经调用");

    }

    public void ShowRewardedAd(string pos, Action<bool> callBack = null)
    {
       



    }

    public bool IsInterstitialReady()
    {
        return false;
    }

    public bool IsRewardedAdReady()
    {
        return false;
    }

    public void ShowMRec()
    {
    }

    public void HideMRec()
    {
    }

    public bool IsMRecShowing()
    {
        return false;
    }
}