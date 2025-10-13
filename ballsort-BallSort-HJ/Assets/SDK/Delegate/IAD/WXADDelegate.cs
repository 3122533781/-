using System;

public class WXADDelegate : IAD
{
    public bool IsAllADRemoved()
    {
        return false;
        //return App.Instance.Model.isADRemoved;
    }

    public void ShowBanner()
    {
       // WXSDKManager.Instance.ShowBanner();
    }

    public void HideBanner()
    {
        // WXSDKManager.Instance.HideBanner();
    }

    public bool IsBannerShowing()
    {
        return false;
    }

    public void ShowInterstitialAds(string pos, Action<bool> callBack = null)
    {
      //  WXSDKManager.Instance.ShowInterstitial(callBack);
    }

    public void ShowRewardedAd(string pos, Action<bool> callBack = null)
    {
       // WXSDKManager.Instance.ShowRewarded(callBack);
    }

    public bool IsInterstitialReady()
    {
        return true;
       // return WXSDKManager.Instance.IsInterstitialReady();
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
    public void ShowDebugger()
    {
    }

    public Boolean IsMRecShowing()
    {
        return false;
    }
}
