using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GroupDisplay : MonoBehaviour
{
    [Button]
    public void Show()
    {
        Init();
        gameObject.SetActive(true);
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        if (_mask != null)
        {
            _sequence.Append(_mask.DOFade(0.7f, _fadeDuration));
        }

        _sequence.Join(_group.DOFade(1f, _fadeDuration));
        _sequence.Join(_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + _upDis, _upDuration));
    }

    [Button]
    public void Hide(Action onHide)
    {
        _sequence?.Kill();

        if (_mask != null)
        {
            _sequence.Append(_mask.DOFade(0.0f, _fadeDuration));
        }

        _sequence.Join(_group.DOFade(0.0f, _fadeDuration));
        _sequence.Join(_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y - _upDis, _upDuration)
        .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onHide.Invoke();
            }));
    }

    private void Init()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            _originPosY = _rectTransform.anchoredPosition.y;
        }

        if (_group == null)
        {
            _group = GetComponent<CanvasGroup>();
        }

        _rectTransform.anchoredPosition =
            new Vector2(_rectTransform.anchoredPosition.x, _originPosY - _upDis);
        if (_mask != null)
        {
            _mask.color = _mask.color.Alpha0();
        }

        _group.alpha = 0;
    }

    [SerializeField] private float _upDis = 150;
    [SerializeField] private float _upDuration = 0.3f;
    [SerializeField] private Image _mask;
    [SerializeField] private float _fadeDuration = 0.3f;

    private RectTransform _rectTransform;
    private CanvasGroup _group;
    private float _originPosY;

    private Sequence _sequence;
}