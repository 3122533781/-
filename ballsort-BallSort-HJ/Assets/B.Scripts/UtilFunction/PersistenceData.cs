using System;
using UnityEngine;

public class PersistenceData<T> where T : IConvertible
{
    public event Action<T, T> OnValueChange = delegate { };

    public T Value
    {
        get => _value;
        set
        {
            T old = _value;
            _value = value;
            Save();
            OnValueChange.Invoke(old, value);
        }
    }


    public PersistenceData(string key, T defaultValue)
    {
        _key = key;
        CheckDataValid();
        if (Storage.Instance.HasKey(GetKey()))
        {
            Read(defaultValue);
        }
        else
        {
            _value = defaultValue;
            Save();
        }
    }

    private void Save()
    {
        Type type = typeof(T);
        if (type == typeof(int))
        {
            int intValue = (int) Convert.ChangeType(_value, typeof(int));
            Storage.Instance.SetInt(GetKey(), intValue);
        }
        else if (type == typeof(bool))
        {
            bool boolValue = (bool) Convert.ChangeType(_value, typeof(bool));
            Storage.Instance.SetBool(GetKey(), boolValue);
        }
        else if (type == typeof(string))
        {
            string stringValue = (string) Convert.ChangeType(_value, typeof(string));
            Storage.Instance.SetString(GetKey(), stringValue);
        }
        else if (type == typeof(float))
        {
            float floatValue = (float) Convert.ChangeType(_value, typeof(float));
            Storage.Instance.SetFloat(GetKey(), floatValue);
        }
    }

    private void Read(T defaultValue)
    {
        Type type = typeof(T);
        if (type == typeof(int))
        {
            int intValue = Storage.Instance.GetInt(GetKey(), 0);
            _value = (T) Convert.ChangeType(intValue, typeof(T));
        }
        else if (type == typeof(bool))
        {
            bool boolValue = Storage.Instance.GetBool(GetKey(), false);
            _value = (T) Convert.ChangeType(boolValue, typeof(T));
        }
        else if (type == typeof(string))
        {
            string stringValue = Storage.Instance.GetString(GetKey(), "");
            _value = (T) Convert.ChangeType(stringValue, typeof(T));
        }
        else if (type == typeof(float))
        {
            float floatValue = Storage.Instance.GetFloat(GetKey(), 0);
            _value = (T) Convert.ChangeType(floatValue, typeof(T));
        }
    }

    private string GetKey()
    {
        return $"PersistenceData_{typeof(T)}_{_key}";
    }

    private void CheckDataValid()
    {
        Type type = typeof(T);
        if (type == typeof(int) || type == typeof(bool) ||
            type == typeof(string) || type == typeof(float))
        {
        }
        else
        {
            Debug.LogError(type + " is not support type.");
        }
    }

    private T _value;
    private string _key;
}