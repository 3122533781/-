using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationModel
{
    public const string EnglishLanguageCode = "en";
    public const string ChineseLanguageCode = "cn";
    public string CurrentLanguageCode { get; private set; }
    public Language CurrentLanguage { get; set; }

    public bool IsReadSuccess { get; set; }

    public bool IsUsingEnglish
    {
        get { return CurrentLanguageCode == EnglishLanguageCode; }
    }

    public bool IsUsingChinese
    {
        get { return CurrentLanguageCode == ChineseLanguageCode; }
    }

    public LocalizationModel()
    {
        _tagToText = new Dictionary<string, string>();
        if (Storage.Instance.HasKey(CurrentLangSaveKey))
        {
            CurrentLanguageCode = Storage.Instance.GetString(CurrentLangSaveKey, DefaultLanguageCode);
        }
        else
        {
            CurrentLanguageCode = GetSystemLanguageCode();
        }
    }

    public string GetTextByTag(string tag, params System.Object[] param)
    {
        if (_tagToText.ContainsKey(tag))
        {
            if (param == null || !param.Any())
            {
                param = new object[] {""};
            }

            return string.Format(_tagToText[tag], param);
        }
        else
        {
            return "(unset tag)";
        }
    }

    public void SetLanguage(string code, XElement rootElement, Language language)
    {
        CurrentLanguage = language;
        CurrentLanguageCode = code;
        Storage.Instance.SetString(CurrentLangSaveKey, code);
        LoadAllText(rootElement);
        LocalizationEvent.OnLanguageChangedEvent.Invoke(code);
    }

    public void LoadAllText(XElement rootElement)
    {
        _tagToText.Clear();
        foreach (XElement childElement in rootElement.Elements())
        {
            string name = childElement.Attribute("name").Value;
            string text = childElement.Attribute("text").Value;
            if (!_tagToText.ContainsKey(name))
            {
                _tagToText.Add(name, text);
            }
            else
            {
                Debug.LogError($"Add tag :{name} is duplicate");
            }
        }
    }

    public string GetSystemLanguageCode()
    {
        string SystemLanguageCode = "en";

        if (Application.systemLanguage == SystemLanguage.Chinese ||
            Application.systemLanguage == SystemLanguage.ChineseSimplified)
        {
            SystemLanguageCode = "cn";
        }
        else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            SystemLanguageCode = "cns";
        }

        LDebug.Log($"Current systemlanguageName : {Application.systemLanguage.ToString()}");

        return SystemLanguageCode;
    }

    private string CurrentLangSaveKey = "CurrentLangSaveKey";
    private const string DefaultLanguageCode = "en";

    private Dictionary<string, string> _tagToText;
}