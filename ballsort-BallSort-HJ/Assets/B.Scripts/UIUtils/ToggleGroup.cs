using System;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGroup : MonoBehaviour
{
    public Action<int> OnToggleChange = delegate { };

    public void Init(int index)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].Reset(i == index);
        }
    }

    public void AddToggle(ToggleButton button)
    {
        _buttons.Add(button);
        if (isActiveAndEnabled)
        {
            button.OnChange += ToggleChange;
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].OnChange += ToggleChange;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].OnChange -= ToggleChange;
        }
    }

    private void ToggleChange(bool isOn, ToggleButton button)
    {
        if (isOn)
        {
            SwithOtherOff(button);
            OnToggleChange.Invoke(_buttons.IndexOf(button));
        }
    }

    private void SwithOtherOff(ToggleButton onTarget)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (_buttons[i] != onTarget)
            {
                _buttons[i].IsOn = false;
            }
        }
    }

    [SerializeField] private List<ToggleButton> _buttons = null;
}