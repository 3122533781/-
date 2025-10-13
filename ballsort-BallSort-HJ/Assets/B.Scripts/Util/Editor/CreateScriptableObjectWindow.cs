using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class CreateScriptableObjectWindow : EditorWindow
{
    [MenuItem("SoyBean/Open Scene/Capture %#a")]
    public static void Capture()
    {
        // 构建完整路径
        string fullPath = Path.Combine(Application.persistentDataPath, "Capture" + _number++ + ".png");

        // 保存截图
        ScreenCapture.CaptureScreenshot(fullPath);

        // 打印完整路径（方便查找）
        Debug.Log("截图已保存到: " + fullPath);

        // 打开保存目录（可选）
        System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(fullPath));
    }

    [MenuItem("SoyBean/Helper/Scriptable Object Manager")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow<CreateScriptableObjectWindow>();
        window.titleContent = new GUIContent("Scriptable Object Manager");
    }

    private static void CreateNewScriptableObject(Type instanceType)
    {
        string assetPath =
            EditorUtility.SaveFilePanelInProject("Select Path (" + instanceType.ToString() + ")",
                Guid.NewGuid().ToString(), "asset", "", EditorPrefs.GetString(PathPreferencesKey, "Assets"));

        if (string.IsNullOrEmpty(assetPath) == false)
        {
            EditorPrefs.SetString(PathPreferencesKey, Path.GetDirectoryName(assetPath));

            ScriptableObject asset = ScriptableObject.CreateInstance(instanceType);
            AssetDatabase.CreateAsset(asset, assetPath);
        }
    }

    public void OnEnable()
    {
        _searchFilter = EditorPrefs.GetString(SearchPreferencesKey, string.Empty);
        UpdateTypesAndLables();
    }

    public void OnGUI()
    {
        var filter = EditorGUILayout.TextField("Search", _searchFilter);
        if (!filter.Equals(_searchFilter))
        {
            _searchFilter = filter;
            EditorPrefs.SetString(SearchPreferencesKey, _searchFilter);
            UpdateTypesAndLables();
        }

        if (_types.Length == 0)
        {
            EditorGUILayout.HelpBox("There are no derived ScriptableObject types to manage.", MessageType.Error);
            return;
        }

        _index = EditorGUILayout.Popup(new GUIContent("ScriptableObject"), _index, _labels);
        if (GUILayout.Button("Create"))
        {
            CreateNewScriptableObject(_types[_index]);
        }
    }

    private void UpdateTypesAndLables()
    {
        _types =
            (from type in RuntimeReflectionUtility.AllSimpleCreatableTypesDerivingFrom(typeof(ScriptableObject))
             where type.Assembly.FullName.Contains("UnityEngine") == false
             where type.Assembly.FullName.Contains("UnityEditor") == false
             select type).ToArray();
        if (!string.IsNullOrEmpty(_searchFilter))
        {
            _types =
                (from type in _types
                 where type.ToString().Contains(_searchFilter) == true
                 select type).ToArray();
        }

        _labels = _types.Select(t => new GUIContent(t.FullName)).ToArray();
        _index = 0;
    }

    private string _searchFilter;
    private int _index;
    private Type[] _types;
    private GUIContent[] _labels;
    private static int _number = 0;
    private const string SearchPreferencesKey = "ScriptableObjectCreatorWindow_Search";
    private const string PathPreferencesKey = "ScriptableObjectCreatorWindow_Path";
}