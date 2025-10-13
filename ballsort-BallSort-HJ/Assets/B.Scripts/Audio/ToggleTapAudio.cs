using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleTapAudio : MonoBehaviour
{
    private void OnEnable()
    {
        _button.Get(gameObject).onValueChanged.AddListener(ClickButton);
    }

    private void OnDisable()
    {
        _button.Get(gameObject).onValueChanged.RemoveListener(ClickButton);
    }

    private void ClickButton(bool isOn)
    {
        AudioClipHelper.Instance.PlayButtonTap();
    }

    private DelayGetComponent<Toggle> _button = new DelayGetComponent<Toggle>();
}
