using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
public class CheckinDialog : Dialog
{
    private void Start()
    {
        RefreshUI();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _entries.Count; i++)
        {
            _entries[i].OnClickCheckIn += ClickCheckin;
        }

        _btnClaim.onClick.AddListener(NormalClaim);
        //_btnDoubleClaim.onClick.AddListener(DoubleClaim);
        //_btnDoubleClaim.enabled = true;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _entries.Count; i++)
        {
            _entries[i].OnClickCheckIn -= ClickCheckin;
        }

        _btnClaim.onClick.RemoveListener(NormalClaim);
        //_btnDoubleClaim.onClick.RemoveListener(DoubleClaim);
    }

    private void NormalClaim()
    {
        ClaimReward(_currentEntry.Data);
        _currentEntry.PlayCheckinAnim();
        HandleCheckinOver();
        _btnClaim.enabled = false;
    }

    private void ClickCheckin(CheckinData data)
    {
        // ClaimReward(data);

        HandleCheckinOver();
    }

    private void DoubleClaim()
    {
        ADMudule.ShowRewardedAd("Checkin", ret =>
        {
            if (ret)
            {
                ClaimReward(_currentEntry.Data, true);
                _currentEntry.PlayCheckinAnim();
                HandleCheckinOver();
            }
        });
    }

    private void HandleCheckinOver()
    {
        //_btnDoubleClaim.enabled = false;
        Game.Instance.GetSystem<CheckinSystem>().PlusCheckinDay();
        JobUtils.Delay(0.5f, () =>
        {
            Deactivate();
            DialogManager.Instance.GetDialog<GetRewardDialog>().Init(Game.Instance.GetSystem<CheckinSystem>().GetTodayReward());
            //if (App.Instance.GetSystem<CheckinSystem>().CurrentCheckinDayIndex > 0)
            //{
            //    App.Instance.GetSystem<RatingSystem>().CheckAndShowRating();
            //}
        });
    }

    private void RefreshUI()
    {
        for (int i = 0; i < _entries.Count; i++)
        {
            int checkinIndex = Game.Instance.GetSystem<CheckinSystem>().CurrentCheckinDayIndex;
            CheckinData data = CheckinConfig.Instance.All[i];
            bool needOpenButton = checkinIndex + 1 == i && !Game.Instance.GetSystem<CheckinSystem>().HasCheckInToday;
            _entries[i].SetData(data, i <= checkinIndex, needOpenButton);
            if (needOpenButton)
            {
                _currentEntry = _entries[i];
            }
        }
    }

    private void ClaimReward(CheckinData checkinData, bool isDouble = false)
    {
        List<Reward> rewards = new List<Reward>();
        //foreach (Reward reward in checkinData.Rewards)
        //{
        //    Reward reward1 = new Reward()
        //    {
        //        type = reward.type,
        //        count = reward.count
        //    };
        //    if (isDouble)
        //    {
        //        reward1.count *= 2;
        //    }

        //    rewards.Add(reward1);
        //}

        //Game.Instance.GetSystem<RewardSystem>().AddManyRewardsOnce("CheckIn", rewards);
    }

    [SerializeField] private Button _btnClaim = null;
    [SerializeField] private List<CheckinDialogEntry> _entries = null;

    private CheckinDialogEntry _currentEntry;
}