using UnityEngine;
using UnityEngine.UI;

public class LocalizationLanguageButton : MonoBehaviour
{
    [SerializeField] public string LanguageCode = "en";
    [SerializeField] public ToggleButton Toggle; 

    public void SetData(Language data)
    {
        _languageName.text = data.LanguageName;
        LanguageCode = data.LangaugeCode;
        bool isSelected = LocalizationManager.Instance.Model.CurrentLanguage == data;
        _languageName.color = isSelected ? _selectedColor : _unselectedColor;
    }

    public void Refresh()
    {
        bool isSelected = LocalizationManager.Instance.Model.CurrentLanguage.LangaugeCode == LanguageCode;
        _languageName.color = isSelected ? _selectedColor : _unselectedColor;
    }

    private void OnDisable()
    {
    }

    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private Text _languageName;
}