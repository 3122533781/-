using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Models;
using UnityEngine;

public class UserServiceMockup : AbstractUserService
{
    public override void GuestLogin(Action<User, bool> onCompleted)
    {
        //StaticModule.Network_LocalLogin();
        User newUser = new User();
        newUser.IsFetchFormService = false;
        newUser.device_id = SystemInfo.deviceUniqueIdentifier;
        newUser.Type = GetUserType();
        newUser.username = "MockupLoginName";
        if (onCompleted != null)
        {
            onCompleted.Invoke(newUser, true);
        }
    }

    public override void FetchGameConfig(Action<GameConfigs, bool> onCompleted)
    {
        GameConfigs result = new GameConfigs();
        result.canRedeem = false;

        if (onCompleted != null)
        {
            onCompleted.Invoke(result, true);
        }
    }

    public override void FetchIPInfo1(Action<IPInfo, bool> onCompleted)
    {
        FakeFetchIp(onCompleted);
    }

    public override void FetchIPInfo2(Action<IPInfo, bool> onCompleted)
    {
        FakeFetchIp(onCompleted);
    }

    public override void UpdateUserInfo(Action<bool> onCompleted, Dictionary<string, string> updateData)
    {
        onCompleted.Invoke(true);
    }

    public override void UpdateUserProfile(string profile)
    {

    }

    private void FakeFetchIp(Action<IPInfo, bool> onCompleted)
    {
        IPInfo ipInfo = new IPInfo();

        //获取本机Ip
        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipInfo.ip = ip.ToString(); //ipv4
            }
            else if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ipInfo.ip = ip.ToString(); //ipv6
            }
        }

        if (onCompleted != null)
        {
            onCompleted.Invoke(ipInfo, true);
        }
    }
}