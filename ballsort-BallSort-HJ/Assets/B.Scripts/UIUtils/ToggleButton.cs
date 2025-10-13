using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
    public Action<bool, ToggleButton> OnChange = delegate { };

    public enum ButtonState
    {
        State12,
        State13
    }

    [SerializeField] private ButtonState buttonState;

    public bool IsOn
    {
        get => _isOn;
        set
        {
            if (value != _isOn)
            {
                _isOn = value;
                SetObjectState();
                OnChange.Invoke(_isOn, this);
            }
        }
    }

    public void Reset(bool isOn)
    {
        _isOn = isOn;
        SetObjectState();
    }

    private void OnEnable()
    {
        //Cross.SetActive(IsOn);
        SetObjectState();
        if (buttonState == ButtonState.State12)
        {
            _button.onClick.AddListener(ClickChange);
        }
        else
        {
            _button.onClick.AddListener(ClickButton);
        }
    }

    private void OnDisable()
    {
        if (buttonState == ButtonState.State12)
        {
            _button.onClick.RemoveListener(ClickChange);
        }
        else
        {
            _button.onClick.RemoveListener(ClickButton);
        }
    }

    private void ClickButton()
    {
        IsOn = true;
    }

    public void ClickChange()
    {
        IsOn = !IsOn;
        if (Cross != null)
            Cross.SetActive(!IsOn);
        SetObjectState();
    }

    private void SetObjectState()
    {
        _onObject.SetActive(IsOn);
        _offObject.SetActive(!IsOn);
        if (Cross != null)
            Cross.SetActive(!IsOn);

        ChangeOutlineColor();
    }

    private void ChangeOutlineColor()
    {
        if (_outline != null && _outlineColors != null && _outlineColors.Count >= 2)
        {
            _outline.Refresh(_outlineColors[IsOn ? 0 : 1], 6);
        }
    }

    [SerializeField] private Button _button = null;
    [SerializeField] private GameObject _onObject = null;
    [SerializeField] private GameObject _offObject = null;
    [SerializeField] private GameObject Cross = null;
    [SerializeField] private Text2DOutline _outline = null;
    [SerializeField] private List<Color> _outlineColors = null;

    private bool _isOn;
}