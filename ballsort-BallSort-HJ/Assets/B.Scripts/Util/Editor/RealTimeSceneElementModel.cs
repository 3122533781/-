using Fangtang;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeSceneElementModel
{
    public List<SceneElement> AllElement { get; private set; }

    public List<bool> AllElementFoldoutBool { get; set; }

    public void ResetAllElement(IEnumerable<SceneElement> data)
    {
        AllElement.Clear();
        AllElement.AddRange(data);

        while (AllElementFoldoutBool.Count < AllElement.Count)
        {
            AllElementFoldoutBool.Add(false);
        }

        while (AllElementFoldoutBool.Count > AllElement.Count)
        {
            AllElementFoldoutBool.RemoveAt(AllElementFoldoutBool.Count - 1);
        }
    }

    public RealTimeSceneElementModel()
    {
        AllElement = new List<SceneElement>();
        AllElementFoldoutBool = new List<bool>();
    }
}