using _02.Scripts.Home.Active;
using ProjectSpace.BubbleMatch.Scripts.SubSystem.ActivitySystem;
using ProjectSpace.Lei31Utils.Scripts.Common;
using UnityEngine;

namespace ProjectSpace.BubbleMatch.Scripts.Home.Active
{
    /// <summary>
    /// 活动按钮，会设置本身的红点
    /// 使用条件：
    /// 1.有红点
    /// 2.有显示条件
    /// </summary>
    public class ActiveItem : DialogButtonItem
    {
        [SerializeField] protected RedDotItem redDotItem;
        [SerializeField] protected AppriseType redType;

        /// <summary>
        /// 检查是否解锁功能
        /// </summary>
        /// <param name="isCanShow"></param>
        protected override void CheckIsCanShow(out bool isCanShow)
        {
            isCanShow = Game.Instance.GetSystem<ActivitySystem>().IsUnlockActive(nameType);
        }


        /// <summary>
        /// 绑定显示 && 红点
        /// </summary>
        protected override void BindShowAction()
        {
            base.BindShowAction();
            redDotItem.Refresh(redType);
        }
    }
}