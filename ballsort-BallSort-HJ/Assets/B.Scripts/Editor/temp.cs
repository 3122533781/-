#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class CreateScriptableSingleton
{
    [MenuItem("Assets/Create/CollectionConfig")]
    public static void CreateCollectionConfig()
    {
        // ����ʵ��
        CollectionConfig config = ScriptableObject.CreateInstance<CollectionConfig>();

        // ���ñ���·���������� Resources Ŀ¼�£���Ϊ ScriptableSingleton<T> ��� Resources ���أ�
        string path = "Assets/Resources/CollectionConfig.asset";

        // ����ļ��Ѵ��ڣ����ظ�����
        if (AssetDatabase.LoadAssetAtPath<CollectionConfig>(path) == null)
        {
            AssetDatabase.CreateAsset(config, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("CollectionConfig �����ɣ�" + path);
        }
        else
        {
            Debug.LogWarning("CollectionConfig �Ѵ��ڣ�" + path);
        }

        // ѡ���´�������Դ
        Selection.activeObject = config;
    }
}
#endif