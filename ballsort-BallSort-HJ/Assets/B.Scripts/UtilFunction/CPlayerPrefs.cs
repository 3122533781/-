using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class CPlayerPrefs
{
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SetIntList(string key, List<int> numbers)
    {
        if (numbers == null || numbers.Count == 0)
        {
            PlayerPrefs.DeleteKey(key);
            return;
        }

        var value = string.Join(",", numbers);
        PlayerPrefs.SetString(key, value);
    }

    public static List<int> GetIntList(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return new List<int>();
        }

        var value = PlayerPrefs.GetString(key);
        return string.IsNullOrEmpty(value) ? new List<int>() : value.Split(',').Select(int.Parse).ToList();
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        return HasKey(key) ? PlayerPrefs.GetInt(key) : defaultValue;
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void SetLong(string key, long value)
    {
        PlayerPrefs.SetString(key, value.ToString());
    }

    public static long GetLong(string key)
    {
        if (PlayerPrefs.GetString(key) == "")
        {
            return -1;
        }
        return long.Parse(PlayerPrefs.GetString(key));
    }

    public static void AddInt(string key, int value = 1)
    {
        int old = GetInt(key);
        SetInt(key, old + value);
    }

    public static float GetFloat(string key, float defaultValue = 0.0f)
    {
        return HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static string GetString(string key, string defaultValue = null)
    {
        return HasKey(key) ? PlayerPrefs.GetString(key) : defaultValue;
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        return HasKey(key) ? 1 == PlayerPrefs.GetInt(key) : defaultValue;
    }

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }
}