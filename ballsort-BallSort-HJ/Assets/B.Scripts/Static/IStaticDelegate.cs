using System.Collections.Generic;

public static class IStaticDelegate
{
    public static bool IsOpen = true;

    public static void LogEvent(string name)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].LogEvent(name);
        }
    }

    public static void LogEvent(string name, string parameterName, int value)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].LogEvent(name, parameterName, value);
        }
    }

    public static void LogEvent(string name, string parameterName, string value)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].LogEvent(name, parameterName, value);
        }
    }
    
    public static void LogEvent(string name, params StaticParameter[] parameters)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].LogEvent(name, parameters);
        }
    }

    public static void BeginStage(string stageId)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].BeginStage(stageId);
        }
    }

    public static void CompletedStage(string stageId)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].CompletedStage(stageId);
        }
    }

    public static void FailedStage(string stageId, string cause)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].FailedStage(stageId, cause);
        }
    }

    public static void LevelUp(int level)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].LevelUp(level);
        }
    }

    public static void SetAccount(string accountId)
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < _statics.Count; i++)
        {
            _statics[i].SetAccount(accountId);
        }
    }

    static IStaticDelegate()
    {
        _statics = new List<IStatic>();
#if UNITY_EDITOR
        _statics.Add(new EditorStatic());
#else
       // _statics.Add(new FireBaseStatic());
        // _statics.Add(new TalkingDataStatic());
       // _statics.Add(new FaceBookStatic());
        //_statics.Add(new AppsFlyerStatic());
#endif
    }

    private static List<IStatic> _statics;
}