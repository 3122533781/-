using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorADManager : MonoSingleton<EditorADManager>
{
    public void ShowBanner()
    {
//#if UNITY_EDITOR
        _bannerPanel.SetActive(true);
//#endif
    }

    public void HideBanner()
    {
//#if UNITY_EDITOR
        _bannerPanel.SetActive(false);
//#endif
    }

    public void ShowInterstitial(Action onCompleted)
    {
//#if UNITY_EDITOR
        _onPlayInterstitialCompleted = onCompleted;
        _interstitialPanel.SetActive(true);
//#endif
    }

    public void ShowRewarded(Action<bool> onCompleted)
    {
//#if UNITY_EDITOR
        _onPlayRewardedCompleted = onCompleted;
        _rewardedPanel.SetActive(true);
//#endif
    }

    private void OnEnable()
    {
        _btnCloseInterstitial.onClick.AddListener(CloseInterstitial);
        _btnCloseRewarded.onClick.AddListener(CloseRewarded);
        _btnCloseAndGetRewarded.onClick.AddListener(CloseRewardedAndGet);
    }

    private void OnDisable()
    {
        _btnCloseInterstitial.onClick.RemoveListener(CloseInterstitial);
        _btnCloseRewarded.onClick.RemoveListener(CloseRewarded);
        _btnCloseAndGetRewarded.onClick.RemoveListener(CloseRewardedAndGet);
    }

    private void CloseRewardedAndGet()
    {
        _rewardedPanel.SetActive(false);
        _onPlayRewardedCompleted.Invoke(true);
    }

    private void CloseRewarded()
    {
        _rewardedPanel.SetActive(false);
        _onPlayRewardedCompleted.Invoke(false);
    }

    private void CloseInterstitial()
    {
        _interstitialPanel.SetActive(false);
        _onPlayInterstitialCompleted?.Invoke();
    }


    [SerializeField] private Button _btnCloseInterstitial = null;
    [SerializeField] private Button _btnCloseRewarded = null;
    [SerializeField] private Button _btnCloseAndGetRewarded = null;
    [SerializeField] private GameObject _bannerPanel = null;
    [SerializeField] private GameObject _interstitialPanel = null;
    [SerializeField] private GameObject _rewardedPanel = null;

    private Action _onPlayInterstitialCompleted;
    private Action<bool> _onPlayRewardedCompleted;
}