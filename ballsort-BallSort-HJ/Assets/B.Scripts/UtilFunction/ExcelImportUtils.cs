using System;
using System.Collections.Generic;
using System.Linq;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;
using UnityEditor;
using UnityEngine;

public class ExcelImportUtils
{
    //[1,150,10]
    public static Vector3Int ParseVector3Int(string value)
    {
        string vString = value.Replace("[", "").Replace("]", "");
        string[] itemSplits = vString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        return new Vector3Int(int.Parse(itemSplits[0]), int.Parse(itemSplits[1]), int.Parse(itemSplits[2]));
    }

    //[1,150,10]&[201,208,5]&[211,218,3]&[221,228,8]&[511,526,1]&[551,566,2]
    public static List<Vector3Int> ParseVector3IntList(string value)
    {
        string vString = value.Replace("[", "").Replace("]", "");

        string[] splits = vString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

        List<Vector3Int> result = new List<Vector3Int>();
        for (int i = 0; i < splits.Length; i++)
        {
            string item = splits[i];
            string[] itemSplits = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            result.Add(new Vector3Int(int.Parse(itemSplits[0]), int.Parse(itemSplits[1]), int.Parse(itemSplits[2])));
        }

        return result;
    }

    //[0,1]
    public static Vector2Int ParseVector2Int(string value)
    {
        if (value == null || value == "")
        {
            return new Vector2Int();
        }

        string vString = value.Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "");

        string[] splits = vString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        int intOne = int.Parse(splits[0]);
        int intTwo = int.Parse(splits[1]);

        return new Vector2Int(intOne, intTwo);
    }

    //1&3&52&   数字以 & 分隔
    public static List<int> ParseToIntList(string value)
    {
        List<int> result = new List<int>();
        string[] splits = value.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < splits.Length; i++)
        {
            int num = int.Parse(splits[i]);
            result.Add(num);
        }

        return result;
    }

    public static List<RewardData> ParseToRewardDataList(string value)
    {
        List<RewardData> results = new List<RewardData>();
        List<Vector2Int> items = ParseToVector2List(value);

        for (int i = 0; i < items.Count; i++)
        {
            RewardData data = new RewardData((GoodType)items[i].x, items[i].y);
            results.Add(data);
        }

        return results;
    }

    //[1,10]&[1,15]
    public static List<Vector2Int> ParseToVector2List(string value)
    {
        if (value == "-1")
        {
            return new List<Vector2Int>();
        }

        string vString = value.Replace("[", "").Replace("]", "");

        string[] splits = vString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

        List<Vector2Int> result = new List<Vector2Int>();
        for (int i = 0; i < splits.Length; i++)
        {
            string[] items = splits[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //            Debug.Log(splits[i]);
            Vector2Int pair = new Vector2Int();
            pair.x = int.Parse(items[0]);
            pair.y = int.Parse(items[1]);
            result.Add(pair);
        }

        return result;
    }

    public static List<Vector3> ParseToVector3List(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Trim() == "-1")
        {
            return new List<Vector3>();
        }

        string vString = value.Replace("[", "").Replace("]", "");

        string[] splits = vString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < splits.Length; i++)
        {
            string[] items = splits[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Vector3 pair = new Vector3();
            pair.x = float.Parse(items[0]);
            pair.y = float.Parse(items[1]);
            pair.z = float.Parse(items[2]);
            result.Add(pair);
        }

        return result;
    }

    // public static List<Vector3Int> ParseToVector3List(string value)
    // {
    //     if (value == "-1")
    //     {
    //         return new List<Vector3Int>();
    //     }
    //
    //     string vString = value.Replace("[", "").Replace("]", "");
    //
    //     string[] splits = vString.Split(new char[] {'&'}, StringSplitOptions.RemoveEmptyEntries);
    //
    //     List<Vector3Int> result = new List<Vector3Int>();
    //     for (int i = 0; i < splits.Length; i++)
    //     {
    //         string[] items = splits[i].Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
    //         Vector3Int pair = new Vector3Int();
    //         pair.x = int.Parse(items[0]);
    //         pair.y = int.Parse(items[1]);
    //         pair.z = int.Parse(items[2]);
    //         result.Add(pair);
    //     }
    //
    //     return result;
    // }

    //[aaaaa,bbbb,cccc,dddd]
    public static List<string> ParseStrings(string value)
    {
        string vString = value.Replace("[", "").Replace("]", "");

        string[] splits = vString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        return splits.ToList();
    }

    //[0,2,1.3,0]
    public static List<float> Parsefloats(string value)
    {
        List<float> result = new List<float>();
        string vString = value.Replace("[", "").Replace("]", "");

        string[] splits = vString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < splits.Length; i++)
        {
            float floatValue = float.Parse(splits[i]);
            result.Add(floatValue);
        }

        return result;
    }

    public static List<int> ParseInts(string value)
    {
        List<int> result = new List<int>();
        string vString = value.Replace("[", "").Replace("]", "");

        string[] splits = vString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < splits.Length; i++)
        {
            int intValue = int.Parse(splits[i]);
            result.Add(intValue);
        }

        return result;
    }

    //[(0,1),(1,0),(1,1),(1,2),(2,1)]
    public static List<Vector2Int> ParseVector2Ints(string value)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        string vString = value.Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "");

        string[] splits = vString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < splits.Length; i++)
        {
            int intOne = int.Parse(splits[i]);
            int intTwo = int.Parse(splits[i + 1]);
            Vector2Int vector = new Vector2Int(intOne, intTwo);
            result.Add(vector);
            i++;
        }

        return result;
    }

    public static T ParseTo<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return default(T);
        }

        Type type = typeof(T);
        try
        {
            if (type == typeof(int))
            {
                return (T)Convert.ChangeType(value, typeof(int));
            }
            else if (type == typeof(bool))
            {
                bool parseValue = int.Parse(value) == 1;
                return (T)Convert.ChangeType(parseValue, typeof(bool));
            }
            else if (type == typeof(string))
            {
                return (T)Convert.ChangeType(value, typeof(string));
            }
            else if (type == typeof(float))
            {
                return (T)Convert.ChangeType(value, typeof(float));
            }
        }
        catch
        {
            Debug.Log(value);
        }

        return default;
    }

#if UNITY_EDITOR

    public static Sprite ParserToSprite(string path)
    {
        Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sp == null)
        {
            Debug.LogError("Parser to sprite failed path:" + path);
        }

        return sp;
    }

#endif

    public static Dictionary<S, K> ParserToDictionary<S, K>(string value)
    {
        Dictionary<S, K> result = new Dictionary<S, K>();
        string[] splits = value.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var t in splits)
        {
            string vString = t.Replace("[", "").Replace("]", "");
            for (int i = 0; i < vString.Length; i++)
            {
                string[] items = splits[i].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                S item1 = ParseTo<S>(items[0]);
                K item2 = ParseTo<K>(items[1]);
                result.Add(item1, item2);
            }
        }

        return result;
    }

    //[0.5,1.2]
    public static Vector2 ParseVector2(string value)
    {
        string vString = value.Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "");

        string[] splits = vString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        float intOne = Convert.ToSingle(splits[0]);
        float intTwo = Convert.ToSingle(splits[1]);

        return new Vector2(intOne, intTwo);
    }
}