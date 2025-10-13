using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSpace.BubbleMatch.Scripts.Setting
{
    public class LanguageItem : MonoBehaviour
    {
        [SerializeField] private Button itemButton;
        [SerializeField] private Text text;
        private Language item;

        private void OnEnable()
        {
            itemButton.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            itemButton.onClick.RemoveListener(Click);
        }


        public void Refresh(Language instanceLanguage)
        {
            item = instanceLanguage;
            text.text = item.LanguageName;
        }

        private void Click()
        {
            AudioClipHelper.Instance.PlayButtonTap();
            LocalizationManager.Instance.SetLanguage(item.LangaugeCode);
            DialogManager.Instance.GetDialog<LanguageOptionsDialog>().CloseDialog();

            DialogManager.Instance.GetDialog<OptionDialog>().showText.text = item.LanguageName;
            AudioClipHelper.Instance.PlayButtonTap();
            VibrationManager.Instance.SelectedBlockImpact();
        }
    }
}