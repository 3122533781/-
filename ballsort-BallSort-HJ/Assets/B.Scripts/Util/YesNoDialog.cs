using System;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;


public class YesNoDialogContent : DialogContent
{
    public string title;
    public string info;
    public string okText;
    public string noText;
    public Action onOk;
    public Action onNo;
}

public class YesNoDialog : Dialog
{
    public void Show(string title, string info, Action onOk, string okText, Action onNo, string noText)
    {
        _titleText.text = title;
        _okText.text = okText;
        _noText.text = noText;
        Show(info, onOk, onNo);
    }

    public void Show(string title, string info, Action onOk, string okText)
    {
        _titleText.text = title;
        _okText.text = okText;
        Show(info, onOk);
    }

    public void Show(string info, Action onOk, Action onCancel = null)
    {
        _onOk = onOk;
        _onCancel = onCancel;
        _btnNo.gameObject.SetActive(onCancel != null);
        _text.text = info;
        Activate();
    }

    private void OnEnable()
    {
        _btnOk.onClick.AddListener(ClickOk);
        _btnNo.onClick.AddListener(ClickCancel);
    }

    private void OnDisable()
    {
        _btnOk.onClick.RemoveListener(ClickOk);
        _btnNo.onClick.RemoveListener(ClickCancel);
    }

    private void ClickCancel()
    {
        _onCancel.Invoke();
        Deactivate();
    }

    private void ClickOk()
    {
        _onOk.Invoke();
        Deactivate();
    }

    [SerializeField] private Button _btnOk = null;
    [SerializeField] private Button _btnNo = null;
    [SerializeField] private Text _text = null;
    [SerializeField] private Text _okText = null;
    [SerializeField] private Text _noText = null;
    [SerializeField] private Text _titleText = null;

    private Action _onOk;
    private Action _onCancel;
}