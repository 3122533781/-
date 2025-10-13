using System.Diagnostics;
using DG.Tweening;
using UnityEngine;

public class NumberScaleAnim : MonoBehaviour
{
    public void FuncScale()
    {
        //_funcTimer++;
        //if (NeedScale())
        //{
        //    PlayScaleAnim();
        //}
        PlayScaleAnim();
    }

    private void PlayScaleAnim()
    {
        if (!_isPlaying)
        {
            _isPlaying = true;
            _rectTransform.Get(gameObject).localScale = Vector3.one * 0.8f;
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(_rectTransform.Get(gameObject).DOScale(_scaleValue, _scaleTime));
            _sequence.Append(_rectTransform.Get(gameObject).DOScale(Vector3.one * 0.8f, _scaleTime));
            _sequence.OnComplete(() => {
                _rectTransform.Get(gameObject).localScale = Vector3.one;
                _isPlaying = false; });
        }
    }

    private bool NeedScale()
    {
        return _funcTimer >= _funcTimeToScale;
    }

    private void OnEnable()
    {
        _funcTimer = 0;
        _isPlaying = false;
    }

    [SerializeField] private int _funcTimeToScale = 2;
    [SerializeField] private Vector3 _scaleValue = new Vector3(0.9f, 1.1f, 1);
    [SerializeField] private float _scaleTime = 0.1f;

    private DelayGetComponent<Transform> _rectTransform = new DelayGetComponent<Transform>();
    private int _funcTimer;

    private Sequence _sequence;
    private bool _isPlaying;
}