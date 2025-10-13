using System;
using _02.Scripts.Common;
using _02.Scripts.Util;
using Newtonsoft.Json;
using ProjectSpace.BubbleMatch.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.Common;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using Sirenix.Utilities;
using UnityEngine;

namespace ProjectSpace.Lei31Utils.Scripts.Utils
{
    public class ServerTimeManager : MonoSingleton<ServerTimeManager>
    {
        public DateTime CurrentDateTime { get; set; } //当前时间戳
        public TimeSpan CurrentTimeSpan { get; private set; }
        public bool isInit = false;

        public DateTime DayOverTime
        {
            get => GetDayOverTime();
            set => SetDayOverTime();
        }

        public DateTime WeekOverTime
        {
            get => GetWeekOverTime();
            set => SetWeekOverTime();
        }

        private readonly Coroutine _countDownCoroutine;
        private bool _startUpdate = false;

        /// <summary>
        /// 网络同步获取当前时间戳，挪动到loading中
        /// </summary>
        /// <returns></returns>
        public void GetNorthUsTime(long currentTime, Action callBack)
        {
            //初始化数据
            var utcTimeSpan = TimeStamp2DateTime(currentTime);
            CurrentDateTime = utcTimeSpan.AddHours(-5);
            // var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            // CurrentDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTimeSpan, easternZone); //得到美国时间*/
            callBack?.Invoke();
            isInit = true;
            _startUpdate = true;
            Debug.Log($"服务器时间:当前{CurrentDateTime};跨天:{DayOverTime};跨周{WeekOverTime}");
        }

        public long GetCurrentTime()
        {
            return ConvertDateTimeToUtc_10(CurrentDateTime);
        }

        public long GetNextWeekSpan()
        {
            return ConvertDateTimeToUtc_10(WeekOverTime);
        }

        private DateTime GetDayOverTime()
        {

            return JsonConvert.DeserializeObject<DateTime>("456");
        }

        private DateTime GetWeekOverTime()
        {
            var time = SoyProfile.Get(SoyProfileConst.WeekFreshTime, SoyProfileConst.WeekFreshTimeDefault);
            if (time.IsNullOrWhitespace())
            {
                SetWeekOverTime();
                time = SoyProfile.Get(SoyProfileConst.WeekFreshTime, SoyProfileConst.WeekFreshTimeDefault);
            }

            return JsonConvert.DeserializeObject<DateTime>(time);
        }

        private void SetDayOverTime()
        {
            //触发跨天事件
            var res = JsonConvert.SerializeObject(GetNextDayTime());
            SoyProfile.Set(SoyProfileConst.DayFreshTime, res);
        }

        private void SetWeekOverTime()
        {
            var res = JsonConvert.SerializeObject(GetNextWeekTime());
            //触发跨天事件
            SoyProfile.Set(SoyProfileConst.WeekFreshTime, res);
        }

        public DateTime GetNextDayTime()
        {
            var currentTime = CurrentDateTime;
            var dateTime = currentTime.AddDays(1d);
            dateTime = dateTime.Date;
            return dateTime;
        }

        private DateTime GetNextWeekTime()
        {
            var currentTime = CurrentDateTime;
            var dateTime = currentTime.AddDays(7 - (int)CurrentDateTime.DayOfWeek);
            return dateTime.Date;
        }

        private DateTime TimeStamp2DateTime(long timeStamp, int timeZone = 0)
        {
            DateTime startTime = new DateTime(1970, 1, 1, timeZone, 0, 0);
            DateTime dt = startTime.AddMilliseconds(timeStamp);
            return dt;
        }

        /// <summary>
        /// 时间转化为10位时间戳
        /// </summary>
        /// <param name="time">获取的时间</param>
        /// <returns></returns>
        public static long ConvertDateTimeToUtc_10(DateTime time)
        {
            TimeSpan timeSpan = time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(timeSpan.TotalSeconds);
        }

        private void RefreshDayAction()
        {
            if (CurrentDateTime >= DayOverTime) //跨天
            {
                SetDayOverTime();
         
                RefreshDay();
                Debug.Log($"Bear:触发跨天");
            }
            else
            {


            }
        }

        private void RefreshWeekAction()
        {
            if (CurrentDateTime >= WeekOverTime) //跨天
            {
                RefreshWeek();
             
            }
            else
            {
            }
        }

        public void RefreshWeek()
        {
            SetWeekOverTime();
        }

        public void RefreshDay()
        {
            EventDispatcher.instance.DispatchEvent(AppEventType.DayOver);
        }

        #region 服务器时间控制器

        //跨天携程,模拟服务器再跑
        private float _lastCheckTime; //上一次检测的时间

        public ServerTimeManager(Coroutine countDownCoroutine)
        {
            _countDownCoroutine = countDownCoroutine;
        }

        private static event Action OnPassTime = delegate { }; //每秒执行的方法

        public static void RegisterActionOnPassTime(Action action)
        {
            OnPassTime += action;
        }

        public static void UnRegisterActionOnPassTime(Action action)
        {
            if (!IsQuiting)
            {
                OnPassTime -= action;
            }
        }

        private bool _isOffLine;
        private bool _isPaused;

        private void OnApplicationFocus(bool hasFocus)
        {
            if (_isOffLine == false && hasFocus == false)
            {
                _isOffLine = true;
            }

            _isPaused = !hasFocus;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (_isOffLine == false && pauseStatus)
            {
                _isOffLine = true;
            }

            _isPaused = pauseStatus;
        }

        private void Update()
        {
            //1秒检测一次
            var passTime = Time.realtimeSinceStartup - _lastCheckTime;
            if (!_startUpdate || !(passTime >= 1)) return;
            _lastCheckTime = Time.realtimeSinceStartup;
            CurrentDateTime = CurrentDateTime.AddSeconds(passTime); //维护当前游戏时间
            OnPassTime?.Invoke();
            RefreshDayAction();
            RefreshWeekAction();

            SoyProfile.DelayUpdate();
        }

        #endregion 服务器时间控制器
    }
}