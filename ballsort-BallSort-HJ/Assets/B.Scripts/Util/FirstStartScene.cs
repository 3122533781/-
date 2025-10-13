
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class FirstStartScene : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(_nextSceneName);
#if UNITY_EDITOR
        Game.Instance.isSDKInitCompleted = true;
#else
  Game.Instance.isSDKInitCompleted = true;
        //GHGYSDK.Instance.Init((success, info) =>
        //{
        //  // GHGYSDK.Instance.ShowMaxTestUI();
        //    Debug.Log(success ? "SDK 初始化成功" : "SDK 初始化失败");
        //    if (success)
        //    {
        //        Game.Instance.isSDKInitCompleted = true;
        //        InitCallBacks();
        //        FetchArkConfig();
        //    }
        //});
#endif
    }

    //public static void FetchArkConfig()
    //{
    //    string bizCodes = "Usergrouping";
    //    GHGYSDK.Instance.FetchArkConfig(bizCodes, ((GetConfigSuccess, configs) =>
    //    {
    //        Debug.Log($"FetchArkConfig success:{GetConfigSuccess}");
    //        if (GetConfigSuccess)
    //        {
    //            foreach (var abData in configs)
    //            {
    //                // abData.vid 当前配置的唯一标识
    //                foreach (var kv in abData.fields)
    //                {
    //                    Debug.Log($"FetchArkConfig fkey:{kv.fkey}, fvalue:{kv.fvalue}");

    //                    if (kv.fkey.Equals(bizCodes))
    //                    {
    //                        try
    //                        {
    //                            // 解析 fvalue 为 JSON 对象
    //                            JObject config = JObject.Parse(kv.fvalue);

    //                            // 获取 "ins" 的值
    //                            var insValue = config["ShowAdInterval"]?.Value<int>() ?? 99;
    //                            Game.Instance.GetSystem<RemoteControlSystem>().InsertAdShowTime.Value = (float)insValue;
    //                            Debug.Log($"FetchArkConfig InsertAdShowTime Value: {Game.Instance.GetSystem<RemoteControlSystem>().InsertAdShowTime.Value}");
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            Debug.LogError($"Error parsing JSON: {ex.Message}");
    //                        }

    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError("Failed to fetch config.");
    //        }
    //    }));
    //}

    private void InitCallBacks()
    {
        //Debug.Log("Init callbacks");
        //Events.onAdShowEvent += (show) =>
        //{
        //    Debug.Log($"Ad show trigger BGM {!show}");
        //    AudioManager.Instance.Model.IsBgmOpen = !show;
        //};
    }

    //private void FetchArkConfigOld()
    //{
    //    GHGYSDK.Instance.FetchArkConfig("Usergrouping", ((success, configs) =>
    //    {
    //        if (success)
    //        {
    //            Debug.Log($"Fetch Ark Config Success===>>>");
    //            foreach (var abData in configs)
    //            {
    //                // abData.vid 当前配置的唯一标识
    //                foreach (var kv in abData.fields)
    //                {
    //                    if (kv.fkey.Equals("Usergrouping"))
    //                    {
    //                        Debug.Log($"========FetchArkConfig===Usergrouping==={kv.fvalue}===>>>>>>");
    //                        if (!string.IsNullOrEmpty(kv.fvalue))
    //                        {
    //                            ArkConfig arkConfig = JsonUtility.FromJson<ArkConfig>(kv.fvalue);
    //                            if (arkConfig != null)
    //                            {
    //                                Debug.Log($"========FetchArkConfig===ShowAdInterval==={arkConfig.ShowAdInterval}===>>>>>>");
    //                                Game.Instance.GetSystem<RemoteControlSystem>().InsertAdShowTime.Value = float.Parse(arkConfig.ShowAdInterval);
    //                                Debug.Log($"========ShowAdInterval.value==={Game.Instance.GetSystem<RemoteControlSystem>().InsertAdShowTime.Value}===>>>>>>");
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }));
    //}

    [SerializeField] private string _nextSceneName = "Home";

    public class ArkConfig
    {
        public string ShowAdInterval;
    }
}