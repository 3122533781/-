using System;
using System.Diagnostics;
using _02.Scripts.Config;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;

public class ADPosConst
{
    public const string ShopWatchAD = "ShopWatchAD";
    public const string WinTriple = "WinTriple";
    public const string GetManual = "GetManual";
    public const string StoreGetGold = "StoreGetGold";
    public const string Revive = "Revive";
    public const string SubLevelChange = "SubLevelChange";
    public const string DoubleChest = "DoubleChest";
    public const string MoneyWallet = "MoneyWallet";
    public const string ChallengePast = "ChallengePast";
    public const string GetCoinDialog = "GetCoinDialog";
}

public class ADMudule
{
    public static void ShowBanner()
    {
        if (!Game.Instance.Model.IsAdRemoved.Value)
            IADDelegate.ShowBanner();
    }

    public static void HideBanner()
    {
        IADDelegate.HideBanner();
    }

    public static bool IsBannerShowing()
    {
        return IADDelegate.IsBannerShowing();
    }

    public static void ShowMRec()
    {
        IADDelegate.ShowMRec();
    }

    public static void HideMRec()
    {
        IADDelegate.HideMRec();
    }

    public static bool IsMRecShowing()
    {
        return IADDelegate.IsMRecShowing();
    }

    public static void ShowInterstitialAds(string pos, Action<bool> onCompleted)
    {
        if (!Game.Instance.Model.IsAdRemoved.Value && Game.Instance.GetSystem<ADStrategySystem>().AccIntersistalTime > 61f && Game.Instance.Model.IsWangZhuan())
        {
            IADDelegate.ShowInterstitialAds(pos, (success) =>
            {
                Game.Instance.GameStatic.WatchADPlus();
                Game.Instance.GetSystem<ADStrategySystem>().ResetIntersistalTime();
                onCompleted?.Invoke(success);
            });
        }
        else
        {
            onCompleted?.Invoke(true);
        }
    }

    /// <summary>
    /// 3关后点击放弃也会弹出广告
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="onCompleted"></param>
    public static void ShowRewardAdByLevel(string pos, Action<bool> onCompleted)
    {
        if (!Game.Instance.Model.IsAdRemoved.Value && IsInterstitialReady()
            )
        {
            ShowRewardedAd(pos, onCompleted);
        }
        else
        {
            onCompleted?.Invoke(true);
        }
    }

    public static void ShowRewardedAd(string pos, Action<bool> onCompleted)
    {
        IADDelegate.ShowRewardedAd(pos, (success) =>
        {
            Game.Instance.GameStatic.WatchADPlus();
            Game.Instance.GetSystem<ADStrategySystem>().ResetIntersistalTime();
            onCompleted?.Invoke(success);
        });
    }

    public static bool IsInterstitialReady()
    {
        return IADDelegate.IsInterstitialReady();
    }

    public static bool IsRewardedAdReady()
    {
        return IADDelegate.IsRewardedAdReady();
    }
}