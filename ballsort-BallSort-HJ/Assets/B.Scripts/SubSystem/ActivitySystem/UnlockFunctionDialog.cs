using System;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSpace.BubbleMatch.Scripts.SubSystem.ActivitySystem
{
    public class UnlockFunctionDialog : Dialog
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button goButton;
        private UnlockActiveData unlockActiveData;

        public void ShowDialog(UnlockActiveData isUnlock)
        {
            base.ShowDialog();
            unlockActiveData = isUnlock;
            // icon.sprite = SpriteManager.Instance.GetActiveIcon(isUnlock.ID - 1);
            icon.SetNativeSize();
            goButton.onClick.AddListener(ClickGO);
        }

        public override void CloseDialog()
        {
            base.CloseDialog();
            goButton.onClick.RemoveListener(ClickGO);
        }

        private void ClickGO()
        {
            Game.Instance.BackHome(GetBackHomeAction());
            CloseDialog();
        }

        public Action GetBackHomeAction()
        {
            Action res = null;
            res = () => { DialogManager.Instance.ShowDialog(unlockActiveData.id); };

            return res;
        }
    }
}