using System.Collections.Generic;
using UnityEngine;

public class LocalizationConfig : ScriptableSingleton<LocalizationConfig>
{
    [SerializeField] public List<Language> LanguageList;

    public Language GetLanguage(string code)
    {
        if (_codeToLanguage == null)
        {
            InitDic();
        }

        if (_codeToLanguage.ContainsKey(code))
        {
            return _codeToLanguage[code];
        }

        return null;
    }

    private void InitDic()
    {
        _codeToLanguage = new Dictionary<string, Language>();
        foreach (Language language in LanguageList)
        {
            _codeToLanguage.Add(language.LangaugeCode, language);
        }
    }

    private Dictionary<string, Language> _codeToLanguage;
}