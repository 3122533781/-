using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    public void SetTag(string tag)
    {
        _txtTag = tag;
        SetLocalizedText();
    }

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
        if (!string.IsNullOrEmpty(_txtTag.Trim()))
        {
            _thisText.SetLocalizedTextByTag(_txtTag);
        }
    }

    private void OnLanguageChangedEvent(string langCode)
    {
        SetLocalizedText();
    }

    [FormerlySerializedAs("txtTag")] [SerializeField]
    private string _txtTag;

    private Text _thisText;
}