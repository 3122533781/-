using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageToggle : MonoBehaviour
{
    
    public Text showText;
   
    string language => transform.GetChild(0).GetComponent<Text>().text;

    public void SetLanguage(bool isOn)
    {
        if (isOn)
        {
            DialogManager.Instance.GetDialog<OptionDialog>().showText.text = language;
        }
    }
}
