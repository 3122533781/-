// using System;
// using System.Collections.Generic;
// using System.Text;
// using WhiteEggTart;
//
// public static class GameHabitTimeHelper
// {
//     [RegisterCommand(Help = "打印用户活跃信息", Name = "dishab")]
//     public static void DebugHourToTime()
//     {
//         Terminal.Log("ActiveStrin:{0} \n BestActiveHour:{1}", GetHourString(), GetBestActiveHour());
//     }
//
//     /// <summary>
//     /// 获取活跃度最高的小时
//     /// </summary>
//     /// <returns></returns>
//     public static int GetBestActiveHour()
//     {
//         int maxHour = 0;
//         int maxTime = 0;
//         foreach (KeyValuePair<int, int> pair in _hourToTime)
//         {
//             if (pair.Value > maxTime)
//             {
//                 maxTime = pair.Value;
//                 maxHour = pair.Key;
//             }
//         }
//
//         return maxHour;
//     }
//
//     /// <summary>
//     /// 每秒调用一次，20分钟尝试设置一次活跃
//     /// </summary>
//     public static void TickActive()
//     {
//         _onlineTime += 1;
//         if (_onlineTime >= 1200)
//         {
//             SaveActiveHour(DateTime.UtcNow.Hour);
//             _onlineTime = 0;
//         }
//     }
//
//     private static int _onlineTime;
//
//     /// <summary>
//     /// 保存当前活跃的小时
//     /// </summary>
//     public static void SaveActiveHour(int hour)
//     {
//         if (!HasSet(hour) && hour >= 0 && hour < 24)
//         {
//             _hourToTime[hour]++;
//             SaveSetHour(hour);
//             Save();
//             LDebug.Log("GameHabit", $"Save active hour {hour} all:{GetHourString()}");
//         }
//     }
//
//     static GameHabitTimeHelper()
//     {
//         _onlineTime = 0;
//         InitDic();
//     }
//
//     private static void InitDic()
//     {
//         _persistenceHabitTime = new PersistenceData<string>("GameHabitTimeHelper_persistenceHabitTime", "");
//
//         if (string.IsNullOrEmpty(_persistenceHabitTime.Value))
//         {
//             _persistenceHabitTime.Value = DefaultValue();
//         }
//
//         _hourToTime = new Dictionary<int, int>();
//         char[] chars = _persistenceHabitTime.Value.ToCharArray();
//         for (int i = 0; i < chars.Length; i++)
//         {
//             int hourTime = int.Parse(chars[i].ToString());
//             _hourToTime.Add(i, hourTime);
//         }
//     }
//
//     private static void Save()
//     {
//         LDebug.Log("GameHabit", "Save GameHabitTimeHelper string " + GetHourString().ToString());
//         _persistenceHabitTime.Value = GetHourString().ToString();
//     }
//
//     private static string DefaultValue()
//     {
//         StringBuilder builder = new StringBuilder();
//         for (int i = 0; i < 24; i++)
//         {
//             builder.Append(0);
//         }
//
//         return builder.ToString();
//     }
//
//     /// <summary>
//     /// 判断今天的几点有没有设置过活跃度
//     /// </summary>
//     /// <param name="hour">几点</param>
//     /// <returns></returns>
//     private static bool HasSet(int hour)
//     {
//         DateTime now = DateTime.UtcNow;
//         int timestamp =
//             DataFormater.ConvertDateTimeToTimeStamp(new DateTime(now.Year, now.Month,
//                 now.Day));
//         return Storage.Instance.GetBool($"GameHabit_Set_{timestamp}_{hour}", false);
//     }
//
//     private static void SaveSetHour(int hour)
//     {
//         DateTime now = DateTime.UtcNow;
//         int timestamp =
//             DataFormater.ConvertDateTimeToTimeStamp(new DateTime(now.Year, now.Month,
//                 now.Day));
//         Storage.Instance.SetBool($"GameHabit_Set_{timestamp}_{hour}", true);
//     }
//
//     private static string GetHourString()
//     {
//         StringBuilder builder = new StringBuilder();
//         foreach (KeyValuePair<int, int> pair in _hourToTime)
//         {
//             builder.Append(pair.Value);
//         }
//
//         return builder.ToString();
//     }
//
//     //24个小时，每个小时对应玩家在线的次数
//     private static Dictionary<int, int> _hourToTime;
//     private static PersistenceData<string> _persistenceHabitTime;
// }