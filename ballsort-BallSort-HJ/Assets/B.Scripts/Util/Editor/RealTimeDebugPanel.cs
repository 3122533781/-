using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

public class RealTimeDebugPanel : EditorWindow
{
    public static RealTimeDebugPanelModel Model
    {
        get
        {
            if (_panel == null)
            {
                _panel = EditorWindow.GetWindow<RealTimeDebugPanel>();
            }
            if (_panel._model == null)
            {
                _panel._model = new RealTimeDebugPanelModel();
            }

            return _panel._model;
        }
    }

    [MenuItem("SoyBean/Debug/RealTime Debug Panel %#&d")]
    public static void OpenDebugPanel()
    {
        EditorWindow.GetWindow<RealTimeDebugPanel>();
    }

    private void Update()
    {
        GameObject gameObject = Model.CurrentObject as GameObject;
        if (gameObject != null)
        {
            Model.InitComponents(gameObject.GetComponents<Component>());
        }

        if (Model.Components == null || _index >= Model.Components.Length)
        {
            _index = 0;
            Model.Reset();
        }

        Repaint();
    }

    private void OnGUI()
    {
        UnityEngine.Object obj = EditorGUILayout.ObjectField("Object : ", Model.CurrentObject,
            typeof(UnityEngine.Object), true);

        if (Model.CurrentObject != obj)
        {
            Model.CurrentObject = obj;
        }

        if (Model.Labels != null && Model.Components != null
             && Model.Components.Length > 0)
        {
            _index = EditorGUILayout.Popup(new GUIContent("Component:"), _index, Model.Labels);

            DrawCurrentDebugTarget(Model.Components[_index]);
        }
    }

    private void DrawCurrentDebugTarget(Component target)
    {
        if (target == null)
            return;

        _scrollView = EditorGUILayout.BeginScrollView(_scrollView);

        DrawProperty(target);
        DrawField(target);

        EditorGUILayout.EndScrollView();
    }

    private void DrawProperty(Component target)
    {
        EditorGUILayout.LabelField("Property:",
              new GUIStyle()
              {
                  fontStyle = FontStyle.Bold,
                  normal = new GUIStyleState() { textColor = Color.blue }
              });
        EditorGUI.indentLevel++;
        FieldInfoDawer.DrawProperty(target);
        EditorGUI.indentLevel--;
    }

    private void DrawField(Component target)
    {
        EditorGUILayout.LabelField("Field:",
               new GUIStyle()
               {
                   fontStyle = FontStyle.Bold,
                   normal = new GUIStyleState() { textColor = Color.blue }
               });
        EditorGUI.indentLevel++;

        DrawPublicField(target);
        DrawPrivateField(target);
        EditorGUI.indentLevel--;
    }

    private void DrawPublicField(Component target)
    {
        Type type = target.GetType();
        IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

        SerializedObject serializedObject = new SerializedObject(target);

        if (fields.Count() > 0)
        {
            EditorGUILayout.LabelField("Public:",
                new GUIStyle()
                {
                    fontStyle = FontStyle.Bold,
                    normal = new GUIStyleState() { textColor = Color.yellow }
                });
            EditorGUI.indentLevel++;
            foreach (FieldInfo field in fields)
            {
                FieldInfoDawer.DrawField(field, target, serializedObject);
            }
            EditorGUI.indentLevel--;
        }
    }

    private void DrawPrivateField(Component target)
    {
        Type type = target.GetType();
        IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        SerializedObject serializedObject = new SerializedObject(target);

        if (fields.Count() > 0)
        {
            EditorGUILayout.LabelField("Private:",
                new GUIStyle()
                {
                    fontStyle = FontStyle.Bold,
                    normal = new GUIStyleState() { textColor = Color.yellow }
                });
            EditorGUI.indentLevel++;
            foreach (FieldInfo field in fields)
            {
                if (!field.Name.Contains("k__"))
                {
                    FieldInfoDawer.DrawField(field, target, serializedObject);
                }
            }
            EditorGUI.indentLevel--;
        }
    }

    private int _index;
    private Vector2 _scrollView;
    private RealTimeDebugPanelModel _model;

    private static RealTimeDebugPanel _panel;
}
