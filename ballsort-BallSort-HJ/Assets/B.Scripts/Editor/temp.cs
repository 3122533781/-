#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class CreateScriptableSingleton
{
    [MenuItem("Assets/Create/CollectionConfig")]
    public static void CreateCollectionConfig()
    {
        // 创建实例
        CollectionConfig config = ScriptableObject.CreateInstance<CollectionConfig>();

        // 设置保存路径（必须在 Resources 目录下，因为 ScriptableSingleton<T> 会从 Resources 加载）
        string path = "Assets/Resources/CollectionConfig.asset";

        // 如果文件已存在，不重复创建
        if (AssetDatabase.LoadAssetAtPath<CollectionConfig>(path) == null)
        {
            AssetDatabase.CreateAsset(config, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("CollectionConfig 已生成：" + path);
        }
        else
        {
            Debug.LogWarning("CollectionConfig 已存在：" + path);
        }

        // 选中新创建的资源
        Selection.activeObject = config;
    }
}
#endif