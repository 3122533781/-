using System;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using SoyBean.IAP;

public class GameModel
{
    public int DailyPassStage { get; set; }

    public int FirstStartGameTimestamp { get; private set; }

    public PersistenceData<bool> IsAdRemoved { get; private set; }
    public PersistenceData<int> PurchaseTime { get; private set; } //购买次数

    public PersistenceData<int> WatchTripleCoinTime { get; private set; } //3倍金币观看次数
    public PersistenceData<bool> CanRedeem { get; private set; }
    public PersistenceData<bool> IsOnlyB { get; private set; }
    public PersistenceData<bool> ServerCanRedeem { get; private set; }

    public bool AdRemovedIsBuy()
    {
        return IsAdRemoved.Value || SoyProfile.Get(SoyProfileConst.ADRemoved, SoyProfileConst.ADRemoved_Default);
    }

    public void RegisterAd(Action<bool, bool> action)
    {
        IsAdRemoved.OnValueChange += action;
    }

    public void UnRegisterAd(Action<bool, bool> action)
    {
        IsAdRemoved.OnValueChange -= action;
    }

    public int GetPurchaseTimeExcludeRemoveAD()
    {
        if (IsAdRemoved.Value)
        {
            return PurchaseTime.Value - 1;
        }
        else
        {
            return PurchaseTime.Value;
        }
    }

    public void PlusPurchaseTime()
    {
        PurchaseTime.Value += 1;
    }

   

    public void SetAdRemoved()
    {
        IsAdRemoved.Value = true;
        SoyProfile.Set(SoyProfileConst.ADRemoved, IsAdRemoved.Value);
        ADMudule.HideBanner();
    }

    public double GetCurrentDay()
    {
        TimeSpan span = DateTime.Now - DataFormater.ConvertTimeStampToDateTime(FirstStartGameTimestamp);
        return span.TotalDays;
    }

    public GameModel()
    {
        IsAdRemoved = new PersistenceData<bool>("AppModel_IsAdRemoved", false);
        PurchaseTime = new PersistenceData<int>("AppModel_PurchaseTime", 0);
        CanRedeem = new PersistenceData<bool>("AppModel_CanRedeem", true);
        IsOnlyB = new PersistenceData<bool>("AppModel_IsOnlyB", false);
        ServerCanRedeem = new PersistenceData<bool>("AppModel_ServerCanRedeem", false);
        FirstStartGameTimestamp = PlayerDataStorage.GetFirstStartGameTimestamp();
        DailyPassStage = 0;
        int dayTimeStamp = DataFormater.GetDayTimeStamp(DateTime.Now);
        WatchTripleCoinTime = new PersistenceData<int>($"WatchTripleCoinTime_{dayTimeStamp}", 0);
        PurchaseTotal = new PersistenceData<float>("AppModel_PurchaseTotal", 0);
    }

    /// <summary>
    /// 是否是网赚
    /// </summary>
    /// <returns></returns>
    public bool IsWangZhuan()
    {
#if UNITY_EDITOR
        return Game.Instance.Model.CanRedeem.Value;
#endif
        return Game.Instance.Model.CanRedeem.Value;
        //return Game.Instance.Model.CanRedeem.Value; ;//App.Instance.Model.CanRedeem.Value;
    }

    private int _currentMaxLevel;

    public void RefreshCanRedeem()
    {
#if SDK
        CanRedeem.Value = ServerCanRedeem.Value && !SDKManager.GetAppsFlyerOrganicInstallStatus() &&
            App.Instance.GetSystem<RedeemSystem>().CheckRedeemValid();
#endif
    }

    public void SetServerCanRedeem(bool canRedeem)
    {
        ServerCanRedeem.Value = canRedeem;
        RefreshCanRedeem();
    }

    public PersistenceData<float> PurchaseTotal { get; private set; } //累计充值

    public void PlusPurchaseTotal(float price)
    {
        PurchaseTotal.Value += price;
    }
}