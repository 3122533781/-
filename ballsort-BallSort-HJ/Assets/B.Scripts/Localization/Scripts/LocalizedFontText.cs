using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedFontText : MonoBehaviour
{
    private void Awake()
    {
        _thisText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        LocalizationEvent.OnLanguageChangedEvent += OnLanguageChangedEvent;
        Invoke("SetLocalizedText", 0.01F);
    }

    private void OnDisable()
    {
        LocalizationEvent.OnLanguageChangedEvent -= OnLanguageChangedEvent;
    }

    private void SetLocalizedText()
    {
        _thisText.font = LocalizationManager.Instance.Model.CurrentLanguage.FontData.Font;
    }

    private void OnLanguageChangedEvent(string langCode)
    {
        SetLocalizedText();
    }

    private Text _thisText;
}