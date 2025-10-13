using System.Collections.Generic;
using UnityEngine;

public static class DebugInfoDrawer
{
    public static void DrawLine(Vector3 from, Vector3 to, bool debugDraw)
    {
        if (debugDraw)
        {
            Debug.DrawLine(from, to);
        }
        else
        {
            Gizmos.DrawLine(from, to);
        }
    }

    /// template
    ///  DebugInfoDrawer.ButtonHorizontal(this.GetType().ToString(),);
    public static void ButtonHorizontal(string title, params object[] drawParams)
    {
        GUI.matrix = GUIMatrixProvider.Get();

        GUILayout.Space(GetSpace(title));
        float buttonHight = 50f;
        float buttonWedth = 150f;
        
        GUILayout.BeginHorizontal();
        GUI.color = Color.red;
        GUILayout.Label(title,GUILayout.Height(buttonHight));
        GUI.color = Color.white;
      
        for (int i = 0; i < drawParams.Length; i++)
        {
            GUILayout.Button(drawParams[i].ToString(), GUILayout.Width(buttonWedth), GUILayout.Height(buttonHight));
        }
        GUILayout.EndHorizontal();
    }

    private static float GetSpace(string title)
    {
        if (_titleToSpace == null)
        {
            _titleToSpace = new Dictionary<string, float>();
        }

        if (_titleToSpace.ContainsKey(title))
        {
            return _titleToSpace[title];
        }
        else
        {
            int lineCount = _titleToSpace.Count;
            float space = DefaultSpaceY + AddtiveSpace * lineCount;
            _titleToSpace.Add(title, space);
            return space;
        }
    }

    public static Dictionary<string, float> _titleToSpace;

    public static float DefaultSpaceY = 50f;
    public static float AddtiveSpace = 25f;
}