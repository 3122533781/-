using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    public Action<bool> OnValueChange = delegate { };

    [SerializeField] public bool IsOn;

    private void OnEnable()
    {
        _btnToggle.onClick.AddListener(ClickButton);
    }

    private void OnDisable()
    {
        _btnToggle.onClick.RemoveListener(ClickButton);
    }

    public void Init()
    {
        Vector2 targetPos = new Vector2(IsOn ? _onX : _offX, _toggleToy.anchoredPosition.y);
        _toggleToy.anchoredPosition = targetPos;
        SetImageState();
    }

    private void ClickButton()
    {
        IsOn = !IsOn;

        _toggleToy.DOAnchorPos(
            new Vector2(IsOn ? _onX : _offX, _toggleToy.anchoredPosition.y),
            _moveDuration
        ).OnComplete(() =>
        {
            SetImageState();
            OnValueChange.Invoke(IsOn);
        });
    }

    private void SetImageState()
    {
        _onImage.gameObject.SetActive(IsOn);
        _toggleToy.GetComponent<Image>().sprite = IsOn ? Toy1 : Toy2;
        _offImage.gameObject.SetActive(!IsOn);
    }

    [SerializeField] private Button _btnToggle;
    [SerializeField] private RectTransform _toggleToy;
    [SerializeField] private Image _onImage;
    [SerializeField] private Image _offImage;
    [SerializeField] private float _onX;
    [SerializeField] private float _offX;
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private Sprite Toy1;
    [SerializeField] private Sprite Toy2;
}