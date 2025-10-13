using System;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;

public class GameStatic
{
    public int PassLevelNum;
    public PersistenceData<int> SumWatchAD;
    public int FirstLoginTime;
    public PersistenceData<bool> HaveSecondDayLogin;
    public PersistenceData<int> PlayTime; //游戏次数
    public PersistenceData<int> OpenGameTime; //游戏次数

    public void PlusPlayTime()
    {
        PlayTime.Value++;
    }
    public void PlusOpenGameTime()
    {
        OpenGameTime.Value++;
        Debug.Log($"游玩次数{OpenGameTime.Value}");
    }
    
    public bool IsFirstPlayTime()
    {
        return PlayTime.Value == 0 && Game.Instance.LevelModel.MaxUnlockLevel.Value <= 1;
    }

    public bool IsFirstLoginTimeUnset()
    {
        return FirstLoginTime == -1;
    }

    public bool IsSecondDay()
    {
        DateTime first = DataFormater.ConvertTimeStampToDateTime(FirstLoginTime);

        TimeSpan span = DateTime.Now - first;

        return span.TotalDays > 1;
    }

    public void SetFirstLoginTime(int timestamp)
    {
        FirstLoginTime = timestamp;
        this.Save();
    }

    public void WatchADPlus()
    {
        SumWatchAD.Value++;

        if (SumWatchAD.Value == 1)
        {
            StaticModule.WatchAd1();
        }
        else if (SumWatchAD.Value == 5)
        {
            StaticModule.WatchAd5();
        }
        else if (SumWatchAD.Value == 10)
        {
            StaticModule.WatchAd10();
        }
        else if (SumWatchAD.Value == 20)
        {
            StaticModule.WatchAd20();
        }
    }

    public GameStatic()
    {
        HaveSecondDayLogin = new PersistenceData<bool>("GameStatic_HaveSecondDayLogin", false);
        SumWatchAD = new PersistenceData<int>("GameStatic_SumWatchAD", 0);
        PlayTime = new PersistenceData<int>("GameStatic_PlayTime", 0);
        OpenGameTime = new PersistenceData<int>("GameStatic_OpenGameTime", 0);
        this.Read();
    }
}

public static class GameStaticSaver
{
    public static void Save(this GameStatic origin)
    {
        Storage.Instance.SetInt($"PassLevel_{GameStaticSaverKey}", origin.PassLevelNum);
        Storage.Instance.SetInt($"FirstLoginTime_{GameStaticSaverKey}", origin.FirstLoginTime);
//        Storage.Instance.SetInt($"BackHomeTime_{GameStaticSaverKey}", origin.BackHomeTime);
    }

    public static void Read(this GameStatic origin)
    {
        origin.PassLevelNum = Storage.Instance.GetInt($"PassLevel_{GameStaticSaverKey}", 0);
        origin.FirstLoginTime = Storage.Instance.GetInt($"FirstLoginTime_{GameStaticSaverKey}", -1);
//        origin.BackHomeTime = Storage.Instance.GetInt($"BackHomeTime_{GameStaticSaverKey}", 0);
    }


    private const string GameStaticSaverKey = "GameStaticSaverKey";
}