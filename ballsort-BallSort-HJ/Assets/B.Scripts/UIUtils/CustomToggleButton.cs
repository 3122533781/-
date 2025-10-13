using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CustomToggleButton : MonoBehaviour
{
    public Action<bool, CustomToggleButton> OnChange = delegate { };

    public void Selected()
    {
        _toggle.Get(gameObject).isOn = true;
    }

    private void OnEnable()
    {
        _toggle.Get(gameObject).onValueChanged.AddListener(ToggleValueChanged);
    }

    private void OnDisable()
    {
        _toggle.Get(gameObject).onValueChanged.RemoveListener(ToggleValueChanged);
    }

    private void ToggleValueChanged(bool isOn)
    {
        OnChange.Invoke(isOn, this);
    }

    private DelayGetComponent<Toggle> _toggle = new DelayGetComponent<Toggle>();
}