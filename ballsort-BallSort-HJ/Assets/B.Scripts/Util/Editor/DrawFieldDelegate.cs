using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public static class DrawNullField
{
    public static void DrawNormalField(string title, object value, Type type)
    {
        if (value != null)
        {
            EditorGUILayout.LabelField(title,
                "未识别类型 Type =" + type.ToString(), new GUIStyle()
                {
                    fontStyle = FontStyle.Bold,
                    normal = new GUIStyleState() { textColor = Color.red }
                });
        }
    }
}
public static class DrawEnumField
{
    public static void DrawNormalField(string title, object value, Type type)
    {
        EditorGUILayout.TextField(title, value.ToString());
    }
}

public static class DrawCustomClassParameter
{
    public static void DrawNormalField(string title, object value, Type type)
    {
        bool isFoldout = DrawFieldDelegate.DrawFoldOut(title, ": (Custom class)");

        if (isFoldout)
        {
            EditorGUI.indentLevel++;
            FieldInfoDawer.DrawProperty(value);
            EditorGUI.indentLevel--;
        }
    }
}

public static class DrawIList
{
    public static void DrawNormalField(string title, object value, Type type)
    {
        bool isFoldout = DrawFieldDelegate.DrawFoldOut(title, ": (list)");

        if (isFoldout)
        {
            IList list = value as IList;
            if (list != null && list.Count > 0)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == null)
                    {
                        EditorGUILayout.LabelField("null");
                    }
                    else
                    {
                        DrawFieldDelegate.Draw(i.ToString(), list[i], list[i].GetType());
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}

public static class DrawDictionary
{
    public static void DrawNormalField(string title, object value, Type type)
    {
        bool isFoldout = DrawFieldDelegate.DrawFoldOut(title, ": (Dic) ");

        if (isFoldout)
        {
            IDictionary dictionary = value as IDictionary;

            if (dictionary != null && dictionary.Count > 0)
            {
                EditorGUI.indentLevel++;
                foreach (DictionaryEntry item in dictionary)
                {
                    EditorGUILayout.LabelField(string.Format("key: {0}", item.Key));

                    if (item.Value == null)
                    {
                        EditorGUILayout.LabelField("null");
                    }
                    else
                    {
                        DrawFieldDelegate.Draw(item.Value.ToString(), item.Value, item.Value.GetType());
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}

public static class DrawFieldDelegate
{
    public static bool DrawFoldOut(string title, string extend)
    {
        bool isFoldout = RealTimeDebugPanel.Model.GetIsFoldout(EditorGUI.indentLevel, title);
        //bool isFoldout = RealTimeDebugPanel.HomeModel.IsFoldOut[RealTimeDebugPanel.HomeModel.CurrentDrawDataCount];
        bool newIsFoldout = EditorGUILayout.Foldout(isFoldout, title + extend);

        if (newIsFoldout != isFoldout)
        {
            RealTimeDebugPanel.Model.SetIsFoldout(EditorGUI.indentLevel, title, newIsFoldout);
            //RealTimeDebugPanel.HomeModel.IsFoldOut[RealTimeDebugPanel.HomeModel.CurrentDrawDataCount] = newIsFoldout;
        }

        return newIsFoldout;
    }

    public static void Draw(string title, object value, Type type)
    {
        RealTimeDebugPanel.Model.DrawOneView(EditorGUI.indentLevel, title, value);

        GUI.color = RealTimeDebugPanel.Model.IsViewChanging(EditorGUI.indentLevel, title) ? Color.green : Color.white;
        if (typeof(System.Single).IsAssignableFrom(type))
        {
            EditorGUILayout.FloatField(title, (float)value);
        }
        else if (typeof(System.String).IsAssignableFrom(type))
        {
            EditorGUILayout.TextField(title, (string)value);
        }
        else if (typeof(System.Int32).IsAssignableFrom(type))
        {
            EditorGUILayout.IntField(title, (int)value);
        }
        else if (typeof(Vector3).IsAssignableFrom(type))
        {
            EditorGUILayout.Vector3Field(title, (Vector3)value);
        }
        else if (typeof(Vector2).IsAssignableFrom(type))
        {
            EditorGUILayout.Vector2Field(title, (Vector2)value);
        }
        else if (typeof(Vector4).IsAssignableFrom(type))
        {
            EditorGUILayout.Vector4Field(title, (Vector4)value);
        }
        else if (typeof(System.Boolean).IsAssignableFrom(type))
        {
            EditorGUILayout.Toggle(title, (System.Boolean)value);
        }
        else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
        {
            EditorGUILayout.ObjectField(title, (UnityEngine.Object)value,
                typeof(UnityEngine.Object), true);
        }
        else if (typeof(IList).IsAssignableFrom(type))
        {
            DrawIList.DrawNormalField(title, value, type);
        }
        else if (typeof(System.Enum).IsAssignableFrom(type))
        {
            DrawEnumField.DrawNormalField(title, value, type);
        }
        else if (typeof(IDictionary).IsAssignableFrom(type))
        {
            DrawDictionary.DrawNormalField(title, value, type);
        }
        else
        {
            DrawCustomClassParameter.DrawNormalField(title, value, type);

            //DrawNullField.DrawNormalField(title, value, type);
        }
        GUI.backgroundColor = Color.white;
    }
}