using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProgressBarMarkerGenerator : MonoBehaviour
{
    [Header("����������")]
    public RectTransform progressBarRect;
    public int defaultMarkerCount = 4;
    public float defaultMarkerYPos = 0f;

    [Header("��λUI����")]
    public RectTransform markerPrefab;
    public Transform markerParent;

    private List<RectTransform> generatedMarkers = new List<RectTransform>();


    public void GenerateEqualMarkers(int markerCount, float markerYPos)
    {
        if (progressBarRect == null || markerPrefab == null || markerParent == null)
        {
            Debug.LogError("GenerateEqualMarkers��������/RectTransform/��λԤ����/������δ��ֵ��");
            return;
        }
        if (markerCount < 1)
        {
            Debug.LogError("GenerateEqualMarkers����λ������������㣩�����1��");
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
            Debug.Log($"���ɵ�λ{i}/{markerCount}��λ��(X,Y)��({markerX:F2}, {markerYPos:F2})����");
        }
    }


    private void SetLevelTextForMarker(RectTransform marker, int order)
    {
        if (marker == null)
        {
            Debug.LogError("SetLevelTextForMarker����λʵ��Ϊ�գ��޷�����Text��");
            return;
        }

        Transform levelChild = marker.Find("Level");
        if (levelChild == null)
        {
            Debug.LogError($"��λ {marker.name} �Ҳ�����Ϊ��Lvel���������壡");
            return;
        }

        Text levelText = levelChild.GetComponent<Text>();
        if (levelText == null)
        {
            Debug.LogError($"�����塰Lvel����δ����Text�������λ��{marker.name}");
            return;
        }

        levelText.text = order.ToString();
        Debug.Log($"��λ {marker.name} ��Lvel�ı�������Ϊ��{order}");
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
        Debug.Log("��������н�������λ");
    }


    private void Start()
    {
        // GenerateEqualMarkers(3, 10f);
    }
}