using System;
using System.Collections.Generic;
using Models;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;

public class LocalUserSystem : GameSystem
{
    public Action OnUserNameChange = delegate { };
    public Action OnUserHeadIconChange = delegate { };
    
    public override void Init()
    {
        // InitFaceBook();
        // GooglePlayCheck();
    }

    public override void Destroy()
    {
    }
    
    public void UpdateProfile(string value)
    {
        Service.UpdateUserProfile(value);
    }

    public void FetchGameConfig(Action onFetchGameConfigCompleted)
    {
        _onFetchGameConfigCompleted = onFetchGameConfigCompleted;
        Service.FetchGameConfig(FetchGameConfigCompleted);
    }

    public void FetchIpInfo(Action onFetchIpCompleted)
    {
        _onFetchIpCompleted = onFetchIpCompleted;
        Service.FetchIPInfo1(FetchIpInfo1Completed);
        Service.FetchIPInfo2(FetchIpInfo2Completed);
    }

    /// <summary>
    /// 第三方登录
    /// </summary>
    public void ThirdLogin(Action onLoginCompleted)
    {
        // StaticModule.Network_Login();
        _onGuestLoginCompleted = onLoginCompleted;
        // LoginType type = Game.Instance.LocalUser.GetLoginType();
        // if (type == LoginType.FaceBook)
        // {
        //     Service.FacebookLogin(GuestLoginCompleted);
        // }
        // else if (type == LoginType.Google)
        // {
        //     Service.GoogleLogin(GuestLoginCompleted);
        // }
        // else if (type == LoginType.Apple)
        // {
        //     Service.AppleLogin(GuestLoginCompleted);
        // }
        // else 
        Service.GuestLogin(GuestLoginCompleted);
    }
    /// <summary>
    /// 第三方绑定
    /// </summary>
    public void ThirdLoginBind(Action<bool> onLoginCompleted)
    {
        // if (Game.Instance.LocalUser.ThirdLoginData != null)
        // {
        //     if (Game.Instance.LocalUser.ThirdLoginData.Type == LoginType.FaceBook)
        //     {
        //         Service.FacebookBind(Game.Instance.LocalUser.ThirdLoginData.ID, ThirdLoginBindCompleted);
        //     }
        //     else if (Game.Instance.LocalUser.ThirdLoginData.Type == LoginType.Google)
        //     {
        //         Service.GoogleBind(Game.Instance.LocalUser.ThirdLoginData.ID, ThirdLoginBindCompleted);
        //     }
        //     else if (Game.Instance.LocalUser.ThirdLoginData.Type == LoginType.Apple)
        //     {
        //         Service.AppleBind(Game.Instance.LocalUser.ThirdLoginData.ID, ThirdLoginBindCompleted);
        //     }
        // }
        // else 
            onLoginCompleted.Invoke(false);

        void ThirdLoginBindCompleted(User user, bool suceess)
        {
            if (suceess)
            {
                Game.Instance.LocalUser.SetUser(user);
                onLoginCompleted.Invoke(true);
            }
            else onLoginCompleted.Invoke(false);
        }
    }

    public void SetReferUser(string user_id)
        // 根据AF的归因记录设置用户的上家
    {
        string data = string.Format("{{\"refer_user\":\"{0}\"}}", user_id);
        UnityNetworkManager.Instance.Post("/user/refer/set", data, (err, response) => { });
    }

    public void SetUpdateHeadIcon(int index)
    {
        LDebug.Log("update head icon :" + index);
        Game.Instance.LocalUser.PlayerHeadID.Value = index;
        Game.Instance.LocalUser.User.avatar_url = index.ToString();

        Dictionary<string, string> infoData = new Dictionary<string, string>();
        infoData.Add("avatar_url", Game.Instance.LocalUser.User.avatar_url);

        Service.UpdateUserInfo((isSuccess) =>
        {
            Debug.Log($"SetUpdateHeadIcon isSuccess:{isSuccess}");
        }, infoData);

        OnUserHeadIconChange.Invoke();
    }

    public void SetUpdateUserName(string userName)
    {
        LDebug.Log("update user name :" + userName);

        Dictionary<string, string> infoData = new Dictionary<string, string>();
        infoData.Add("username", userName);

        Service.UpdateUserInfo((isSuccess) =>
        {
            if (isSuccess)
            {
                Game.Instance.LocalUser.User.username = userName;
                //FloatingWindow.Instance.Show(LocalizationManager.Instance.GetTextByTag(LocalizationConst.CHANGE_SUCCESS));
                OnUserNameChange.Invoke();
                Debug.Log($"SetUpdateUserName isSuccess:{isSuccess}");
            }
            else
            {
                //FloatingWindow.Instance.Show(LocalizationManager.Instance.GetTextByTag(LocalizationConst.CHANGE_FAIL));
            }

        }, infoData);

    }

    private AbstractUserService Service
    {
        get
        {
            if (_service == null)
            {
                if (NetworkSetting.Instance.OpenUserServiceStub)
                {
                    _service = new UserServiceStub();
                }
                else
                {
                    _service = new UserServiceMockup();
                }
            }

            return _service;
        }
    }

    private void FetchGameConfigCompleted(GameConfigs configs, bool isSuccess)
    {
        if (isSuccess)
        {
            Game.Instance.LocalUser.SetNetGameConfig(configs);
        }

        if (_onFetchGameConfigCompleted != null)
            _onFetchGameConfigCompleted.Invoke();
    }

    private void GuestLoginCompleted(User newUser, bool isSuccess)
    {
        if (Game.Instance.Model.GetCurrentDay() > 6)
        {
            StaticModule.LoginSevenDay();
        }
        else if (Game.Instance.Model.GetCurrentDay() > 2)
        {
            StaticModule.LoginThirdDay();
        }
        else if (Game.Instance.Model.GetCurrentDay() > 1)
        {
            StaticModule.LoginSecondDay();
        }
        
        if (isSuccess)
        {
            Game.Instance.LocalUser.SetUser(newUser);
            if (newUser.IsFetchFormService)
            {
                UnityNetworkManager.Instance.user_token = newUser.token;
            }

            if (_onGuestLoginCompleted != null)
                _onGuestLoginCompleted.Invoke();

            StaticModule.SetAccount(newUser.user_id.ToString());
            SDKManager.AppsFlyerGenerateInviteLink(newUser.username, newUser.user_id.ToString());
        }
        else
        {
            new UserServiceMockup().GuestLogin(GuestLoginCompleted);
        }
    }

    private void FetchIpInfo2Completed(IPInfo info2, bool isSuccess)
    {
        if (isSuccess)
        {
            Game.Instance.LocalUser.SetIpInfo2(info2);
        }

        if (_onFetchIpCompleted != null)
            _onFetchIpCompleted.Invoke();
    }

    private void FetchIpInfo1Completed(IPInfo info1, bool isSuccess)
    {
        if (isSuccess)
        {
            Game.Instance.LocalUser.SetIpInfo1(info1);
        }

        if (_onFetchIpCompleted != null)
            _onFetchIpCompleted.Invoke();
    }

    private Action _onFetchIpCompleted;
    private Action _onGuestLoginCompleted;
    private Action _onFetchGameConfigCompleted;

    private AbstractUserService _service;

    public void SetCountry(string country, Action callBack)
    {
        if (Game.Instance.LocalUser.User.country != country)
        {
            Debug.Log($"SetCountry :{country}");
            Dictionary<string, string> infoData = new Dictionary<string, string>();
            infoData.Add("country", country);

            Service.UpdateUserInfo((isSuccess) =>
            {
                Debug.Log($"SetCountry isSuccess:{isSuccess}");
                callBack?.Invoke();
            }, infoData);
        }

    }

    public void GuestLogin(Action onLoginCompleted)
    {
        //StaticModule.Network_Login();
        _onGuestLoginCompleted = onLoginCompleted;
        Service.GuestLogin(GuestLoginCompleted);
    }
}