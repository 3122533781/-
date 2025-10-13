using System;
using System.Collections.Generic;
using Lei31;
using Models;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;

public class UserServiceStub : AbstractUserService
{
    public override void GuestLogin(Action<User, bool> onCompleted)
    {
        //StaticModule.Network_NetLogin();
        string deviceID = SystemInfo.deviceUniqueIdentifier + GameConfig.Instance.DebugUserString;
        User newUser = new User();
        newUser.device_id = deviceID;
        //newUser.device_id = deviceID + "test005";
        UpdateUserLocationByIp(newUser);
        string data = JsonUtility.ToJson(newUser);
        UnityNetworkManager.Instance.Post("auth/signinD", data, (err, response) =>
            {
                if (!err)
                {
                    LDebug.Log("user login:" + response);
                    User fetchUser = JsonUtility.FromJson<User>(response);
                    fetchUser.IsFetchFormService = true;
                    fetchUser.Type = GetUserType();
                    if (onCompleted != null)
                        onCompleted.Invoke(fetchUser, true);
                }
                else
                {
                    if (onCompleted != null)
                        onCompleted.Invoke(null, false);

                    LDebug.Log("user Error:");
                }
            }
        );
    }

    public override void FetchGameConfig(Action<GameConfigs, bool> onCompleted)
    {
        //UnityNetworkManager.Instance.Get("game/config", (err, response) =>
        UnityNetworkManager.Instance.Get("game/config2", (err, response) =>
        {
            if (!err)
            {
                LDebug.Log("FetchGameConfig", response);

                GameConfigs result = JsonUtility.FromJson<GameConfigs>(response);
                if (onCompleted != null)
                    onCompleted.Invoke(result, true);
            }
            else
            {
                if (onCompleted != null)
                    onCompleted.Invoke(null, false);
            }
        });
    }

    public override void FetchIPInfo1(Action<IPInfo, bool> onCompleted)
    {
        UnityNetworkManager.Instance.Get("http://ipapi.co/json", (err, response) =>
        {
            if (!err)
            {
                LDebug.Log("FetchIP", "ip info 1 is:" + response);
                IPInfo info1 = JsonUtility.FromJson<IPInfo>(response);
                Game.Instance.LocalUser.SetIpInfo1(info1);
                if (onCompleted != null)
                    onCompleted.Invoke(info1, true);
            }
            else
            {
                // 暂时     Debug.LogError(response);
                if (onCompleted != null)
                    onCompleted.Invoke(null, false);
            }
        }, true);
    }

    public override void FetchIPInfo2(Action<IPInfo, bool> onCompleted)
    {
        UnityNetworkManager.Instance.Get("http://ip-api.com/json", (err, response) =>
        {
            if (!err)
            {
                LDebug.Log("ip info 2 is:" + response);
                IPInfo info2 = JsonUtility.FromJson<IPInfo>(response);
                if (onCompleted != null)
                    onCompleted.Invoke(info2, true);
            }
            else
            {
                LDebug.Log(response);
                if (onCompleted != null)
                    onCompleted.Invoke(null, false);
            }
        }, true);
    }

    public override void UpdateUserInfo(Action<bool> onCompleted, Dictionary<string, string> updateData)
    {
        JSONObject obj = new JSONObject();
        foreach (var item in updateData)
        {
            obj.Add(item.Key, item.Value);
        }
        string data = obj.ToString();
        UnityNetworkManager.Instance.Post("/user/info", data, (err, response) => { onCompleted.Invoke(!err); });
    }

    public override void UpdateUserProfile(string profile)
    {
        JSONObject obj = new JSONObject();
        obj["profile"] = profile;
        string data = obj.ToString();
        SoyProfile.ONRequest = true;
        UnityNetworkManager.Instance.Post("/user/updateUserProfile", data, (err, response) =>
            {
                if (!err)
                {
                    LDebug.Log("updateUserProfile :" + response);
                    SoyProfile._isStartDelayUpdate.Value = false;
                }
                else
                {
                    LDebug.Log("updateUserProfile err:" + response);
                }

                SoyProfile.ONRequest = false;
            }
        );
    }

    private void UpdateUserLocationByIp(User uInfo)
    {
        if (Game.Instance.LocalUser.IpGroup.HaveValidIp())
        {
            IPInfo validInfo = Game.Instance.LocalUser.IpGroup.GetValidIp();
            uInfo.city = validInfo.city;
            uInfo.country = validInfo.country;
            Debug.Log("TestUserCity:" + uInfo.city);
            Debug.Log("TestUserCountry:" + validInfo.country);
        }
    }
}