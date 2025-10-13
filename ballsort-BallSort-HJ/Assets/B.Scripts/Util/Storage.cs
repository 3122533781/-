using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage
{
    private static Storage _instance;

    public static Storage Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Storage();
            }

            return _instance;
        }
    }

    public string GetString(string key, string defaultValue = "")
    {
        return HasKey(key) ? PlayerPrefs.GetString(key) : defaultValue;
    }

    public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public float GetFloat(string key, float defaultValue = 0)
    {
        return HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
    }

    public void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        return HasKey(key) ? PlayerPrefs.GetInt(key) : defaultValue;
    }

    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public void SetLong(string key, long value)
    {
        PlayerPrefs.SetString(key, value.ToString());
    }

    public long GetLong(string key)
    {
        if (PlayerPrefs.GetString(key) == "")
        {
            return -1;
        }
        return long.Parse(PlayerPrefs.GetString(key));
    }

    public bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
    }

    public void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public void SetList(string key, List<int> value)
    {
        string result = null;
        for (int i = 0; i < value.Count; i++)
        {
            if (i < value.Count - 1)
            {
                result += $"{value[i]},";
            }
            else
            {
                result += $"{value[i]}";
            }
        }

        if (string.IsNullOrEmpty(result)) return;
        PlayerPrefs.SetString(key, result);
    }

    public List<int> GetList(string key)
    {
        string[] temp = PlayerPrefs.GetString(key).Split(',');
        List<int> result = new List<int>();
        if (string.IsNullOrEmpty(temp[0])) return null;
        foreach (var t in temp)
        {
            result.Add(int.Parse(t));
        }

        return result;
    }

    public void SetIntList(string key, List<int> numbers)
    {
        if (numbers == null || numbers.Count == 0)
        {
            PlayerPrefs.DeleteKey(key);
            return;
        }

        var value = string.Join(",", numbers);
        PlayerPrefs.SetString(key, value);
    }

    public List<int> GetIntList(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return new List<int>();
        }

        var value = PlayerPrefs.GetString(key);
        return string.IsNullOrEmpty(value) ? new List<int>() : value.Split(',').Select(int.Parse).ToList();
    }

    public void DeleteKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }
}