using _02.Scripts.InGame.UI;
using Fangtang;
using UnityEngine;

public class InGameTimeController : ElementBehavior<InGame>
{
    private float waitTime;
    public int seconds = 180; // 3分钟 = 180秒（初始时间）

    protected override void OnInit()
    {
        // 初始化时就更新一次UI显示
        UpdateTimeDisplay();
    }

    void Update()
    {
        Timing();
    }

    private void Timing()
    {
        if (seconds <= 0) return;

        waitTime += Time.deltaTime;
        if (waitTime >= 1)
        {
            seconds--;
            waitTime = 0;
            UpdateTimeDisplay(); // 更新UI显示

            if (seconds == 0)
            {
                Context.Failed(); // 时间到，游戏失败
            }
        }
    }

    // 更新时间显示为"MM:SS"格式
    private void UpdateTimeDisplay()
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;
        // 格式化字符串为两位数显示
        string timeString = $"{minutes:D2}:{remainingSeconds:D2}";
        // 调用UI的SetTime方法传递格式化后的字符串
        Context.GetView<InGamePlayingUI>().SetTimeText(timeString);
    }

    public void AddSecound()
    {
        seconds += GlobalDef.INGAME_ADD_TIME;
        // 确保时间不会超过合理上限（可选）
        if (seconds > 3600) seconds = 3600; // 最多60分钟
        UpdateTimeDisplay(); // 更新显示
    }
}
