using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TermPrivacyDialog : Dialog
{
    public ShowType aboutToShowType;
    [SerializeField] private Text titleText;
    [SerializeField] private Text termText;
    [SerializeField] private Text policyText;
    [SerializeField] private ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void ShowDialog()
    {
        base.ShowDialog();
        if (aboutToShowType == ShowType.PrivacyPolicy)
        {
            titleText.text = "Privacy Policy";
            termText.gameObject.SetActive(false);
            policyText.gameObject.SetActive(true);
            scrollRect.content = policyText.rectTransform;
        }
        else {
            titleText.text = "Terms Of Use";
            termText.gameObject.SetActive(true);
            policyText.gameObject.SetActive(false);
            scrollRect.content = termText.rectTransform;
        }
    }
}

public enum ShowType
{
    TermOfUse,
    PrivacyPolicy
}
