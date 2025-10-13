using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class CheckinSystem : GameSystem
{
    public int CurrentCheckinDayIndex { get; private set; }

    public void PlusCheckinDay()
    {
        if (HasCheckInToday == false)
        {
            CurrentCheckinDayIndex++;
            if (CurrentCheckinDayIndex == 6)
            {
                //Completed
                CurrentCheckinDayIndex = -1;
            }

            Storage.Instance.SetInt(CurrentCheckinDayIndexKey, CurrentCheckinDayIndex);
            LastShowTime = DataFormater.ConvertDateTimeToTimeStamp(DateTime.Now);
            HasCheckInToday = true;
            Storage.Save();
        }

    }

    public bool NeedShowToday()
    {
        //int today = DataFormater.GetDayTimeStamp(DateTime.Now);
        //return LastShowTime < today;
        return !HasCheckInToday;
    }

    public override void Init()
    {
        CurrentCheckinDayIndex = Storage.Instance.GetInt(CurrentCheckinDayIndexKey, -1);
    }

    public override void Destroy()
    {
    }
    public GoodSubType2 GetTodayReward()
    {
        // 确定今天是周期中的第几天
        int dayIndex = CurrentCheckinDayIndex == -1 ? 0 : CurrentCheckinDayIndex;

        var config = CheckinConfig.Instance;
        if (config != null && config.All != null && dayIndex < config.All.Count)
        {
            return config.All[dayIndex].Rewards[0].type;
        }


        return GoodSubType2.AddPipe;
    }
    public bool HasCheckInToday
    {
        get
        {
            string strKey = DateTime.Today.ToString("yyyy_MM_dd_checkin");
            UnityEngine.Debug.Log("时间1" + strKey + " " + Storage.Instance.GetBool(strKey));
            return Storage.Instance.GetBool(strKey);
        }
        set
        {
            string strKey = DateTime.Today.ToString("yyyy_MM_dd_checkin");
            UnityEngine.Debug.Log("时间2" + strKey + " " + Storage.Instance.GetBool(strKey));
            Storage.Instance.SetBool(strKey, value);
        }
    }
    public int LastShowTime
    {
        get { return Storage.Instance.GetInt(LastShowTimeKey, 0); }
        set { Storage.Instance.SetInt(LastShowTimeKey, value); }
    }

    private const string CurrentCheckinDayIndexKey = "CurrentCheckinDayIndexKey";
    private const string LastShowTimeKey = "LastShowTimeKey";
}