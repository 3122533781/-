using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarSlider : MonoBehaviour, IProgress
{
    public Action OnProgressOver = delegate { };
    public Action OnProgressStop = delegate { };

    public float CurrentPercentage
    {
        get { return _sliderBar.value / _sliderBar.maxValue; }
    }

    public float CurrentValue
    {
        get => _sliderBar.value;
    }

    public void Reset(float value)
    {
        UpdateProgress(value);
        _sliderBar.value = value;
    }

    public void UpdateProgress(float value)
    {
        if (_currentTargetValue != value)
        {
            enabled = true;
            _currentTargetValue = value;
        }
    }

    private void Update()
    {
        float oldAmount = _sliderBar.value;

        _sliderBar.value =
            Mathf.MoveTowards(_sliderBar.value, _currentTargetValue, Time.deltaTime * _barMoveSpeed);

        if (_sliderBar.value >= _sliderBar.maxValue)
        {
            OnProgressOver.Invoke();
            enabled = false;
        }

        if (Mathf.Approximately(_sliderBar.value, _currentTargetValue)
            && !Mathf.Approximately(oldAmount, _currentTargetValue))
        {
            OnProgressStop.Invoke();
            enabled = false;
        }
    }

    [SerializeField] private Slider _sliderBar = null;
    [SerializeField] private float _barMoveSpeed = 1;
    private float _currentTargetValue;
}