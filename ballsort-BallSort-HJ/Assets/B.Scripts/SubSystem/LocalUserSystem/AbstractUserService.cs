using System;
using System.Collections.Generic;
using Models;

public abstract class AbstractUserService
{
    public abstract void GuestLogin(Action<User, bool> onCompleted);
    public abstract void FetchGameConfig(Action<GameConfigs, bool> onCompleted);
    public abstract void FetchIPInfo1(Action<IPInfo, bool> onCompleted);
    public abstract void FetchIPInfo2(Action<IPInfo, bool> onCompleted);

    public abstract void UpdateUserInfo(Action<bool> onCompleted, Dictionary<string, string> updateData);

    public abstract void UpdateUserProfile(string profile);

    public UserType GetUserType()
    {
        return PlayerDataStorage.GetUserType();
    }
}