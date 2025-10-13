using System;

public interface IAD
{
    void ShowBanner();
    void HideBanner();
    bool IsBannerShowing();
    void ShowInterstitialAds(string pos, Action<bool> callBack = null);
    void ShowRewardedAd(string pos, Action<bool> callBack = null);

    bool IsInterstitialReady();
    bool IsRewardedAdReady();

    void ShowMRec();
    void HideMRec();
    bool IsMRecShowing();
}