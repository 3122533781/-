using UnityEngine;

public static class LDebug
{
    public static void Log(string debug)
    {
        if (LogSwitcher.Instance.Open)
        {
            Log("Debug", debug);
        }
    }

    public static void Log(string title, string debug)
    {
        if (LogSwitcher.Instance.Open)
        {
            Debug.Log($"<color=00FF2FF>{title}:</color> {debug}");
        }
    }

    public static void LogError(string title, string debug)
    {
        if (LogSwitcher.Instance.Open)
        { //网络错误需要修复
          //   Debug.LogError($"<color=00FF2FF>{title}:</color> {debug}");
        }
    }

    public static void LogWarning(string title, string debug)
    {
        if (LogSwitcher.Instance.Open)
        {
            Debug.LogWarning($"<color=FFF700>{title}:</color> {debug}");
        }
    }
}