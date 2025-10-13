using System;

public class NullableADDelegate : IAD
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
        callBack?.Invoke(true);
    }

    public void ShowRewardedAd(string pos, Action<bool> callBack = null)
    {
        callBack?.Invoke(true);
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
        
    }

    public void HideMRec()
    {
    }

    public bool IsMRecShowing()
    {
        return false;
    }
}
