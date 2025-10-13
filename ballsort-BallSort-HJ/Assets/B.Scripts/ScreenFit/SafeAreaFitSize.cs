using UnityEngine;

public class SafeAreaFitSize : MonoBehaviour
{
    //fit center (stretch)
    private void OnEnable()
    {
        if (_thisRect == null)
        {
            _thisRect = GetComponent<RectTransform>();
        }

        float safeAreaTopHeight = Screen.height - Screen.safeArea.y - Screen.safeArea.height;
        float safeAreaBottomHeight = Screen.safeArea.y;

        _thisRect.offsetMax = new Vector2(0, -(_originTop + safeAreaTopHeight));
        _thisRect.offsetMin = new Vector2(0, _originBottom + safeAreaBottomHeight);
    }


    [SerializeField] private float _originTop = 0;
    [SerializeField] private float _originBottom = 0;

    private RectTransform _thisRect;
}