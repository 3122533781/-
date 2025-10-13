//只支持 长度 1000 以内，int float bool string

using System.Collections.Generic;
using System.Data.SqlTypes;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;

public class SoyProfile
{
    public const string InitKey = "init";
    public static PersistenceData<bool> _isStartDelayUpdate;
    public static int DelayTime = 0;
    public static bool ONRequest = false;

    /// <summary>
    /// 是否初始化，如果未初始化则需要使用服务器上的数据
    /// </summary>
    /// <returns></returns>
    public static bool IsInit()
    {
        return _profileUnit.Get<bool>(InitKey);
    }

    public static void UpdateLocalJsonData(string data)
    {
        LDebug.Log("SoyProfile", "UpdateLocalJsonData :" + data);
        _localJson.Value = data;
        _profileUnit = new SoyProfileUnit(_localJson.Value);
        _profileUnit.Set<bool>(InitKey, true);
    }

    public static T Get<T>(string key, T defaultValue = default(T))
    {
        return _profileUnit.Get(key, defaultValue);
    }

    public static void Set<T>(string key, T value)
    {
        // LDebug.Log("SoyProfile", $"Save {value} to {key}");
        //
        // _profileUnit.Set(key, value);
        //
        // _localJson.Value = _profileUnit.DicToJsonString();
        // Game.Instance.LocalUser.User.profile = _localJson.Value;
        //
        // _isStartDelayUpdate.Value = true;
        // Game.Instance.GetSystem<LocalUserSystem>().UpdateProfile(_localJson.Value);
    }


    /// <summary>
    /// 延时一段时间向服务器同步数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="delayTime"></param>
    /// <typeparam name="T"></typeparam>
    public static void DelaySet<T>(string key, T value, int delayTime = 3)
    {
        if (Game.Instance.LocalUser == null || Game.Instance.LocalUser.User == null)
        {
            return;
        }

        LDebug.Log("SoyProfile", $"Save {value} to {key}");

        _profileUnit.Set(key, value);

       // _localJson.Value = _profileUnit.DicToJsonString();
        Game.Instance.LocalUser.User.profile = _localJson.Value;
        _isStartDelayUpdate.Value = true;
        DelayTime = delayTime;
    }


    public static void DelayUpdate()
    {
        //开启延时 && 当前没在使用接口
        if (_isStartDelayUpdate.Value && !ONRequest)
        {
            if (DelayTime <= 0)
            {
                Game.Instance.GetSystem<LocalUserSystem>().UpdateProfile(_localJson.Value);
            }
            else
            {
                DelayTime--;
            }
        }
    }

    static SoyProfile()
    {
        _localJson = new PersistenceData<string>("SoyProfile_LocalJson", "{\"init\":false}");
        _profileUnit = new SoyProfileUnit(_localJson.Value);
        _isStartDelayUpdate = new PersistenceData<bool>("SoyProfile_StartDelayUpdate", false);
    }

    private static PersistenceData<string> _localJson;
    private static SoyProfileUnit _profileUnit;
}

public static class SoyProfileConst
{
    public const string NormalLevel = "NormalLevel";
    public const int NormalLevel_Default = 1;
    
    
    public const string SpecialLevel = "SpecialLevel";
    public const int SpecialLevel_Default = 1;

    public const string UserLogo = "UserLogo";
    public const string UserLogo_Default = "1_0";

    public const string HaveCoin = "HaveCoin";
    public const int HaveCoinDefault = 0;


    public const string ForeverCard = "ForeverCard";
    public const bool ForeverCardDefault = false;

    public const string MonthCard = "MonthCard";
    public const long MonthCardDefault = 0;

    public const string ADRemoved = "ADRemoved";
    public const bool ADRemoved_Default = false;

    public const string PlateProgress = "PlateProgress";
    public const string PlateProgressDefault = "";

    public const string SigninProgress = "SigninProgress";
    public const string SigninProgressDefault = "";

    public const string FirstChargeProgress = "FirstChargeProgress";
    public const string FirstChargeProgressDefault = "";

    public const string LimitTimeProgress = "LimitTimeProgress";
    public const string LimitTimeProgressDefault = "";


    public const string CoinShopProgress = "CoinShopProgress";
    public const string CoinShopProgressDefault = "";

    public const string DayFreshTime = "RefreshDayTime";
    public const string DayFreshTimeDefault = "";

    public const string WeekFreshTime = "WeekFreshTime";
    public const string WeekFreshTimeDefault = "";


    public const string PurchaseTime = "PurchaseTime";
    public const string PurchaseTimeDefault = "";

    public const string OnlineTimeProgress = "OnlineTimeProgress";
    public const string OnlineTimeProgressDefault = "";

    public const string LevelChallengeProgress = "LevelChallengeProgress";
    public const string LevelChallengeProgressDefault = "";

    public const string GameToolProgress = "GameToolProgress";
    public const string GameToolProgressDefault = "";

    public const string GameSkinProgress = "GameSkinProgress";
    public const string GameSkinProgressDefault = "";

    public const string ChestProgress = "ChestProgress";
    public const string ChestProgressDefault = "";
}