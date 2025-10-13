using UnityEngine;

public class IpadScaler : MonoBehaviour
{
    private void OnEnable()
    {
        if (ScreenSizeFitter.IsIPad)
            FitSize();
    }

    private void FitSize()
    {
        _rectTransform.Get(gameObject).localScale = Vector3.one * _scaleTo;
    }

    [SerializeField] private float _scaleTo = 1f;
    private DelayGetComponent<RectTransform> _rectTransform = new DelayGetComponent<RectTransform>();
}