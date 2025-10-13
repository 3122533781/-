using Models;
using System;
using System.Text;
using UnityEngine;

public class StaticModule
{
    #region GameActiveTime

    /// <summary>
    /// 1分钟一次
    /// </summary>
    public static void GameHeartBeat()
    {
        IStaticDelegate.LogEvent("GameHeartBeat");
    }

    #endregion GameActiveTime

    #region Currency

    public static void ConsumeManual(int count)
    {
        string key = $"Consume_Manual";
        IStaticDelegate.LogEvent(key, "Manual", count);
    }

    public static void GetMaterial(Currency currency, int count, string itemId, ItemType itemType = ItemType.Game)
    {
    }

    public static void ConsumeMaterial(Currency currency, int count, string itemId, ItemType itemType = ItemType.Game)
    {
    }

    public enum Currency
    {
        Coin = 0,
        SkinBall = 1,
        SkinTube = 2,
        SkinTheme = 3,
        Tool = 4,
        Background = 5,
        Tile = 6,
        Manual_Time = 7,
        GoldBrick = 8,
        MagicStick = 9,
        RemoveAD = 10,
        Diamond = 11,
    }

    public enum ItemType
    {
        Game,
        AD,
        Buy,
        IAP,
        Share
    }

    #endregion Currency

    #region Remote Notification

    public static void Remote_LifeNotificationEnter()
    {
        string key = $"Remote_LifeNotificationEnter";
        IStaticDelegate.LogEvent(key);
    }

    #endregion Remote Notification

    #region MoneyWallet

    public static void MoneyWallet_Open()
    {
        string key = $"MoneyWallet_Open";
        IStaticDelegate.LogEvent(key);
    }

    public static void MoneyWallet_Lose()
    {
        string key = $"MoneyWallet_Lose";
        IStaticDelegate.LogEvent(key);
    }

    public static void MoneyWallet_WatchAd()
    {
        string key = $"MoneyWallet_WatchAd";
        IStaticDelegate.LogEvent(key);
    }

    #endregion MoneyWallet

    #region GameFlow

    /// <summary>
    /// 进入游戏流程，  目标：创建进入游戏流程漏斗，查看在进入游戏的一次过程中流失的用户，和流失的点
    /// 一般流程点：
    /// 1、firebase 初始化成功（只有firebase 初始化成功之后，打出的点才回正确上报）
    /// 2、Loading 加载完成
    /// 3、Home 加载完成
    /// 4、进入游戏
    /// </summary>
    public static void GameFlow_InitSuccess()
    {
        string key = $"GameFlow_InitSuccess_{GameFlowKey}";
        if (!Storage.Instance.HasKey(key))
        {
            Storage.Instance.SetBool(key, true);
            IStaticDelegate.LogEvent(key);
        }
    }

    public static void GameFlow_LoadingCompleted()
    {
        string key = $"GameFlow_LoadingCompleted_{GameFlowKey}";
        if (!Storage.Instance.HasKey(key))
        {
            Storage.Instance.SetBool(key, true);
            IStaticDelegate.LogEvent("GameFlow_LoadingCompleted");
        }
    }

    public static void GameFlow_EnterHome()
    {
        string key = $"GameFlow_EnterHome_{GameFlowKey}";
        if (!Storage.Instance.HasKey(key))
        {
            Storage.Instance.SetBool(key, true);
            IStaticDelegate.LogEvent("GameFlow_EnterHome");
        }
    }

    public static void GameFlow_EnterGame()
    {
        string key = $"GameFlow_EnterGame_{GameFlowKey}";
        if (!Storage.Instance.HasKey(key))
        {
            Storage.Instance.SetBool(key, true);
            IStaticDelegate.LogEvent("GameFlow_EnterGame");
        }
    }

    private static string GameFlowKey = "StaticModule_GameFlowKey";

    #endregion GameFlow

    #region UIPoint

    public static void UIPoint_Dialog_Enter(string dialogName)
    {
        IStaticDelegate.LogEvent($"UIPoint_{dialogName}_Enter");
    }

    public static void UIPoint_Dialog_Exit(string dialogName)
    {
        IStaticDelegate.LogEvent($"UIPoint_{dialogName}_Exit");
    }

    #endregion UIPoint

    #region Level

    public static void BeginStage(int levelId, int attemptNum, CopiesType type = CopiesType.Thread)
    {
        if (type == CopiesType.Thread) IStaticDelegate.BeginStage(levelId.ToString());
        else
        {
            var key = $"BeginStage";
            IStaticDelegate.LogEvent(key, type.ToString(), levelId.ToString());
        }
    }

    public static void FailedStage(int levelId, int attemptNum, CopiesType type = CopiesType.Thread)
    {
        if (type == CopiesType.Thread) IStaticDelegate.FailedStage(levelId.ToString(), "Time Over");
        else
        {
            var key = $"FailedStage";
            IStaticDelegate.LogEvent(key, type.ToString(), levelId.ToString());
        }
    }

    public static void CompletedStage(int levelId, int attemptNum, int awardNum, CopiesType type = CopiesType.Thread)
    {
        if (type == CopiesType.Thread) IStaticDelegate.CompletedStage(levelId.ToString());
        else
        {
            var key = $"CompletedStage";
            IStaticDelegate.LogEvent(key, type.ToString(), levelId.ToString());
        }
    }

    public static void RestartStage(int levelId, int attemptNum, CopiesType type = CopiesType.Thread)
    {
        var key = $"RestartStage";
        IStaticDelegate.LogEvent(key, type.ToString(), levelId.ToString());
    }

    public static void PassLevelState(int level, int remainCoin, int remainSlotBack, int remainUndo, int remainShuffle)
    {
        var key = $"PassLevelState_{level}";
        IStaticDelegate.LogEvent(key,
            new StaticParameter("RemainCoin", (remainCoin / 100).ToString()),
            new StaticParameter("RemainSlotBack", remainSlotBack),
            new StaticParameter("RemainUndo", remainUndo),
            new StaticParameter("RemainShuffle", remainShuffle));
    }

    //参数第几连胜
    public static void WinningStreakLose(int number)
    {
        var key = $"WinningStreakLose";
        IStaticDelegate.LogEvent(key, "LoseNumber", number.ToString());
    }

    #endregion Level

    #region AD

    public static void PurchaseRemoveAD()
    {
        IStaticDelegate.LogEvent("PurchaseRemoveAD");
    }

    public static void TryShowAutoInsertitial(string pos)
    {
        IStaticDelegate.LogEvent("AD_Insertitial_TryShow", "pos", pos);
    }

    public static void DisplayRewardAds(string pos)
    {
        IStaticDelegate.LogEvent("AD_Reward_Display", "pos", pos);
    }

    public static void WatchRewardAds(string pos)
    {
        IStaticDelegate.LogEvent("AD_Reward_Watch", "pos", pos);
    }

    public static void WatchInsertitialAds(string pos)
    {
        IStaticDelegate.LogEvent("AD_Insertitial_Watch", "pos", pos);
    }

    public static void RewardTriger(string pos)
    {
        IStaticDelegate.LogEvent("Reward_Triger", "pos", pos);
    }

    public static void ADInsertitialTriger(string pos)
    {
        IStaticDelegate.LogEvent("AD_Insertitial_Triger", "pos", pos);
    }

    public static void ADInsertitialNo()
    {
        IStaticDelegate.LogEvent("AD_Insertitial_No");
    }

    public static void RewardLoadingNo()
    {
        IStaticDelegate.LogEvent("Reward_Loading_No");
    }

    public static void RewardLoading()
    {
        IStaticDelegate.LogEvent("Reward_Loading");
    }

    public static void RewardLoadingYes()
    {
        IStaticDelegate.LogEvent("Reward_Loading_Yes");
    }

    public static void RewardLoadingReplace()
    {
        IStaticDelegate.LogEvent("Reward_Loading_Replace");
    }

    public static void RewardCompleted()
    {
        IStaticDelegate.LogEvent("Reward_Completed");
    }

    public static void RewardCancel()
    {
        IStaticDelegate.LogEvent("Reward_Cancel");
    }

    #endregion AD

    #region ExtendSystem

    public static void ShareSuccess()
    {
        IStaticDelegate.LogEvent("ShareSuccess");
    }

    public static void Rating_Popup()
    {
        IStaticDelegate.LogEvent("Rating_Popup");
    }

    public static void Click_EmailFeedback()
    {
        IStaticDelegate.LogEvent("Click_EmailFeedback");
    }

    #endregion ExtendSystem

    #region InAppPurchase

    public static void OpenInAppPurchaseUI()
    {
        IStaticDelegate.LogEvent("IAP_OpenUI");
    }

    public static void ClickInAppPurchaseButton(string pos)
    {
        IStaticDelegate.LogEvent("IAP_ClickButton", "pos", pos);
    }

    public static void InAppPurchaseSuccess(string pos)
    {
        IStaticDelegate.LogEvent("IAP_Success", "pos", pos);
    }

    public static void InAppPurchaseFailed(string pos)
    {
        IStaticDelegate.LogEvent("IAP_Failed", "pos", pos);
    }

    #endregion InAppPurchase

    #region player info(SetAccount SetLevel)

    /*
 * accountId需保证全局唯一，且终生不变。在多设备中传入同样accountId，数据将归入同一个玩家内，
 * 设备激活增加，玩家数不增加；同一设备中传入多个不同 accountId（游戏允许玩家刷小号的情况），会计算多个玩家账户数。
 */

    public static void SetAccount(string accountId)
    {
        IStaticDelegate.SetAccount(accountId);
    }

    public static void SetLevel(int level)
    {
        IStaticDelegate.LevelUp(level);
    }

    #endregion player info(SetAccount SetLevel)

    #region LocalNofification

    public static void RemoteNotificationReceive()
    {
        IStaticDelegate.LogEvent("RemoteNotification_Receive");
    }

    public static void LocalNofificationScheduled()
    {
        IStaticDelegate.LogEvent("LocalNofification_Scheduled");
    }

    public static void LocalNofificationCancel()
    {
        IStaticDelegate.LogEvent("LocalNofification_Cancel");
    }

    public static void LocalNofificationInvokeApp()
    {
        IStaticDelegate.LogEvent("LocalNofification_InvokeApp");
    }

    #endregion LocalNofification

    #region Setting

    public static void SwitchMusicOff()
    {
        IStaticDelegate.LogEvent("MusicOff");
    }

    public static void SwitchSoundOff()
    {
        IStaticDelegate.LogEvent("SoundOff");
    }

    public static void SwitchVibrateOff()
    {
        IStaticDelegate.LogEvent("VibrateOff");
    }

    #endregion Setting

    #region AD Event

    public static void LoginSecondDay()
    {
        string key = $"LoginSecondDay_StaticModule";
        if (!Storage.Instance.HasKey(key))
        {
            Storage.Instance.SetBool(key, true);
            IStaticDelegate.LogEvent("LoginSecondDay");
        }
    }

    public static void LoginThirdDay()
    {
        string key = $"LoginThirdDay_StaticModule";
        if (!Storage.Instance.HasKey(key))
        {
            Storage.Instance.SetBool(key, true);
            IStaticDelegate.LogEvent("LoginThirdDay");
        }
    }

    public static void LoginSevenDay()
    {
        string key = $"LoginSevenDay_StaticModule";
        if (!Storage.Instance.HasKey(key))
        {
            Storage.Instance.SetBool(key, true);
            IStaticDelegate.LogEvent("LoginSevenDay");
        }
    }

    public static void WatchAd1()
    {
        IStaticDelegate.LogEvent("WatchAd1");
    }

    public static void WatchAd5()
    {
        IStaticDelegate.LogEvent("WatchAd5");
    }

    public static void WatchAd10()
    {
        IStaticDelegate.LogEvent("WatchAd10");
    }

    public static void WatchAd20()
    {
        IStaticDelegate.LogEvent("WatchAd20");
    }

    #endregion AD Event

    #region SDK

    public static void AppsFlyerInstallStatus(string status)
    {
        IStaticDelegate.LogEvent($"AppsFlyer_Install_Type", "status", status);
    }

    public static void AppsFlyerMediaSource(string mediaSource)
    {
        IStaticDelegate.LogEvent($"AppsFlyer_Media_Source", "media_source", mediaSource);
    }

    public static void AppsFlyerPlayerType(string type)
    {
        IStaticDelegate.LogEvent($"AppsFlyer_Player_Type", "type", type);
    }

    public static void BindFacebook()
    {
        IStaticDelegate.LogEvent($"BindFacebook");
    }

    #endregion SDK

    #region Chest

    public static void OpenStarChest()
    {
        IStaticDelegate.LogEvent($"OpenStarChest");
    }

    public static void OpenLevelChest()
    {
        IStaticDelegate.LogEvent($"OpenLevelChest");
    }

    #endregion Chest

    #region Powerup

    public static void UsePowerup(string type)
    {
        IStaticDelegate.LogEvent($"UsePowerup", "powerup_type", type);
    }

    #endregion Powerup

    #region Map

    public static void UnLockMap(string map)
    {
        IStaticDelegate.LogEvent($"UnlockMap_{map}");
    }

    public static void UnLockMapIcon(string map)
    {
        IStaticDelegate.LogEvent($"UnlockMapIcon", "mapIcon", map);
    }

    public static void SetBackGroundMapName(string map)
    {
        IStaticDelegate.LogEvent($"BackGroundMap", "MapName", map);
    }

    #endregion Map

    #region ValueBook

    public static void ValueBookLevel(int value)
    {
        IStaticDelegate.LogEvent($"ValueBookLevel{value}");
    }

    #endregion ValueBook

    #region DailyChallenge

    public static void EnterChallenge()
    {
        IStaticDelegate.LogEvent($"EnterChallenge");
    }

    #endregion DailyChallenge

    #region UserPurchaseHabit

    public static void PurchaseHabitToHighInAppPurchase()
    {
        IStaticDelegate.LogEvent($"PurchaseHabit_HIAP");
    }

    public static void PurchaseHabitToHighAD()
    {
        IStaticDelegate.LogEvent($"PurchaseHabit_HAD");
    }

    #endregion UserPurchaseHabit

    #region Redeem

    public static void RedeemApply(int money)
    {
        IStaticDelegate.LogEvent("Redeem_Apply", "money", money.ToString());
    }

    public static void RedeemTaskFinish(int money, int index)
    {
        StaticParameter[] staticParameters = new StaticParameter[] {
            new StaticParameter("money",money.ToString()),
            new StaticParameter("index",index.ToString()),
        };
        IStaticDelegate.LogEvent("Redeem_Task_Finish", staticParameters);
    }

    public static void RedeemShare(int money)
    {
        IStaticDelegate.LogEvent("Redeem_Share", "money", money.ToString());
    }



    public static void RedeemAgain(int money)
    {
        IStaticDelegate.LogEvent("Redeem_Again", "money", money.ToString());
    }

    public static void RedeemCountry(string country)
    {
        IStaticDelegate.LogEvent("Redeem_Country", "country", country);
    }

    #endregion Redeem
}