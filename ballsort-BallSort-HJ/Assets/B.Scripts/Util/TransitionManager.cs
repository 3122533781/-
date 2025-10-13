using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoSingleton<TransitionManager>
{
    
    public void Transition(float fadeoutDuration, Action onTransitionComplete, float fadeinDuration,
        Action onWaitComplete = null, bool useBlck = true)
    {
        _onTransitionComplete = onTransitionComplete;
        _onWaitComplete = onWaitComplete;
        _fadeinDuration = fadeinDuration;
        _transitionImage.color = useBlck ? _black : _white;
        _transitionImage.color =
            new Color(_transitionImage.color.r, _transitionImage.color.g, _transitionImage.color.b,
                0);
        //Color endValue = new Color(_transitionImage.color.r, _transitionImage.color.g, _transitionImage.color.b, 1);
        _transitionImage.gameObject.SetActive(true);
        _transitionImage.DOFade(1, fadeoutDuration).OnComplete(TransitionComplete);
    }

    private void TransitionComplete()
    {
        _onTransitionComplete.Invoke();

        _transitionImage.DOFade(0, _fadeinDuration).OnComplete(WaitComplete);
    }

    private void WaitComplete()
    {
        if (_onWaitComplete != null)
        {
            _onWaitComplete.Invoke();
        }

        _transitionImage.gameObject.SetActive(false);
    }

    [SerializeField] private Image _transitionImage = null;
    [SerializeField] private Color _white = Color.white;
    [SerializeField] private Color _black = Color.black;

    private Action _onTransitionComplete;
    private Action _onWaitComplete;
    private float _fadeinDuration;
}