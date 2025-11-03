using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog
{
    public enum DialogName
    {
        Null,
        SettingDialog,
        RatingStarDialog,
        RatingSimpleDialog,
        ADLoadingDialog,
        YesNoDialog,
        RemoveAdDialog,
        GiftClaimDialog,
        SlotMciDialog,
        RedeemDialog,
        RedeemInfoDialog,
        BigTurntableDialog
    }

    public class DialogContent
    {
    }

    /// <summary>
    /// 类模板接受DialogContext
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Dialog<T> : Dialog where T : DialogContent
    {
        protected T Content;

        public virtual void ShowDialogWithContext(T outContent)
        {
            SetContext(outContent);
            ShowDialog();
        }

        private void SetContext(T context)
        {
            Content = context;
        }
    }


    public class Dialog : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        protected Camera Camera { get; set; }
        public Action OnOpen = delegate { };
        public Action OnClose = delegate { };
        public int Order { get; set; } = 0;


        public virtual void ShowDialog()
        {
            Activate();
        }

        public virtual void CloseDialog()
        {
            Deactivate();
        }


        public virtual void Deactivate()
        {
            if (!DialogManager.ListActiveDialog.Contains(this))
            {
                LDebug.LogError("Dialog", "Deactivate no contain dialog.");
                return;
            }

            StaticModule.UIPoint_Dialog_Exit(GetDialogName());
            DialogManager.ListActiveDialog.Remove(this);
            //            GetComponentInChildren<Canvas>().sortingOrder = 0;
            if (animator != null)
            {
                AnimatorHelper.SetTrigger(animator, "Hide");
                JobUtils.Delay(0.15f, () => { gameObject.SetActive(false); });
            }
            else
            {
                gameObject.SetActive(false);
            }

            AudioClipHelper.Instance.PlayClose();
            OnClose?.Invoke();
            OnClose = null;
            
            if (!DialogManager.Instance.GetIsActiveDialog())
            {
                ADMudule.ShowBanner();
            }
        }

        public virtual void Deactivate(bool isPlayCloseAudio)
        {
            if (!DialogManager.ListActiveDialog.Contains(this))
            {
                LDebug.LogError("Dialog", "Deactivate no contain dialog.");
                return;
            }

            StaticModule.UIPoint_Dialog_Exit(GetDialogName());
            DialogManager.ListActiveDialog.Remove(this);
            //            GetComponentInChildren<Canvas>().sortingOrder = 0;
            if (animator != null)
            {
                AnimatorHelper.SetTrigger(animator, "Hide");
                JobUtils.Delay(0.15f, () => { gameObject.SetActive(false); });
            }
            else
            {
                gameObject.SetActive(false);
            }

            if (isPlayCloseAudio)
                AudioClipHelper.Instance.PlayClose();
            OnClose?.Invoke();
            OnClose = null;
            
            
            if (!DialogManager.Instance.GetIsActiveDialog())
            {
                ADMudule.ShowBanner();
            }
        }


        public virtual void DestroyDialog()
        {
            // 1. 执行关闭前的回调和统计逻辑（与 CloseDialog 保持一致）
            StaticModule.UIPoint_Dialog_Exit(GetDialogName());
            OnClose?.Invoke();
            OnClose = null; // 避免重复调用

            // 2. 从活跃列表中移除（防止管理器残留引用）
            if (DialogManager.ListActiveDialog.Contains(this))
            {
                DialogManager.ListActiveDialog.Remove(this);
            }

            // 3. 播放关闭音效（可选，根据需求保留）
            AudioClipHelper.Instance.PlayClose();

            // 4. 如果有隐藏动画，等动画结束后再销毁；否则立即销毁
            if (animator != null)
            {
                // 触发隐藏动画
                AnimatorHelper.SetTrigger(animator, "Hide");
                // 延迟销毁（等待动画播放完成，时间需与动画时长匹配）
                JobUtils.Delay(0.15f, () =>
                {
                    // 通知管理器从对象池移除
                    RemoveFromManagerPool();
                    Destroy(gameObject);
                });
            }
            else
            {
                // 无动画，直接销毁
                RemoveFromManagerPool();
                Destroy(gameObject);
            }

            // 5. 如果没有活跃弹窗，显示Banner广告（与 CloseDialog 逻辑一致）
            if (!DialogManager.Instance.GetIsActiveDialog())
            {
                ADMudule.ShowBanner();
            }
        }
        private void RemoveFromManagerPool()
        {
            // 获取当前弹窗的唯一ID（与管理器中存储的ID匹配）
            string dialogID = GetDialogID();
            if (!string.IsNullOrEmpty(dialogID) && DialogManager.Instance._pool.ContainsKey(dialogID))
            {
                DialogManager.Instance._pool.Remove(dialogID);
            }
        }
        private string GetDialogID()
        {
            // 优先按类型获取ID（对应 GetDialog<T>()）
            Type dialogType = GetType();
            if (dialogType.IsGenericType && dialogType.GetGenericTypeDefinition() == typeof(Dialog<>))
            {
                // 泛型弹窗（如 Dialog<BookContent>）取原始类型名
                return dialogType.GetGenericArguments()[0].ToString();
            }
            // 非泛型弹窗按类名获取（对应 GetDialog(DialogName)）
            return dialogType.Name;
        }

        public virtual void Activate(int order = -1)
        {
            if (DialogManager.ListActiveDialog.Contains(this))
            {
                LDebug.LogError("Dialog", "Activate too many times.");
                return;
            }

            StaticModule.UIPoint_Dialog_Enter(GetDialogName());
            DialogManager.ListActiveDialog.Add(this);
            var sort = order == -1 ? 1000 + DialogManager.ListActiveDialog.Count : order;
            int sortingOrder = sort;
            Order = sort;
            SetCamera(UICamera.Instance.Camera);
            GetComponentInChildren<Canvas>().sortingOrder = sortingOrder;
            gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "UI";
            LDebug.Log("Dialog", "Show dialog  canvas sorting order " + sortingOrder);
            gameObject.SetActive(true);
            if (animator != null)
            {
                AnimatorHelper.SetTrigger(animator, "Show");
            }

        

            AudioClipHelper.Instance.PlayShowDialogClip();
            OnOpen?.Invoke();
            ADMudule.HideBanner();
        }

        public virtual void Activate(bool isPlayAudio, int order = -1)
        {
            if (DialogManager.ListActiveDialog.Contains(this))
            {
                LDebug.LogError("Dialog", "Activate too many times.");
                return;
            }

            StaticModule.UIPoint_Dialog_Enter(GetDialogName());
            DialogManager.ListActiveDialog.Add(this);
            var sort = order == -1 ? 1000 + DialogManager.ListActiveDialog.Count : order;
            int sortingOrder = sort;
            Order = sort;
            GetComponentInChildren<Canvas>().sortingOrder = sortingOrder;
            GetComponentInChildren<Canvas>().sortingLayerName = "UI";
            LDebug.Log("Dialog", "Show dialog  canvas sorting order " + sortingOrder);
            gameObject.SetActive(true);
            if (animator != null)
            {
                AnimatorHelper.SetTrigger(animator, "Show");
            }

            SetCamera(UICamera.Instance.Camera);
            if (isPlayAudio)
                AudioClipHelper.Instance.PlayShowDialogClip();
            OnOpen?.Invoke();
            
            ADMudule.HideBanner();
        }

        public void SetCamera(Camera camera)
        {
            Canvas canvas = GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
                Camera = camera;
                canvas.worldCamera = camera;
            }
        }

        private string GetDialogName()
        {
            string result = gameObject.name;
            result = result.Replace("(Clone)", "");
            return result;
        }
    }
}