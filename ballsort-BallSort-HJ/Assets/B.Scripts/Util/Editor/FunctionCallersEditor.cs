using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(FunctionCallers))]
public class FunctionCallersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FunctionCallers callers = target as FunctionCallers;

        if (callers == null) return;

        if (callers.All == null)
        {
            callers.All = new List<CallerInfo>();
        }

        EditorGUI.BeginChangeCheck();
        
        if (GUILayout.Button("Add Caller"))
        {
            callers.All.Add(new CallerInfo());
        }

        if (callers.All.Count == 0) return;

        GUILayout.Space(5);

        CacheMonoBehaviorsForGameObject(callers.gameObject);
        CacheMethodsForGameObject(callers.gameObject);

        CallerInfo toBeRemove = null;
        for (int i = 0; i < callers.All.Count; i++)
        {
            GUILayout.BeginVertical("Box");

            CallerInfo callerInfo = callers.All[i];
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                toBeRemove = callerInfo;
            }

            callerInfo.Key = (KeyCode)EditorGUILayout.EnumPopup("Key", callerInfo.Key);

            int idx = _monobehaviorsCache.IndexOf(callerInfo.MonoBehaviourName);
            int nidx = EditorGUILayout.Popup("MonoBehaviour", idx, _monobehaviorsCache.ToArray());
            if (nidx != idx)
            {
                callerInfo.MonoBehaviourName = _monobehaviorsCache[nidx];
            }

            if (!string.IsNullOrEmpty(callerInfo.MonoBehaviourName))
            {
                MethodBinding("Function", callerInfo.MonoBehaviourName, ref callerInfo.MethodName);

                GUI.enabled = EditorApplication.isPlaying;
                if (GUILayout.Button(string.Format("Call(Press '{0}')", callerInfo.Key)))
                {
                    if (callers.gameObject != null && callerInfo.MethodName.Length > 0)
                    {
                        callers.gameObject.SendMessage(callerInfo.MethodName,
                            this, SendMessageOptions.RequireReceiver);
                    }
                }
                GUI.enabled = true;
            }

            GUILayout.EndVertical();
        }

        if (toBeRemove != null)
        {
            callers.All.Remove(toBeRemove);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "FunctionCallers");
            EditorUtility.SetDirty(target);
        }
    }

    private void MethodBinding(string name, string behaviourName, ref string methodName)
    {
        GUILayout.BeginHorizontal();

        List<string> cachedMethods = _methodsCache[behaviourName];
        int idx = cachedMethods.IndexOf(methodName);
        int nidx = EditorGUILayout.Popup(name, idx, cachedMethods.ToArray());
        if (nidx != idx)
        {
            methodName = cachedMethods[nidx];
        }
        if (!string.IsNullOrEmpty(methodName) && methodName.Length != 0)
        {
            if (GUILayout.Button("Clear", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
            {
                methodName = "";
                HandleUtility.Repaint();
            }
        }

        GUILayout.EndHorizontal();
    }

    private void CacheMonoBehaviorsForGameObject(GameObject go)
    {
        MonoBehaviour[] behaviours = go.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour beh in behaviours)
        {
            if (!beh.GetType().ToString().Equals("FunctionCallers"))
            {
                _monobehaviorsCache.Add(beh.GetType().ToString());
            }
        }
    }

    private void CacheMethodsForGameObject(GameObject go)
    {
        List<System.Type> addedTypes = new List<System.Type>();
        MonoBehaviour[] behaviours = go.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour beh in behaviours)
        {
            if (!_methodsCache.ContainsKey(beh.GetType().ToString()))
            {
                _methodsCache.Add(beh.GetType().ToString(), new List<string>());
            }
            System.Type type = beh.GetType();
            if (addedTypes.IndexOf(type) == -1)
            {
                System.Reflection.MethodInfo[] methods = type.GetMethods(
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic);
                foreach (System.Reflection.MethodInfo method in methods)
                {
                    // Only add variables added by user, 
                    // i.e. we don't want functions from the base UnityEngine baseclasses or lower
                    string moduleName = method.DeclaringType.Assembly.ManifestModule.Name;
                    if (!moduleName.Contains("UnityEngine") && !moduleName.Contains("mscorlib") &&
                        !method.ContainsGenericParameters &&
                        System.Array.IndexOf(ignoredMethodNames, method.Name) == -1)
                    {

                        System.Reflection.ParameterInfo[] paramInfo = method.GetParameters();
                        if (paramInfo.Length == 0)
                        {
                            _methodsCache[beh.GetType().ToString()].Add(method.Name);
                        }
                    }
                }
            }
        }
    }

    private static readonly string[] ignoredMethodNames = new string[] {
        "Start", "Awake", "OnEnable", "OnDisable",
        "Update", "OnGUI", "LateUpdate", "FixedUpdate"
    };

    private Dictionary<string, List<string>> _methodsCache = new Dictionary<string, List<string>>();
    private List<string> _monobehaviorsCache = new List<string>();
}
