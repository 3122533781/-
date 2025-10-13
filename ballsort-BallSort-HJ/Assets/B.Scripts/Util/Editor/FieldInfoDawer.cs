using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;

public class FieldInfoDawer
{
    public static void DrawProperty(object target)
    {
        if (target == null)
            return;

        System.Type type = target.GetType();
        PropertyInfo[] properties = type.GetProperties();
        //.Where(t => typeof(UnityEngine.MonoBehaviour).IsAssignableFrom(t.GetType()))
        //.Where(t => typeof(UnityEngine.).IsAssignableFrom(t.GetType()))
        //.ToArray<PropertyInfo>();

        for (int i = 0; i < properties.Length; i++)
        {
            try
            {
                object value = properties[i].GetValue(target, null);
                if (value != null)
                {
                    DrawFieldDelegate.Draw(properties[i].Name, value, value.GetType());
                }
            }
#pragma warning disable  
            catch (System.Exception e)
#pragma warning restore 
            {

            }
        }
    }

    public static void DrawFields(UnityEngine.Object target)
    {
        Type type = target.GetType();
        IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

        SerializedObject serializedObject = new SerializedObject(target);
        foreach (FieldInfo field in fields)
        {
            FieldInfoDawer.DrawField(field, target, serializedObject);
        }
    }


    public static void DrawField(FieldInfo field, object target, SerializedObject serializedObject)
    {
        SerializedProperty property = serializedObject.FindProperty(field.Name);

        if (property != null)
        {
            EditorGUILayout.PropertyField(property, true);
        }
        else
        {
            FieldInfoDawer.DrawField(field, target);
        }
    }

    public static void DrawField(FieldInfo field, object target)
    {
        string title = field.Name;
        object obj = field.GetValue(target);
        DrawFieldDelegate.Draw(title, obj, field.FieldType);
    }
}
