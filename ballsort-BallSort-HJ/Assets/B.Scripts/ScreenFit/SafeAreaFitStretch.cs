using UnityEngine;

public class SafeAreaFitStretch : MonoBehaviour
{
    [SerializeField] public bool IsBottom = true;

    private void OnEnable()
    {
        if (_thisRect == null)
        {
            _thisRect = GetComponent<RectTransform>();
        }

        float safeAreaTopHeight = IsBottom
            ? Screen.safeArea.y
            : Screen.height - Screen.safeArea.y - Screen.safeArea.height;

        float newHeight = _originHeight + safeAreaTopHeight;
        _thisRect.sizeDelta = new Vector2(0, newHeight);
        _thisRect.anchoredPosition = new Vector2(0, (IsBottom ? 1 : -1) * newHeight / 2f);
    }


    [SerializeField] private float _originHeight = 0;

    private RectTransform _thisRect;
}