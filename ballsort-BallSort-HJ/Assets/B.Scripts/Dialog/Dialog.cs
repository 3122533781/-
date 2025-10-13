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
            GetComponentInChildren<Canvas>().sortingOrder = sortingOrder;
            GetComponentInChildren<Canvas>().sortingLayerName = "UI";
            LDebug.Log("Dialog", "Show dialog  canvas sorting order " + sortingOrder);
            gameObject.SetActive(true);
            if (animator != null)
            {
                AnimatorHelper.SetTrigger(animator, "Show");
            }

            SetCamera(UICamera.Instance.Camera);

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