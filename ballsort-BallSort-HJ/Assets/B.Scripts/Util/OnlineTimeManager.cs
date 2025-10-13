using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;

public class OnlineTimeManager : MonoSingleton<OnlineTimeManager>
{
    private float _tick = 0f;
    private float _lastCheckTick = 0f;
    public int OnlineTime { get; private set; } = 0;
    public Action OnlineTimeChangeAction;

    private PersistenceData<int> onlineTimeSum;//在线时长 总和（秒）
    private PersistenceData<string> lastDateStr;
    private PersistenceData<int> onlineDaySum;//玩了多少天

    private PersistenceData<string> lastRealOnlineTimeStr;//用服务器时间计算的在线时间点
    private bool hasCheckOfflineTime = false;

    [MakeButton]
    public void SetSpecialOfferTimeout()
    {
        //Game.Instance.GetSystem<SpecialOfferSystem>().SetTimeOut();
    }

    protected override void HandleAwake()
    {
        base.HandleAwake();
        onlineTimeSum = new PersistenceData<int>("OnlineTimeManager_onlineTimeSum", 0);
        onlineDaySum = new PersistenceData<int>("OnlineTimeManager_onlineDaySum", 0);
        lastDateStr = new PersistenceData<string>("OnlineTimeManager_lastDateStr", "");
        lastRealOnlineTimeStr = new PersistenceData<string>("OnlineTimeManager_lastRealOnlineTimeStr", "");

        if (string.IsNullOrEmpty(lastDateStr.Value) ||
            DateTime.Parse(lastDateStr.Value).Date != GetNowRealTime().Date)
        {
            lastDateStr.Value = GetNowRealTime().Date.ToString();
            onlineDaySum.Value++;
            HandleDateChange();
        }
    }
    protected override void HandleApplicationQuit()
    {
        base.HandleApplicationQuit();
    }

    // Update is called once per frame
    void Update()
    {
        _tick += Time.deltaTime;
        if (_tick >= 1f)
        {
            OnlineTime++;
            _tick = 0f;
            //Game.Instance.GetSystem<SpecialOfferSystem>().RefreshRemainTime(1);
            //Game.Instance.GetSystem<LifeSystem>().CheckInfiniteLife(1);
            //Game.Instance.GetSystem<GameSkinSystem>().CheckTrialSkinValid();
            OnlineTimeChangeAction?.Invoke();
            HandlePassTime(1);
        }

        _lastCheckTick += Time.deltaTime;
        if (_lastCheckTick >= 60)
        {
            _lastCheckTick = 0;
            if (onlineTimeSum != null)
            {
                onlineTimeSum.Value += 60;
            }
            RecordRealOnlineTime();
            SetOfflineTime();
        }
    }

    /// <summary>
    /// 玩家平均在线时长
    /// </summary>
    /// <returns></returns>
    public int GetAverageOnlineTime()
    {
        return onlineDaySum.Value > 0 ? onlineTimeSum.Value / onlineDaySum.Value : 0;
    }

    public void SetOfflineTime()
    {
        //Game.Instance.GetSystem<OfflineRewardSystem>().SetOfflineTime(GetNowRealTime());
    }

    //public void SetClaimWorkRewardTime()
    //{
    //    Game.Instance.GetSystem<OfflineRewardSystem>().SetClaimWorkRewardTime(GetNowRealTime());
    //}

    public TimeSpan GetNowRealPassTime(DateTime lastTime)
    {
        return GetNowRealTime() - lastTime;
    }

    public DateTime GetNowRealTime()
    {
        if (Game.Instance.LocalUser.User.HasLoginTime())
        {
            DateTime loginTime = Game.Instance.LocalUser.User.GetLoginTime();
            return loginTime.AddSeconds(OnlineTime);
        }
        else
        {
            return DateTime.Now;
        }

    }

    private void RecordRealOnlineTime()
    {
        if (Game.Instance.LocalUser.User.HasLoginTime() && hasCheckOfflineTime)
        {
            DateTime loginTime = Game.Instance.LocalUser.User.GetLoginTime();
            DateTime nowTime = loginTime.AddSeconds(OnlineTime);
            lastRealOnlineTimeStr.Value = nowTime.ToString();
        }
    }

    public void CheckRealOfflinePassTime()
    {
        if (!hasCheckOfflineTime)
        {
            hasCheckOfflineTime = true;
            if (Game.Instance.LocalUser.User.HasLoginTime() &&
                !string.IsNullOrEmpty(lastRealOnlineTimeStr.Value))
            {
                DateTime loginTime = Game.Instance.LocalUser.User.GetLoginTime();
                DateTime lastOnlineTime = DateTime.Parse(lastRealOnlineTimeStr.Value);
                int offlinePassTime = (int)(loginTime - lastOnlineTime).TotalSeconds;

                if (offlinePassTime > 0)
                {
                    HandlePassTime(offlinePassTime);
                }
                RecordRealOnlineTime();
            }
        }

    }

    public void HandlePassTime(int timeValue)
    {

    }

    /// <summary>
    /// 到了新的一天
    /// 由于登录的问题，这里不一定获取到了服务器时间，所以，登录等需要严格验证服务器时间的地方，需要在自己的单元进行判断处理
    /// </summary>
    private void HandleDateChange()
    {

    }

    public bool CanUseServerTime()
    {
        return Game.Instance.LocalUser.User != null && Game.Instance.LocalUser.User.HasLoginTime();
    }

    public int NextDayRemainTime()
    {
        DateTime nowDate = GetNowRealTime();
        DateTime nextDate = GetNowRealTime().AddDays(1).Date;
        return (int)(nextDate - nowDate).TotalSeconds;
    }
}
