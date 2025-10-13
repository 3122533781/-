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
            LvelValue.text =$"距离开启新玩法还剩{CalculateLevel()}局";
            if (CalculateLevel() == 0)
                LvelValue.gameObject.SetActive(false);
            boxReward.Init(_coinData);
            skeletonGraphic.AnimationState.SetAnimation(0, "shengli_loops",true);
            AudioClipHelper.Instance.PlaySound(AudioClipEnum.Win);
        }

        private void OnDisable()
        {
            nextLevelButton.onClick.RemoveListener(NextLevelButton);

        }

        private void NextLevelButton()
        {
            Deactivate();
            Game.Instance.LevelModel.EnterLevelID += 1;
            Game.Instance.LevelModel.MaxUnlockLevel.Value += 1;
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
        private void EnterNextLevel()
        {
            if (Game.Instance.LevelModel.CopiesType == CopiesType.Thread)
            {
                if (Game.Instance.LevelModel.EnterLevelID == Game.Instance.LevelModel.MaxUnlockLevel.Value)
                {
                    Game.Instance.RestartGame("NextLevel", Game.Instance.LevelModel.EnterLevelID,
                        forceShowAd: !Game.Instance.Model.IsWangZhuan());
                    if (DialogManager.Instance.GetDialog<LevelUIDialog>() != null)
                        DialogManager.Instance.GetDialog<LevelUIDialog>().PassLastLevel();
                }
                else
                {
                    Game.Instance.RestartGame("NextLevel", Game.Instance.LevelModel.EnterLevelID + 1,
                        forceShowAd: !Game.Instance.Model.IsWangZhuan());
                }
            }
            else if (Game.Instance.LevelModel.CopiesType == CopiesType.SpecialLevel)
            {
                Game.Instance.RestartGame("NextLevel", Game.Instance.LevelModel.EnterLevelID,
                    forceShowAd: !Game.Instance.Model.IsWangZhuan());
            }
        }
    }
}