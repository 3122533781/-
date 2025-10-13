public interface IStatic
{
    void LogEvent(string name);
    void LogEvent(string name, string parameterName, int value);
    void LogEvent(string name, string parameterName, string value);
    void LogEvent(string name, params StaticParameter[] parameters);
    void BeginStage(string stageId);
    void CompletedStage(string stageId);
    void FailedStage(string stageId, string cause);
    void LevelUp(int level);

    void SetAccount(string accountId);
}