using _02.Scripts.Config;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using RU;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace _02.Scripts.InGame.UI
{
    public class InGameSuccessUI : ElementUI<global::InGame>
    {
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button ADnextLevelButton;
        [SerializeField] private Text LvelValue;
        [SerializeField] private InGameBoxReward boxReward;
        [SerializeField] private SkeletonGraphic skeletonGraphic;
        private RewardData _coinData;
        private RewardData _skinData;
        private ThemeConfig config;
        private void Awake()
        {
            config = Resources.Load<ThemeConfig>("Configs/ThemeConfig");
        }
        private void OnEnable()
        {
            _coinData = Context.CellMapModel.LevelData.GetRandomCoin();
            nextLevelButton.onClick.AddListener(NextLevelButton);
            ADnextLevelButton.onClick.AddListener(ADNextLevelButton);
            LvelValue.text =$"距离开启新玩法还剩{CalculateLevel()}局";
            if (CalculateLevel() == 0)
                LvelValue.gameObject.SetActive(false);
            boxReward.Init(_coinData);
           // skeletonGraphic.AnimationState.SetAnimation(0, "shengli_loops",true);
            AudioClipHelper.Instance.PlaySound(AudioClipEnum.Win);
          
        }

        private void OnDisable()
        {
            nextLevelButton.onClick.RemoveListener(NextLevelButton);
            ADnextLevelButton.onClick.RemoveListener(ADNextLevelButton);
        }

        private void ADNextLevelButton()
        {
            Debug.Log("广告出现");
            SuccessTo();
            ADMudule.ShowRewardedAd("WatchAd_GetRevocationTool", (isSuccess) =>
            {
                if (isSuccess)
                {
                    Game.Instance.CurrencyModel.AddGoodCount(GoodType.Coin, 0, 20);
                    Deactivate();
                    SuccessTo();
                }
            });


          


        }
  private void NextLevelButton()
        {
            Deactivate();
            SuccessTo();
            Game.Instance.CurrencyModel.AddGoodCount(GoodType.Coin, 0, 10);
        }

        private void SuccessTo()
        {
            if (Game.Instance.LevelModel.PassLevelNumber.Value == Game.Instance.LevelModel.PassLevelTemp)
            {
                Game.Instance.LevelModel.PassLevelNumber.Value += 1;
                Game.Instance.LevelModel.PassLevelTemp += 1;
            }
            else
            {
                Debug.Log("关卡数小于");
            }

            Game.Instance.LevelModel.EnterLevelID += 1;
            Game.Instance.LevelModel.MaxUnlockLevel.Value += 1;
            _context.GetView<InGamePlayingUI>()._barBegin = 0f;
            _context.GetModel<InGameModel>().EndFinishNumber = 0;


            SoyProfile.Set(SoyProfileConst.NormalLevel, Game.Instance.LevelModel.EnterLevelID);
            Game.Instance.RestartGame("RestartCurrentLevel", Game.Instance.LevelModel.EnterCopies1ID,
                    CopiesType.SpecialLevel, forceShowAd: true);
        }




        private int  CalculateLevel()
        {
            int max = Game.Instance.LevelModel.MaxUnlockLevel.Value;
            int[] levels = config.face_unlock_levels;

            // 遍历找到第一个大于 input 的值
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i] > max)
                {
                    return levels[i] - max;
                }
            }

            // 如果 input 大于等于最后一个值，返回 0 或其他默认值
            return 0;
        }
       
    }
}