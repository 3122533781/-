using Lei31;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Shadow))]
[RequireComponent(typeof(Text))]
public class ShadowFitter : MonoBehaviour
{
    private void Update()
    {
        if (Time.frameCount % 60 == 0)
        {
            float targetShadowY =
                MathHelpers.map(_rectTransform.Get(gameObject).rect.width,
                    _rectWidthRange.x,
                    _rectWidthRange.y,
                    _shadowYRange.x,
                    _shadowYRange.y
                );

            _shadow.Get(gameObject).effectDistance = new Vector2(0, -targetShadowY);
        }
    }

    [SerializeField] private Vector2 _shadowYRange = Vector2.zero;
    [SerializeField] private Vector2 _rectWidthRange = Vector2.zero;

    private DelayGetComponent<Shadow> _shadow = new DelayGetComponent<Shadow>();
    private DelayGetComponent<RectTransform> _rectTransform = new DelayGetComponent<RectTransform>();
}