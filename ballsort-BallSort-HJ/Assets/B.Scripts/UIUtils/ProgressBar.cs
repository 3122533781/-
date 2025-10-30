using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour, IProgress
{
    public float CurrentPercentage
    {
        get
        {
            return _rectBar.fillAmount;
        }
    }

    public void Reset(float percentage)
    {
        UpdateProgress(percentage);
    }

    public void UpdateProgress(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        _rectBar.fillAmount = percentage;

        if (_followImage != null)
        {
            float barTotalWidth = _rectBar.rectTransform.sizeDelta.x;
            float imageX = barTotalWidth * percentage;
            _followImage.anchoredPosition = new Vector2(imageX, followImageY);
        }
    }


    [SerializeField] private float smoothDuration = 0.5f;
    private bool isSmoothing = false;

    public void UpdateProgressSmooth(float targetPercentage)
    {
        targetPercentage = Mathf.Clamp01(targetPercentage);

        if (isSmoothing || Mathf.Approximately(CurrentPercentage, targetPercentage))
        {
            return;
        }

        StartCoroutine(SmoothProgressCoroutine(targetPercentage));
    }

    private IEnumerator SmoothProgressCoroutine(float targetPercentage)
    {
        isSmoothing = true;
        float startPercentage = CurrentPercentage;
        float elapsedTime = 0f;

        while (elapsedTime < smoothDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentProgress = Mathf.Lerp(startPercentage, targetPercentage, elapsedTime / smoothDuration);

            UpdateProgress(currentProgress);

            yield return null;
        }

        UpdateProgress(targetPercentage);
        isSmoothing = false;
    }


    [SerializeField] private Image _rectBar;
    [SerializeField] private RectTransform _followImage;
    [SerializeField] private float followImageY = 39;
}