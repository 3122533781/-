using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.Home.Active
{
    /// <summary>
    /// 按钮点击打开弹窗
    /// 用于常驻活动（设置）
    /// </summary>
    public class DialogButtonItem : MonoBehaviour
    {
        [SerializeField] protected Button iconButton;
        [SerializeField] protected DialogName nameType;

        public void OnEnable()
        {
            CheckIsCanShow(out bool isCanShow);
            gameObject.SetActiveVirtual(isCanShow);
            if (isCanShow)
            {
                BindShowAction();
                RefreshItem();
            }
        }



        public void OnDisable()
        {
            RemoveBindAction();
        }

        /// <summary>
        /// 显示Dialog
        /// </summary>
        protected virtual void ShowDialog()
        {
            DialogManager.Instance.ShowDialog(nameType);
        }

        /// <summary>
        /// 移出绑定
        /// </summary>
        protected virtual void RemoveBindAction()
        {
            iconButton.onClick.RemoveAllListeners();
        }


        /// <summary>
        /// 绑定显示功能
        /// </summary>
        protected virtual void BindShowAction()
        {
            if (nameType == DialogName.Null)
            {
                return;
            }

            iconButton.onClick.AddListener(ShowDialog);
        }
        
        
        /// <summary>
        /// 判断活动是否显示
        /// </summary>
        /// <param name="isCanShow"></param>
        protected virtual void CheckIsCanShow(out bool isCanShow)
        {
            isCanShow = true;
        }
        
        /// <summary>
        /// 刷新显示逻辑
        /// </summary>
        protected virtual void RefreshItem()
        {
        }
    }
}