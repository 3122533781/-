using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProgressBarMarkerGenerator : MonoBehaviour
{
    [Header("进度条配置")]
    public RectTransform progressBarRect;
    public int defaultMarkerCount = 4;
    public float defaultMarkerYPos = 0f;

    [Header("点位UI配置")]
    public RectTransform markerPrefab;
    public Transform markerParent;

    private List<RectTransform> generatedMarkers = new List<RectTransform>();


    public void GenerateEqualMarkers(int markerCount, float markerYPos)
    {
        if (progressBarRect == null || markerPrefab == null || markerParent == null)
        {
            Debug.LogError("GenerateEqualMarkers：进度条/RectTransform/点位预制体/父对象未赋值！");
            return;
        }
        if (markerCount < 1)
        {
            Debug.LogError("GenerateEqualMarkers：点位数量（不含起点）必须≥1！");
            return;
        }

        ClearAllMarkers();

        float progressBarWidth = progressBarRect.rect.width;
        Vector2 progressBarPivot = progressBarRect.pivot;
        int splitCount = markerCount;

        for (int i = 1; i <= markerCount; i++)
        {
            float markerX = (float)i / splitCount * progressBarWidth;

            Vector2 markerAnchoredPos = new Vector2(
                markerX - (progressBarWidth * progressBarPivot.x),
                markerYPos
            );

            RectTransform newMarker = Instantiate(markerPrefab, markerParent);
            newMarker.gameObject.SetActive(true);
            newMarker.localScale = Vector3.one;
            newMarker.anchoredPosition = markerAnchoredPos;

            SetLevelTextForMarker(newMarker, i);

            generatedMarkers.Add(newMarker);
            Debug.Log($"生成点位{i}/{markerCount}，位置(X,Y)：({markerX:F2}, {markerYPos:F2})像素");
        }
    }


    private void SetLevelTextForMarker(RectTransform marker, int order)
    {
        if (marker == null)
        {
            Debug.LogError("SetLevelTextForMarker：点位实例为空，无法设置Text！");
            return;
        }

        Transform levelChild = marker.Find("Level");
        if (levelChild == null)
        {
            Debug.LogError($"点位 {marker.name} 找不到名为“Lvel”的子物体！");
            return;
        }

        Text levelText = levelChild.GetComponent<Text>();
        if (levelText == null)
        {
            Debug.LogError($"子物体“Lvel”上未挂载Text组件！点位：{marker.name}");
            return;
        }

        levelText.text = order.ToString();
        Debug.Log($"点位 {marker.name} 的Lvel文本已设置为：{order}");
    }


    public void GenerateEqualMarkersWithDefault()
    {
        GenerateEqualMarkers(defaultMarkerCount, defaultMarkerYPos);
    }


    public void ClearAllMarkers()
    {
        foreach (var marker in generatedMarkers)
        {
            if (marker != null)
            {
                DestroyImmediate(marker.gameObject);
            }
        }
        generatedMarkers.Clear();
        Debug.Log("已清空所有进度条点位");
    }


    private void Start()
    {
        // GenerateEqualMarkers(3, 10f);
    }
}