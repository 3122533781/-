using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TextNumberAnimHelper
{
    public static IEnumerator PlayNumberChangeAnim(Text target, int current, int endValue, float duration,
        Action onCompleted = null)
    {
        Assert.AreEqual(duration > 0.05f, true);

        float stepDuration = 0.05f;
        int stepCount = Mathf.RoundToInt(duration / stepDuration);

        int stepValue = (endValue - current) / stepCount;
        if (stepValue == 0)
        {
            stepValue = 1;
        }

        float origin = 0;
        if (endValue < current)
        {
//            while (current + origin > endValue)
//            {
//                target.text = (current + origin).ToString();
//                yield return Yielders.Get(stepDuration);
//                origin += stepValue;
//            }
        }
        else
        {
            while (current + origin < endValue)
            {
                target.text = (current + origin).ToString();
                yield return Yielders.Get(stepDuration);
                origin += stepValue;
            }
        }

        target.text = endValue.ToString();

        onCompleted?.Invoke();
    }

    public static IEnumerator PlayNumberChangeAnim(Text target, float current, float endValue, float duration
        , Action onCompleted = null)
    {
        Assert.AreEqual(duration > 0.05f, true);

        float stepDuration = 0.05f;
        int stepCount = Mathf.RoundToInt(duration / stepDuration);

        float stepValue = (endValue - current) / stepCount;

        float origin = 0;
        while (current + origin < endValue)
        {
            target.text = (current + origin).ToString("0.00");
            yield return Yielders.Get(stepDuration);
            origin += stepValue;
        }

        target.text = endValue.ToString("0.00");
        onCompleted?.Invoke();
    }
}