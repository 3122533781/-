using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocalizationPanel : MonoBehaviour, IPointerDownHandler
{
    private void OnEnable()
    {
        if (_languageButtons == null)
        {
            CreateButtons();
        }

        SetFirst();
        Refresh();
        _group.OnToggleChange += OnToggleChange;
    }

    private void OnDisable()
    {
        _group.OnToggleChange -= OnToggleChange;
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        for (int i = 0; i < _languageButtons.Count; i++)
        {
            _languageButtons[i].Refresh();
        }
    }

    private void SetFirst()
    {
        _selectedIndex = _languageButtons.FindIndex(x =>
            x.LanguageCode == LocalizationManager.Instance.Model.CurrentLanguage.LangaugeCode);
        _languageButtons[_selectedIndex].Toggle.Reset(true);
        //SetSelectText(LocalizationConfig.Instance.GetLanguage(_languageButtons[_selectedIndex].LanguageCode));
    }

    public void SetSelectText()
    {
        selectText.text = LocalizationManager.Instance.Model.CurrentLanguage.LanguageName;
    }

    private void OnToggleChange(int index)
    {
        _selectedIndex = index;
        LocalizationManager.Instance.SetLanguage(_languageButtons[_selectedIndex].LanguageCode);
        SetSelectText();
        gameObject.SetActive(false);
    }

    private void CreateButtons()
    {
        _languageButtons = new List<LocalizationLanguageButton>();
        for (int i = 0; i < LocalizationConfig.Instance.LanguageList.Count; i++)
        {
            LocalizationLanguageButton button = CreateButton(LocalizationConfig.Instance.LanguageList[i]);
            _languageButtons.Add(button);
        }
    }

    private LocalizationLanguageButton CreateButton(Language data)
    {
        LocalizationLanguageButton result = Instantiate(_languagePrefab, _languageParent);
        result.SetActiveVirtual(true);
        _group.AddToggle(result.Toggle);
        result.SetData(data);
        return result;
    }


    private bool _isSelf;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            if (!_isSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _isSelf = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isSelf = true;
    }

    [SerializeField] private LocalizationLanguageButton _languagePrefab;
    [SerializeField] private RectTransform _languageParent;
    [SerializeField] private ToggleGroup _group;
    [SerializeField] private Text selectText;

    private List<LocalizationLanguageButton> _languageButtons;
    private int _selectedIndex;
}