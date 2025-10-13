using System;
using System.Collections.Generic;
//using LitJson;
using UnityEngine;

public class SoyProfileUnit
{
    public Dictionary<string, object> KeyToObj;

    //public string DicToJsonString()
    //{
    //  //  LitJson.JsonData origin = new JsonData();
    //    //foreach (string key in KeyToObj.Keys)
    //    //{
    //    //    if (bool.TryParse(KeyToObj[key].ToString(), out bool boolResult))
    //    //    {
    //    //        origin[key] = boolResult;
    //    //    }
    //    //    else if (int.TryParse(KeyToObj[key].ToString(), out int intResult))
    //    //    {
    //    //        origin[key] = intResult;
    //    //    }
    //    //    else if (float.TryParse(KeyToObj[key].ToString(), out float floatResult))
    //    //    {
    //    //        origin[key] = floatResult;
    //    //    }
    //    //    else
    //    //    {
    //    //        origin[key] = KeyToObj[key].ToString();
    //    //    }
    //    //}

    //    //return origin.ToJson();
    //}

    public T Get<T>(string key, T defaultValue = default(T))
    {
        if (KeyToObj.ContainsKey(key))
        {
            return ParseByJson<T>(KeyToObj[key]);
        }
        else
        {
            Debug.LogWarning($"You use key:{key} get profile is not exist");
            return defaultValue;
        }
    }

    public void Set<T>(string key, T value)
    {
        if (KeyToObj.ContainsKey(key))
        {
            KeyToObj[key] = value;
        }
        else
        {
            KeyToObj.Add(key, value);
        }
    }

    private static T ParseByJson<T>(object o)
    {
        if (o.GetType() != typeof(T))
        {
            Debug.LogError("Parse error. check type");
            return default(T);
        }

        return (T) Convert.ChangeType(o, typeof(T));
    }

    public SoyProfileUnit(string profileJson)
    {
        KeyToObj = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(profileJson))
        {
           // LitJson.JsonData data = JsonMapper.ToObject(profileJson);
            //foreach (string key in data.Keys)
            //{
            //    KeyToObj.Add(key, JsonToObj(data[key].ToString()));
            //}
        }
    }

    private object JsonToObj(string stringValue)
    {
        if (bool.TryParse(stringValue, out bool boolResult))
        {
            return boolResult;
        }
        else if (int.TryParse(stringValue, out int intResult))
        {
            return intResult;
        }
        else if (float.TryParse(stringValue, out float floatResult))
        {
            return floatResult;
        }
        else
        {
            return stringValue;
        }
    }
}