using System.Collections.Generic;
using _02.Scripts.InGame.UI;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.InGame
{
    public class InGameWinReward : ElementUI<global::InGame>
    {
        [SerializeField] private Button adButton;
        [SerializeField] private Button receiveButton;
        [SerializeField] private Text moneyText;
        private List<MoneyReward> _rewardList;
        private MoneyReward _coinData;
        private MoneyReward _starData;
        private bool _isWatch = false;

        private void OnEnable()
        {
            adButton.onClick.AddListener(WatchAd);
            receiveButton.onClick.AddListener(NextLevelButton);
        }

        private void OnDisable()
        {
            adButton.onClick.RemoveAllListeners();
            receiveButton.onClick.RemoveAllListeners();
        }

        public override void Activate()
        {
            base.Activate();
            _isWatch = false;
            _rewardList = new List<MoneyReward>();
            if (Game.Instance.Model.IsWangZhuan())
            {

             
            }

            _starData = new MoneyReward { goodType = GoodType.Star, count = 3 };
            _rewardList.Add(_coinData);
            _rewardList.Add(_starData);
            Refresh();
        }

        private void Refresh()
        {
            moneyText.text = $"+${_coinData.GetCount()}";
            adButton.SetActiveVirtual(!_isWatch);
        }

        private void WatchAd()
        {
            ADMudule.ShowRewardedAd("WatchAd_GetRevocationTool", (isSuccess) =>
            {
                if (isSuccess)
                {
                    _isWatch = true;
                    PlayAnimeAndClose();
                }
            });
        }

        private void NextLevelButton()
        {
            Deactivate();
            //  ADMudule.ShowInterstitialAds("NoADShowAD", null);
            Game.Instance.LevelModel.EnterLevelID += 1;
            Game.Instance.LevelModel.MaxUnlockLevel.Value += 1;
            SoyProfile.Set(SoyProfileConst.NormalLevel, Game.Instance.LevelModel.EnterLevelID);
            Game.Instance.RestartGame("RestartCurrentLevel", Game.Instance.LevelModel.EnterCopies1ID,
                    CopiesType.SpecialLevel, forceShowAd: true);
        }

        private void PlayAnimeAndClose()
        {
            Deactivate();
            StarAnime(true);
        }

        private void CoinAnime()
        {
         
        }

        private void CheckAnimeOver()
        {
            JobUtils.Delay(0.5f, () =>
            {
                CheckIsShowRedeemInfoDialog();
                Game.Instance.NotUseCode();
                Game.Instance.GetSystem<RatingSystem>().CheckAndShowRating();
            });
        }

        private void CheckIsShowRedeemInfoDialog()
        {
            if (Game.Instance.Model.IsWangZhuan() && Game.Instance.LevelModel.EnterLevelID ==
                Game.Instance.LevelModel.MaxUnlockLevel.Value - 1 && Game.Instance.LevelModel.MaxUnlockLevel.Value == 2 &&
                !CPlayerPrefs.GetBool("isShowRedeemInfo", false))
            {
                DialogManager.Instance.ShowDialog(DialogName.RedeemInfoDialog);
            }
        }

        private void StarAnime(bool isPlayMoney)
        {
        
        }

   
    }
}