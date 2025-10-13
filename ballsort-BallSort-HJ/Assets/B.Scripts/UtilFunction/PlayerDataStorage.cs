using System;
using System.Collections.Generic;
using UnityEditor;

public class PlayerDataStorage
{
    public static int GetFirstStartGameTimestamp()
    {
        if (!Storage.Instance.HasKey(FirstStartGameKey))
        {
            Storage.Instance.SetInt(FirstStartGameKey,
                DataFormater.ConvertDateTimeToTimeStamp(DateTime.Now));
        }

        return Storage.Instance.GetInt(FirstStartGameKey);
    }

    public static UserType GetUserType()
    {
        return (UserType) Storage.Instance.GetInt(UserTypeKey, 0);
    }

    public static void SetUserType(UserType value)
    {
        Storage.Instance.SetInt(UserTypeKey, (int) value);
    }

    public static bool GetVibrationOpen()
    {
        return Storage.Instance.GetBool(VibrationOpenKey, true);
    }

    public static void SetVibrationOpen(bool isOpen)
    {
        Storage.Instance.SetBool(VibrationOpenKey, isOpen);
    }

    public static void SetSoundOpen(bool isOpen)
    {
        Storage.Instance.SetBool(SoundOpenKey, isOpen);
    }

    public static bool GetSoundOpen()
    {
        return Storage.Instance.GetBool(SoundOpenKey, true);
    }

    public static void SetBGMOpen(bool isOpen)
    {
        Storage.Instance.SetBool(BGMOpenKey, isOpen);
    }

    public static bool GetBGMOpen()
    {
        return Storage.Instance.GetBool(BGMOpenKey, true);
    }

    public static void SetSumStar(int sumStar)
    {
        Storage.Instance.SetInt(SumStarKey, sumStar);
    }

    public static int GetSumStar()
    {
        return Storage.Instance.GetInt(SumStarKey, 0);
    }

    public static void SetMaxOpenLevel(int value)
    {
        Storage.Instance.SetInt(MaxOpenLevelKey, value);
    }

    public static int GetMaxOpenLevel()
    {
        return Storage.Instance.GetInt(MaxOpenLevelKey, 0);
    }

    public static void SetMapCollectLabel(List<int> collects)
    {
        Storage.Instance.SetList(MapCollectLabel, collects);
    }

    public static List<int> GetMapCollectLabel()
    {
        return Storage.Instance.GetList(MapCollectLabel);
    }

    public static void SetMapExitPosition(float posY)
    {
        Storage.Instance.SetFloat(MapExitPosition, posY);
    }

    public static float GetMapExitPosition()
    {
        return Storage.Instance.GetFloat(MapExitPosition);
    }

    public static void SetExitGameTime(int value)
    {
        Storage.Instance.SetInt(ExitGameTime, value);
    }

    public static int GetExitGameTime()
    {
        return Storage.Instance.GetInt(ExitGameTime);
    }    

    public static void SetRemainingRecoveryManualTime(int value)
    {
        Storage.Instance.SetInt(RemainingRecoveryManualTime, value);
    }

    public static int GetRemainingRecoveryManualTime()
    {
        return Storage.Instance.GetInt(RemainingRecoveryManualTime);
    }

    private const string MapExitPosition = "MapExitPosition";
    private const string MaxOpenLevelKey = "MaxOpenLevelKey";
    private const string SumStarKey = "SumStarKey";
    private const string SoundOpenKey = "SoundOpenKey";
    private const string BGMOpenKey = "BGMOpenKey";
    private const string VibrationOpenKey = "VibrationOpenKey";
    private const string UserTypeKey = "UserTypeKey";
    private const string FirstStartGameKey = "FirstStartGameKey";
    private const string MapCollectLabel = "MapCollectLabel";
    private const string ExitGameTime = "ExitGameTime";
    private const string RemainingRecoveryManualTime = "RemainingRecoveryManualTime";
}