using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        
    }

    public void Init(bool isOn)
    {
        IsOn = isOn;

        // 核心修改：将 x 轴改为 y 轴，使用 _onY/_offY 控制上下位置
        _toggleToy.DOAnchorPos(new Vector2(_toggleToy.anchoredPosition.x, IsOn ? _onY : _offY), _moveDuration)
            .OnComplete(() =>
            {
                SetImageState();
            });
    }

    private void ClickButton()
    {
        IsOn = !IsOn;

        // 同样修改：x 轴不变，只改 y 轴
        _toggleToy.DOAnchorPos(new Vector2(_toggleToy.anchoredPosition.x, IsOn ? _onY : _offY), _moveDuration)
            .OnComplete(() =>
            {
                SetImageState();
                OnValueChange.Invoke(IsOn);
            });
    }

    private void SetImageState()
    {
        _onImage.gameObject.SetActive(IsOn);
        if(IsOn)
        _toggleToy.gameObject.GetComponent<Image>().sprite = Toy1;
        else _toggleToy.gameObject.GetComponent<Image>().sprite = Toy2;
        _offImage.gameObject.SetActive(!IsOn);
    }

    [SerializeField] private Button _btnToggle = null;
    [SerializeField] private RectTransform _toggleToy = null;
    [SerializeField] private Image _onImage = null;
    [SerializeField] private Image _offImage = null;
    [SerializeField] private float _onY;   // 打开时的 y 轴位置
    [SerializeField] private float _offY;  // 关闭时的 y 轴位置
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private Sprite Toy1;
    [SerializeField] private Sprite Toy2;
}