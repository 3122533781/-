using UnityEngine;
using System.Collections;

public class ScreenHeightCalculator : MonoBehaviour
{
    [SerializeField] private RectTransform targetRect; // 目标UI元素
    [SerializeField] private RectTransform targetRect2; // 目标UI元素

    private void OnEnable()
    {
        // 选择一种方式：
        // 方式1：立即执行（带延迟日志）
        ApplyScaledTopPadding();

        // 方式2：完全延迟执行
        // StartCoroutine(ApplyScaledTopPaddingWithDelay());
    }

    public float GetScaledScreenHeight()
    {
        StartCoroutine(ApplyScaledTopPaddingWithDelay()); // 正确调用协程

        if (targetRect2 != null && targetRect2.sizeDelta.y > 2000)
            return 73f;
        else
            return 5f;
    }

    public void ApplyScaledTopPadding()
    {
        if (targetRect == null)
        {
            Debug.LogError("未指定目标RectTransform");
            return;
        }

        float scaledPadding = GetScaledScreenHeight();
    }

    public IEnumerator ApplyScaledTopPaddingWithDelay()
    {
        if (targetRect == null)
        {
            Debug.LogError("未指定目标RectTransform");
            yield break;
        }

        // 延迟0.2秒
        yield return new WaitForSeconds(0f);

        // 延迟后执行计算
        float scaledPadding = targetRect2 != null && targetRect2.sizeDelta.y > 2000 ? 73f : 5f;
        Debug.Log("大小" + scaledPadding + " " + (targetRect2 != null ? targetRect2.sizeDelta.y : 0));

        // 应用边距
        targetRect.offsetMax = new Vector2(targetRect.offsetMax.x, -scaledPadding);
    }
}