using _02.Scripts.InGame;
using NPOI.SS.Formula.Functions;
using ProjectSpace.BubbleMatch.Scripts.Setting;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageOptionsDialog : Dialog
{

    [SerializeField] private LanguageItem prefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private Image maskBG;

    private void Start()
    {
        AwakeHandle();
    }
    public void AwakeHandle()
    {
        for (int i = 0; i < LocalizationConfig.Instance.LanguageList.Count; i++)
        {
            var obj = Instantiate(prefab, content);
            obj.Refresh(LocalizationConfig.Instance.LanguageList[i]);
        }

    }

    [SerializeField] protected Button closeBtn;
    private void OnEnable()
    {
        maskBG.sprite = InGameManager.Instance.bg.sprite;
        closeBtn.onClick.AddListener(CloseLanguageOptionsBtn);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(CloseLanguageOptionsBtn);

    }

    public void CloseLanguageOptionsBtn()
    {
        DialogManager.Instance.GetDialog<LanguageOptionsDialog>().CloseDialog();
    }


}
