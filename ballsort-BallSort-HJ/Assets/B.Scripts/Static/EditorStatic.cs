public class EditorStatic : IStatic
{
    public void LogEvent(string name)
    {
        LDebug.Log("IStatic", name);
    }

    public void LogEvent(string name, string parameterName, int value)
    {
        LDebug.Log("IStatic", $"name:{name} parameter:{parameterName} int value:{value}");
    }

    public void LogEvent(string name, string parameterName, string value)
    {
        LDebug.Log("IStatic", $"name:{name} parameter:{parameterName} string value:{value}");
    }

    public void LogEvent(string name, params StaticParameter[] parameters)
    {
        LDebug.Log("IStatic", $"name:{name} parameter length:{parameters.Length} ");
    }

    public void BeginStage(string stageId)
    {
        LDebug.Log("IStatic", $"BeginStage {stageId}");
    }

    public void CompletedStage(string stageId)
    {
        LDebug.Log("IStatic", $"CompletedStage {stageId}");
    }

    public void FailedStage(string stageId, string cause)
    {
        LDebug.Log("IStatic", $"FailedStage {stageId}");
    }

    public void LevelUp(int level)
    {
        LDebug.Log("IStatic", $"LevelUp {level}");
    }

    public void SetAccount(string accountId)
    {
        LDebug.Log("IStatic", $"SetAccount {accountId}");
    }
}