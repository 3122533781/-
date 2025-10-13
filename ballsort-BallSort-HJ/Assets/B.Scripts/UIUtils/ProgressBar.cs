using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 新增：用于协程

public class ProgressBar : MonoBehaviour, IProgress
{
    // 原有属性：不改动
    public float CurrentPercentage
    {
        get
        {
            return _rectBar.fillAmount;
        }
    }

    // 原有方法：不改动
    public void Reset(float percentage)
    {
        UpdateProgress(percentage);
    }

    // 原有方法：不改动
    public void UpdateProgress(float percentage)
    {
        _rectBar.fillAmount = percentage;

        var posX = _rectBar.rectTransform.sizeDelta.x * percentage;
        // _pig.rectTransform.anchoredPosition = new Vector2(posX, 39);
    }


    // ---------------------- 新增部分开始 ----------------------
    // 可在Inspector面板调整：进度条缓慢移动的总时长（默认0.5秒，值越大越慢）
    [SerializeField] private float smoothDuration = 0.5f;
    // 标记是否正在播放平滑动画，避免重复触发冲突
    private bool isSmoothing = false;

    /// <summary>
    /// 新增：缓慢更新进度条（平滑过渡到目标进度）
    /// </summary>
    /// <param name="targetPercentage">目标进度（0~1范围，与原有UpdateProgress参数格式一致）</param>
    public void UpdateProgressSmooth(float targetPercentage)
    {
        // 1. 进度值安全校验（与原有逻辑保持一致，限制在0~1）
        targetPercentage = Mathf.Clamp01(targetPercentage);

        // 2. 避免重复动画：若正在平滑过渡，或目标进度与当前一致，则不执行
        if (isSmoothing || Mathf.Approximately(CurrentPercentage, targetPercentage))
        {
            return;
        }

        // 3. 启动协程执行平滑动画
        StartCoroutine(SmoothProgressCoroutine(targetPercentage));
    }

    /// <summary>
    /// 新增：协程 - 控制进度条从当前值平滑过渡到目标值
    /// </summary>
    private IEnumerator SmoothProgressCoroutine(float targetPercentage)
    {
        isSmoothing = true; // 标记动画开始
        float startPercentage = CurrentPercentage; // 记录动画起始进度
        float elapsedTime = 0f; // 已流逝时间

        // 4. 逐帧更新进度：直到时间耗尽或进度达到目标
        while (elapsedTime < smoothDuration)
        {
            elapsedTime += Time.deltaTime; // 累加时间（与帧率无关）
            // 线性插值计算当前进度：实现平滑过渡
            float currentProgress = Mathf.Lerp(startPercentage, targetPercentage, elapsedTime / smoothDuration);

            // 5. 复用原有UpdateProgress方法更新UI（确保与原有逻辑完全一致，不重复代码）
            UpdateProgress(currentProgress);

            yield return null; // 等待下一帧，实现逐帧平滑移动
        }

        // 6. 动画结束：强制更新到目标进度（避免浮点误差导致未达目标）
        UpdateProgress(targetPercentage);
        isSmoothing = false; // 标记动画结束
    }
    // ---------------------- 新增部分结束 ----------------------


    // 原有字段：不改动
    [SerializeField] private Image _rectBar;
    // [SerializeField] private Image _pig;
}