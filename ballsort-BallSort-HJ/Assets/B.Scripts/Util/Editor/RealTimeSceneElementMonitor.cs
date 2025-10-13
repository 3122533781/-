using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Fangtang;

public class RealTimeSceneElementMonitor : EditorWindow
{
    public RealTimeSceneElementMonitor()
    {
        _model = new RealTimeSceneElementModel();
    }

    [MenuItem("SoyBean/Debug/RealTime SceneElement monitor Panel %#&f")]
    public static void OpenDebugPanel()
    {
         EditorWindow.GetWindow<RealTimeSceneElementMonitor>();
    }

    private void Update()
    {
        _model.ResetAllElement(FindAllElement());

        Repaint();
    }

    private GUIStyle GetFoldoutStyle()
    {
        GUIStyle result = EditorStyles.foldout;
        result.stretchWidth = false;
        result.fixedWidth = 20f;
        return result;
    }

    private void OnGUI()
    {
        if (!Application.isPlaying) return;

        _scrollView = EditorGUILayout.BeginScrollView(_scrollView);
        for (int i = 0; i < _model.AllElement.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            _model.AllElementFoldoutBool[i] =
                EditorGUILayout.Foldout(_model.AllElementFoldoutBool[i], " ", GetFoldoutStyle());
            EditorGUILayout.ObjectField(_model.AllElement[i], _model.AllElement[i].GetType()
                ,true);

            EditorGUILayout.EndHorizontal();
            if (_model.AllElementFoldoutBool[i])
            {
                EditorGUI.indentLevel++;
                DrawSceneElementInfo(_model.AllElement[i]);
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawSceneElementInfo(SceneElement element)
    {
        System.Type sceneElementType = element.GetType();
        System.Reflection.PropertyInfo stateMachineProperty = sceneElementType.GetProperty("StateMachine");
        if (stateMachineProperty != null)
        {
            object stateMachine = stateMachineProperty.GetValue(element, null);
            System.Type stateMachineType = stateMachine.GetType();

            object currentState = stateMachineType.GetProperty("currentState").GetValue(stateMachine, null);
            object previousState = stateMachineType.GetProperty("previousState").GetValue(stateMachine, null);
            EditorGUILayout.LabelField("Current State", currentState == null ? "null" : currentState.ToString());
            EditorGUILayout.LabelField("Previous State", previousState == null ? "null" : previousState.ToString());
            EditorGUILayout.Space();
        }
        else
        {
            System.Reflection.FieldInfo modelField = sceneElementType.GetField("_stateModel",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (modelField != null) //GenericSceneElement
            {
                object model = modelField.GetValue(element);
                System.Type modelType = model.GetType();

                object currentState = modelType.GetProperty("CurrentState").GetValue(model, null);
                object previousState = modelType.GetProperty("PreviousState").GetValue(model, null);
                EditorGUILayout.LabelField("Current State", currentState == null ? "null" : currentState.ToString());
                EditorGUILayout.LabelField("Previous State", previousState == null ? "null" : previousState.ToString());
                EditorGUILayout.Space();
            }
        }

        System.Reflection.PropertyInfo controllersProperty = sceneElementType.GetProperty("Controllers");
        if (controllersProperty != null)
        {
            DrawComponents(element, controllersProperty, "Controllers");
        }

        System.Reflection.PropertyInfo viewsProperty = sceneElementType.GetProperty("Interfaces");
        if (viewsProperty != null)
        {
            DrawComponents(element, viewsProperty, "Interfaces");
        }
    }

    private void DrawComponents(SceneElement element, System.Reflection.PropertyInfo componentsProperty, string label)
    {
        object components = componentsProperty.GetValue(element, null);
        System.Type componentsType = components.GetType();
        EditorGUILayout.LabelField(label);
        if (components != null)
        {
            System.Type elementComponentsType = componentsType;
            System.Reflection.FieldInfo viewsField = elementComponentsType.GetField("_components",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            IDictionary componentsDictionary =
                viewsField.GetValue(components) as IDictionary;
            if (componentsDictionary != null)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Number of " + label + ": " + componentsDictionary.Count.ToString());

                foreach (var component in componentsDictionary.Values)
                {
                    MonoBehaviour behavior = component as MonoBehaviour;
                    if (behavior != null)
                    {
                        GUI.enabled = behavior.enabled;
                        EditorGUILayout.LabelField(behavior.GetType().ToString());
                        GUI.enabled = true;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
    }

    private SceneElement[] FindAllElement()
    {
        return Object.FindObjectsOfType<SceneElement>();
    }

    private RealTimeSceneElementModel _model;
    private Vector2 _scrollView;
}
