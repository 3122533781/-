using Prime31;
using UnityEngine;

public class SafeAreaFitStretchAll : MonoBehaviour, Prime31.IObjectInspectable
{
    private void OnEnable()
    {
        Set();
    }

    [MakeButton]
    private void Set()
    {
        if (_thisRect == null)
        {
            _thisRect = GetComponent<RectTransform>();
        }

        float safeAreaTopHeight = Screen.height - Screen.safeArea.y - Screen.safeArea.height;
        float safeAreaBottomHeight = Screen.safeArea.y;

//        LDebug.Log($"safeAreaTopHeight:{safeAreaTopHeight} safeAreaBottomHeight:{safeAreaBottomHeight}");

//        _thisRect.sizeDelta = new Vector2(0, -safeAreaTopHeight);
//        _thisRect.anchoredPosition = new Vector2(0, -1 * safeAreaTopHeight / 2f);

        _thisRect.offsetMin = new Vector2(0, safeAreaBottomHeight);
        _thisRect.offsetMax = new Vector2(0, -safeAreaTopHeight);
        
        //offsetMin.x : left offsetMin.y : bottom
        //offsetMax.x : right  offsetMax.y: Top
    }

    private RectTransform _thisRect;
}