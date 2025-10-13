using System;
using System.Globalization;
using System.Text;
using UnityEngine;
#pragma warning disable 618
using static System.TimeZone;

#pragma warning restore 618

public static class DataFormater
{
    public static int GetDayRefreshTime(int timeStamp, int dayBeginHour)
    {
        DateTime createTime = ConvertTimeStampToDateTime(timeStamp);
        int refreshHour = dayBeginHour < 0 ? dayBeginHour + 24 : dayBeginHour;
        DateTime expiredTime = createTime.Date;
        if (createTime.Hour >= refreshHour)
        {
            expiredTime = expiredTime.AddDays(1);
        }

        expiredTime = expiredTime.AddHours(refreshHour);
        return ConvertDateTimeToTimeStamp(expiredTime);
    }

    /// <summary>
    /// 以 星期一的 dayBeginHour 为刷新时间
    /// 计算从 timeStamp 开始到什么时间点需要刷新
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <param name="dayBeginHour"></param>
    /// <returns></returns>
    public static int GetWeekRefreshTime(int timeStamp, int dayBeginHour)
    {
        DateTime createTime = ConvertTimeStampToDateTime(timeStamp);
        int refreshHour = dayBeginHour < 0 ? dayBeginHour + 24 : dayBeginHour;
        DateTime expiredTime = createTime.Date;

        if (createTime.DayOfWeek == DayOfWeek.Monday)
        {
            if (createTime.Hour >= refreshHour)
            {
                expiredTime = expiredTime.AddDays(7);
            }

            expiredTime = expiredTime.AddHours(refreshHour);
        }
        else
        {
            int dayOfWeek = (int) (createTime.DayOfWeek + 6) % 7 + 1;
            expiredTime = expiredTime.AddDays(8 - dayOfWeek);
            expiredTime = expiredTime.AddHours(refreshHour);
        }

        return ConvertDateTimeToTimeStamp(expiredTime);
    }

    public static int ConvertDateTimeToTimeStamp(DateTime time)
    {
        DateTime startTime = CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        return (int) (time - startTime).TotalSeconds;
    }

    public static DateTime ConvertTimeStampToDateTime(int timeStamp)
    {
        DateTime dtStart = CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    public static string FormatDigitalClock(long timestamp)
    {
        TimeSpan span = TimeSpan.FromSeconds(timestamp);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", Mathf.FloorToInt((float) span.TotalHours), span.Minutes,
            span.Seconds);
    }

    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");
        hex = hex.Replace("#", "");
        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        }

        return new Color32(r, g, b, a);
    }

    public static string ToTimeTitleString(int secondSum)
    {
        TimeSpan span = TimeSpan.FromSeconds(secondSum);
        return ToTimeTitleString(span);
    }

    public static string ToTimeTitleString(TimeSpan span)
    {
        if (span.Days >= 10)
        {
            return string.Format("{0:D2}Day {1:D2}Hour", span.Days, span.Hours);
        }
        else if (span.Days >= 1)
        {
            return string.Format("{0:D1}Day {1:D2}Hour", span.Days, span.Hours);
        }
        else if (span.Hours >= 10)
        {
            return string.Format("{0:D2}Hour {1:D2}Min", span.Hours, span.Minutes);
        }
        else if (span.Hours >= 1)
        {
            return string.Format("{0:D1}Hour {1:D2}Min", span.Hours, span.Minutes);
        }
        else if (span.Minutes >= 10)
        {
            return string.Format("{0:D2}Min", span.Minutes);
        }
        else if (span.Minutes >= 1)
        {
            return string.Format("{0:D1}Min", span.Minutes);
        }
        else
        {
            return string.Format("{0:D2}Decond", span.Seconds);
        }
    }

    public static string ToTimeString(int secondSum)
    {
        TimeSpan span = TimeSpan.FromSeconds(secondSum);
        return ToTimeString(span);
    }

    public static string ToTimeString(TimeSpan span)
    {
        if (span.Days >= 10)
        {
            return string.Format("{0:D2}d {1:D2}h", span.Days, span.Hours);
        }
        else if (span.Days >= 1)
        {
            return string.Format("{0:D1}d {1:D2}h", span.Days, span.Hours);
        }
        else
        {
            if (span.Hours >= 10)
            {
                return string.Format("{0:D2}:{1:D2}:{2:D2}", span.Hours, span.Minutes,
                    span.Seconds);
            }
            else if (span.Hours >= 1)
            {
                return string.Format("{0:D1}:{1:D2}:{2:D2}", span.Hours, span.Minutes,
                    span.Seconds);
            }
            else
            {
                if (span.Seconds >= 10)
                {
                    return string.Format("{0:D2}:{1:D2}", span.Minutes,
                        span.Seconds);
                }
                else
                {
                    return string.Format("{0:D2}:{1:D2}", span.Minutes,
                        span.Seconds);
                }
            }
        }
    }

    public static int GetDayTimeStamp(DateTime dateTime)
    {
        DateTime date = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);

        return ConvertDateTimeToTimeStamp(date);
    }
    
    public static int GetMonthTimeStamp(DateTime dateTime)
    {
        DateTime date = new DateTime(dateTime.Year, dateTime.Month, 1);

        return ConvertDateTimeToTimeStamp(date);
    }

    private const string FormatTimeSeperator = "'";

    /// <summary>
    /// 将Unix时间戳转换为时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime GetDateTime(long timeStamp)
    {
        DateTime startDt = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
        long lTime = timeStamp * 10000;
        TimeSpan toNow = new TimeSpan(lTime);
        return startDt.Add(toNow);
    }
}