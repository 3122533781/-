
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
[CreateAssetMenu(fileName = "CollectionPositionConfig", menuName = "ScriptTableObjects/CollectionPositionConfig", order = 1)]
public class CollectionPositionConfig : ScriptableSingleton<CollectionPositionConfig>
{
    public CollectionPositionData[] All;

    public CollectionPositionData GetPositionData(int collectId)
    {
        if (All == null || All.Length == 0)
        {
            Debug.LogError("CollectionPositionConfig 数据为空！");
            return null;
        }

        foreach (var data in All)
        {
            if (data.id == collectId)
            {
                return data;
            }
        }

        Debug.LogWarning($"未找到ID为 {collectId} 的位置配置");
        return null;
    }


}

