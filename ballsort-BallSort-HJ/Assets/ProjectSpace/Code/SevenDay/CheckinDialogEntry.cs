using System;
using DG.Tweening;
using Lei31.Localizetion;
using UnityEngine;
using UnityEngine.UI;

public class CheckinDialogEntry : MonoBehaviour
{
    public Action<CheckinData> OnClickCheckIn = delegate { };
    public CheckinData Data { get; set; }

    public void SetData(CheckinData data, bool hasCheckin, bool openButton)
    {
        Data = data;
        if(_hasCheckinImage != null)
        {
            _hasCheckinImage.gameObject.SetActive(hasCheckin);
        }
        if(_titleText!=null)
        _titleText.text = $"第{data.DayNumber}天";

        _btnCheckIn.interactable = openButton;
        if(GaoLiang!=null)
            GaoLiang.SetActive(openButton);
        
        SetRewards();
    }

    public void PlayCheckinAnim()
    {
        if (_hasCheckinImage != null)
        {
            _hasCheckinImage.gameObject.SetActive(true);
            //_hasCheckinImage.color =
            //    new Color(_hasCheckinImage.color.r, _hasCheckinImage.color.g, _hasCheckinImage.color.b, 0);
            _hasCheckinImage.transform.localScale = Vector3.one * 3f;

            _hasCheckinImage.DOFade(0.5f, 0.5f);
            _hasCheckinImage.transform.DOScale(1, 0.5f);
            GaoLiang.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _btnCheckIn.onClick.AddListener(ClickCheckIn);
    }

    private void OnDisable()
    {
        _btnCheckIn.onClick.RemoveListener(ClickCheckIn);
    }

    private void ClickCheckIn()
    {
        _btnCheckIn.interactable = false;
        OnClickCheckIn.Invoke(Data);
        PlayCheckinAnim();
    }

    private void SetRewards()
    {
        if(Data==null)
        {
            Debug.Log("值为空");
        }
        if (_oneRewardItem == null)
        {
            Debug.Log("值2为空");
        }
        if (Data.Rewards.Count > 0)
        {
            _oneRewardItem.SetData(Data.Rewards[0]);
        }

    }

    [SerializeField] private Image _hasCheckinImage = null;
    [SerializeField] private Text _titleText = null;
    [SerializeField] private Button _btnCheckIn = null;
    [SerializeField] private Transform _rewardContent = null;
    [SerializeField] private GameObject _rewardItemPrefab = null;
    [SerializeField] private RewardItemBaseUI _oneRewardItem = null;
    [SerializeField] private GameObject GaoLiang = null;
}