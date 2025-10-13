using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTapAudio : MonoBehaviour
{
    private void OnEnable()
    {
        _button.Get(gameObject).onClick.AddListener(ClickButton);
    }

    private void OnDisable()
    {
        _button.Get(gameObject).onClick.RemoveListener(ClickButton);
    }

    private void ClickButton()
    {
        AudioClipHelper.Instance.PlayButtonTap();
    }

    private DelayGetComponent<Button> _button = new DelayGetComponent<Button>();
}
