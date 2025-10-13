using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomToggleGroup : MonoBehaviour
{
    public Action<int> OnToggleChange = delegate { };

    public void Init(int index)
    {
        _buttons[index].Selected();
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

    private void ToggleChange(bool isOn, CustomToggleButton button)
    {
        if (isOn)
        {
            OnToggleChange.Invoke(_buttons.IndexOf(button));
        }
    }

    [SerializeField] private List<CustomToggleButton> _buttons = null;
    
}
