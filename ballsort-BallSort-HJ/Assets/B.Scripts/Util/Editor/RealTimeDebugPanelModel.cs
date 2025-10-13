using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class RealTimeDebugPanelModel
{
    public UnityEngine.Object CurrentObject { get; set; }

    public Component[] Components { get; private set; }
    public GUIContent[] Labels { get; private set; }

    public Dictionary<string, bool> IdToIsFoldOut { get; set; }
    public Dictionary<string, float> PathToChangeTime { get; set; }

    public List<string> AllDisplayObjectsPath { get; private set; }
    public List<float> AllDisplayObjectChangeTime { get; private set; }
    public List<object> AllDisplayObjects { get; private set; }

    public void DrawOneView(int indentLevel, string title,object value)
    {
        string path = string.Format("{0}/{1}", indentLevel, title);
        if (!AllDisplayObjectsPath.Contains(path))
        {
            AllDisplayObjectsPath.Add(path);
            AllDisplayObjectChangeTime.Add(0f);
            AllDisplayObjects.Add(value);
        }
        else
        {
            int index = AllDisplayObjectsPath.IndexOf(path);

            if (AllDisplayObjects[index] == null || !AllDisplayObjects[index].Equals(value))
            {
                AllDisplayObjectChangeTime[index] = Time.time;
            }
        }
    }

    public bool IsViewChanging(int indentLevel, string title)
    {
        string path = string.Format("{0}/{1}", indentLevel, title);
        if (!AllDisplayObjectsPath.Contains(path))
        {
            return false;
        }

        int index = AllDisplayObjectsPath.IndexOf(path);

        return AllDisplayObjectChangeTime[index] + 2 > Time.time;
    }

    public bool GetIsFoldout(int indentLevel, string title)
    {
        string key = string.Format("{0}_{1}", indentLevel, title);
        if (!IdToIsFoldOut.ContainsKey(key))
        {
            IdToIsFoldOut.Add(key, false);
        }
        return IdToIsFoldOut[key];
    }

    public void SetIsFoldout(int indentLevel, string title, bool value)
    {
        string key = string.Format("{0}_{1}", indentLevel, title);

        IdToIsFoldOut[key] = value;
    }

    public void InitComponents(Component[] data)
    {
        Components = data;
        Labels = Components.Select(t => new GUIContent(t.GetType().ToString())).ToArray();
    }

    public RealTimeDebugPanelModel()
    {
        IdToIsFoldOut = new Dictionary<string, bool>();
        PathToChangeTime = new Dictionary<string, float>();

        AllDisplayObjectsPath = new List<string>();
        AllDisplayObjectChangeTime = new List<float>();
        AllDisplayObjects = new List<object>();
    }

    public void Reset()
    {
        AllDisplayObjectsPath.Clear();
        AllDisplayObjectChangeTime.Clear();
        AllDisplayObjects.Clear();
    }

    private int _currentDrawDataCount;
}