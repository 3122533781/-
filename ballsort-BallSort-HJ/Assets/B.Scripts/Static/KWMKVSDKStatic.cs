using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KWMKVSDKStatic : IStatic
{
    public void LogEvent(string name)
    {
      //  GHGYSDK.Instance.Track(name);
    }

    public void LogEvent(string name, string parameterName, int value)
    {
        //Dictionary<string, object>
        //    eventValues = new Dictionary<string, object>();
        //eventValues.Add(parameterName, value);
        //GHGYSDK.Instance.Track(name, eventValues);
    }

    public void LogEvent(string name, string parameterName, string value)
    {
        //Dictionary<string, object>
        //    eventValues = new Dictionary<string, object>();
        //eventValues.Add(parameterName, value);
        //GHGYSDK.Instance.Track(name, eventValues);
    }

    public void LogEvent(string name, params StaticParameter[] parameters)
    {
        //Dictionary<string, object>
        //    eventValues = new Dictionary<string, object>();
        //for (int i = 0; i < parameters.Length; i++)
        //{
        //    eventValues.Add(parameters[i].Name, parameters[i].GetString());
        //}

        //GHGYSDK.Instance.Track(name, eventValues);
    }

    public void BeginStage(string stageId)
    {
        //Dictionary<string, object>
        //    eventValues = new Dictionary<string, object>();
        //eventValues.Add("stageId", stageId);
        //GHGYSDK.Instance.Track("BeginStage", eventValues);
    }

    public void CompletedStage(string stageId)
    {
        //Dictionary<string, object>
        //    eventValues = new Dictionary<string, object>();
        //eventValues.Add("stageId", stageId);
        //GHGYSDK.Instance.Track("CompletedStage", eventValues);
    }

    public void FailedStage(string stageId, string cause)
    {
        //Dictionary<string, object>
        //    eventValues = new Dictionary<string, object>();
        //eventValues.Add("stageId", stageId);
        //GHGYSDK.Instance.Track("FailedStage", eventValues);
    }

    public void LevelUp(int level)
    {
    }

    public void SetAccount(string accountId)
    {
    }

    public void SourceCurrency(string currency, float amount, string itemType, string itemId)
    {
    }

    public void SinkCurrency(string currency, float amount, string itemType, string itemId)
    {
    }
}