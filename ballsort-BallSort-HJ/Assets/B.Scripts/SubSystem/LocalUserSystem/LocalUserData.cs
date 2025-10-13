using Models;
using System;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils;
using UnityEngine;

public class LocalUserData
{
    public string Token { get; private set; }
    public IpInfoGroup IpGroup { get; private set; }
    public User User { get; private set; }
    public GameConfigs NetGameConfig { get; private set; }
    public string AppsFlyShareLink { get; set; }
    //public Sprite UserSprite {
    //    get {
    //        //return HeadIconConfig.Instance.GetDataByID(PlayerHeadID.Value).Icon;
    //    }
    //}

    public Action<bool> OnCanRedeemValueChange = null;

    public PersistenceData<int> PlayerHeadID { get; private set; }  //玩家头像ID

    public bool CanRedeem()
    {
        // return false;
        //#if UNITY_EDITOR
        //        if (NetGameConfig == null) return false;
        //#endif
        //if (!NetGameConfig.global.canRedeem) return false;
        //if (_isOrganic && !IpGroup.IsIPAvailable()) return false;
        return !GetIsOrganic();
    }

    public bool IsFaceBookLogin()
    {
        if (User == null)
            return false;

        return !string.IsNullOrEmpty(User.facebook_id);
    }

    public bool isLogin()
    {
        return !string.IsNullOrEmpty(Token);
    }

    public void OpenRedeem()
    {
        SetIsOrganic(false);
        OnCanRedeemValueChange?.Invoke(true);
        Game.Instance.isSDKInitCompleted = true;
    }

    private void SetIsOrganic(bool value)
    {
        Storage.Instance.SetInt(OrganicInstallKey, value ? 1 : 0);
    }

    private bool GetIsOrganic()
    {
        return Storage.Instance.GetInt(OrganicInstallKey, 1) == 1 ? true : false;
    }

    public void SetUser(User user)
    {
        LDebug.Log("Receive local user data");
        User = user;
        SetToken(User.token);

        if (!SoyProfile.IsInit() && !string.IsNullOrEmpty(user.profile))
        {
            LDebug.Log("SoyProfile", "SoyProfile is not init, so use remote . " + user.profile);
            SoyProfile.UpdateLocalJsonData(user.profile);
        }

        try
        {
            // SetServerTime(); 服务器时间问题
        }
        catch (Exception e)
        {
            Debug.LogError($"下拉服务器进度中出现错误:{e}");
        }

        //如果没有头像则随机给一个，并上传服务器，也保存在本地
        if (!User.IsOwnHead())
        {
            //int randomID = HeadIconConfig.Instance.RandomGet().ID;
            //Game.Instance.LocalUser.PlayerHeadID.Value = randomID;
            //Game.Instance.GetSystem<LocalUserSystem>().SetUpdateHeadIcon(randomID);
        }
        else
        {
            Game.Instance.LocalUser.PlayerHeadID.Value = User.GetHeadID();
        }
    }

    /// <summary>
    /// 同步服务器时间戳
    /// </summary>
    private void SetServerTime()
    {
        ServerTimeManager.Instance.GetNorthUsTime(Game.Instance.LocalUser.User.server_time, () =>
        {
            //初始化数据
            Game.Instance.CurrencyModel.ResetGameToolByServer();

            bool isAdRemoved = SoyProfile.Get<bool>(SoyProfileConst.ADRemoved, SoyProfileConst.ADRemoved_Default);
            Game.Instance.Model.IsAdRemoved.Value = isAdRemoved;

            int level = SoyProfile.Get(SoyProfileConst.NormalLevel, SoyProfileConst.NormalLevel_Default);
            int specialLevel = SoyProfile.Get(SoyProfileConst.SpecialLevel, SoyProfileConst.NormalLevel_Default);
            Game.Instance.LevelModel.MaxUnlockLevel.Value = level;
            Game.Instance.LevelModel.EnterLevelID = level;

            Game.Instance.LevelModel.MaxUnlockCopies1.Value = specialLevel;
            Game.Instance.LevelModel.EnterCopies1ID = specialLevel;
        });
    }

    public void ResetUser()
    {
        User = new User();
        //UserSprite = null;
    }

    public void SetNetGameConfig(GameConfigs config)
    {
        LDebug.Log($"Receive NetGameConfig NetGameConfig canRedeem:{config.canRedeem}");
        NetGameConfig = config;
        Game.Instance.Model.SetServerCanRedeem(config.canRedeem);
    }

    public void SetIpInfo1(IPInfo value)
    {
        IpGroup.IPInfo_1 = value;
        LDebug.Log("Receive ip info 1");
    }

    public void SetIpInfo2(IPInfo value)
    {
        IpGroup.IPInfo_2 = value;
        LDebug.Log("Receive ip info 2");
    }

    private void SetToken(string value)
    {
        Token = value;
        Storage.Instance.SetString(TokenSaveID, value);
    }

    public LocalUserData()
    {
        IpGroup = new IpInfoGroup();
        Token = Storage.Instance.GetString(TokenSaveID);

        PlayerHeadID = new PersistenceData<int>("LocalUserData_PlayerHeadID", 0);
        User = new User();
    }

    private const string TokenSaveID = "user_token";
    private const string OrganicInstallKey = "OrganicInstall";
}