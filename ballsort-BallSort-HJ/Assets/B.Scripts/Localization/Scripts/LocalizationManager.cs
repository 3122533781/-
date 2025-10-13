using UnityEngine;
using System.Xml.Linq;
using System;
using Fangtang.Utils;

public class LocalizationManager : Singleton<LocalizationManager>
{
    public LocalizationModel Model { get; set; }

    public bool IsAsianCharactor
    {
        get { return Model.CurrentLanguageCode == "zh" || Model.CurrentLanguageCode == "jp"; }
    }
    
    public bool IsUsingChinese
    {
        get { return Model.IsUsingChinese; }
    }

    public void SetLanguage(string code)
    {
        XDocument document = null;
        bool readSuccess = ReadFormXML(code, out document);
        XElement root = document.Element("Resources");
        Model.IsReadSuccess = readSuccess;
        Model.SetLanguage(code, root, LocalizationConfig.Instance.GetLanguage(code));
    }

    public string GetTextByTag(string tag, params System.Object[] param)
    {
        return Model.GetTextByTag(tag, param);
    }

    private bool ReadFormXML(string langCode, out XDocument xDoc)
    {
        var txtFile = Resources.Load("strings-" + langCode.ToLower()) as TextAsset;

        if (txtFile == null)
        {
            LDebug.Log("Localization", $"Load data file {langCode.ToLower()} failed ");
            txtFile = Resources.Load("strings-en") as TextAsset;
            xDoc = XDocument.Parse(txtFile.text.ToString());
            return false;
        }
        else
        {
            LDebug.Log("Localization", $"Load data file {langCode.ToLower()} success ");
            xDoc = XDocument.Parse(txtFile.text.ToString());
            return true;
        }
    }

    private LocalizationManager()
    {
        Model = new LocalizationModel();
        Model.CurrentLanguage = LocalizationConfig.Instance.GetLanguage(Model.CurrentLanguageCode);
        Init();
    }

    private void Init()
    {
        XDocument document = null;
        bool readSuccess = ReadFormXML(Model.CurrentLanguageCode, out document);
        XElement root = document.Element("Resources");
        Model.IsReadSuccess = readSuccess;
        if (!readSuccess)
        {
            Model.SetLanguage(LocalizationModel.EnglishLanguageCode, root,
                LocalizationConfig.Instance.GetLanguage(LocalizationModel.EnglishLanguageCode));
        }
        else
        {
            Model.LoadAllText(root);
        }
    }
}

public class SpecialTagReplacer
{
    public string specialTag;
    public string replaceValue;

    public SpecialTagReplacer(string _specialTag, string _replaceValue)
    {
        this.specialTag = _specialTag;
        this.replaceValue = _replaceValue;
    }
}

[Serializable]
public class Language
{
    public string LanguageName;
    public string LangaugeCode;
    public LanguageFontData FontData;
}

[Serializable]
public class LanguageFontData
{
    public Font Font;
    public float FontTextUnitWidth;
    public float FontTextUnitHeight;
}