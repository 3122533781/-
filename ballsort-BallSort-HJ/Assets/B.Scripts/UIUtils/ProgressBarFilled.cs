using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFilled : MonoBehaviour, IProgress
{
    public Action OnProgressOver = delegate { };
    public Action OnProgressStop = delegate { };

    public float CurrentPercentage
    {
        get { return _rectBar.fillAmount; }
    }

    public void Reset(float percentage)
    {
        UpdateProgress(percentage);
        _rectBar.fillAmount = percentage;
    }

    public void UpdateProgress(float percentage)
    {
        if (_currentTargetPercentage != percentage)
        {
            enabled = true;
            _currentTargetPercentage = percentage;
        }
    }

    private void Update()
    {
        float oldAmount = _rectBar.fillAmount;

        _rectBar.fillAmount =
            Mathf.MoveTowards(_rectBar.fillAmount, _currentTargetPercentage, Time.deltaTime * _barMoveSpeed);

        if (_rectBar.fillAmount >= 1)
        {
            OnProgressOver.Invoke();
            enabled = false;
        }

        if (Mathf.Approximately(_rectBar.fillAmount, _currentTargetPercentage)
            && !Mathf.Approximately(oldAmount, _currentTargetPercentage))
        {
            OnProgressStop.Invoke();
            enabled = false;
        }
    }

    [SerializeField] private Image _rectBar = null;
    [SerializeField] private float _barMoveSpeed = 1;
    private float _currentTargetPercentage;
}