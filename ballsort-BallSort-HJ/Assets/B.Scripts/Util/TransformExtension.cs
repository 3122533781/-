using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public static class TransformExtension
{
    public static void Reset(this Transform thisTransform)
    {
        thisTransform.position = Vector3.zero;
        thisTransform.rotation = Quaternion.identity;
        thisTransform.localScale = Vector3.one;
    }

    public static void ResetLocal(this Transform thisTransform)
    {
        thisTransform.localPosition = Vector3.zero;
        thisTransform.localRotation = Quaternion.identity;
        thisTransform.localScale = Vector3.one;
    }

    public static void SetActive(this Transform thisTransform, bool isActive)
    {
        if (thisTransform.gameObject.activeSelf != isActive)
        {
            thisTransform.gameObject.SetActive(isActive);
        }
    }

    public static async void DoTextValue(this Text thisText, int from, int target)
    {
        int times = target - from;
        int waitTime = 300 / times;
        for (int i = 0; i < times; i++)
        {
            await Task.Delay(waitTime);
            if (target > from)
            {
                from++;
                if (thisText == null)
                {
                    return;
                }
                
                thisText.text = from.ToString();
            }
        }
    }

    public static async void DoTextAppend(this Text thisText, string value, int unitMilliseconds)
    {
        thisText.text = "";
        char[] charArray = value.ToCharArray();
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < charArray.Length; i++)
        {
            await Task.Delay(unitMilliseconds);
            builder.Append(charArray[i]);
            thisText.text = builder.ToString();
        }
    }
    
    public static void SetAnchoredPositionY(this RectTransform origin, float y)
    {
        origin.anchoredPosition = new Vector2(origin.anchoredPosition.x, y);
    }

    public static void SetAnchoredPositionX(this RectTransform origin, float x)
    {
        origin.anchoredPosition = new Vector2(x, origin.anchoredPosition.y);
    }

    public static void SetSizeDeltaY(this RectTransform origin, float y)
    {
        origin.sizeDelta = new Vector2(origin.sizeDelta.x, y);
    }

    public static void SetSizeDeltaX(this RectTransform origin, float x)
    {
        origin.sizeDelta = new Vector2(x, origin.sizeDelta.y);
    }
}