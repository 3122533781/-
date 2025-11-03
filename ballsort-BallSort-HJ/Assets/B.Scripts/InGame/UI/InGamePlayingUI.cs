using System.Collections.Generic;
using _02.Scripts.Config;
using _02.Scripts.Home.Active;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using _02.Scripts.InGame.Controller;
using _02.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;
using UnityEngine;
using UnityEngine.UI;
using System;

using _02.Scripts.Home.Active;
using Spine.Unity;

namespace _02.Scripts.InGame.UI
{
    public class InGamePlayingUI : ElementUI<global::InGame>
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private Button btnback;
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button skinEntryBtn;
        [SerializeField] private Button restartSetting;
        [SerializeField] private Button revocationTool;
        [SerializeField] private Button addNewPipeTool;
        [SerializeField] private Button RemoveTool;
        [SerializeField] private SmartLocalizedText levelTxt;
        [SerializeField] private SmartLocalizedText ProgressTxt;
        [SerializeField] private SmartLocalizedText TimeTxt;
        [SerializeField] private Text addPipeTxt;
        [SerializeField] private Text removeToolTxt;
        [SerializeField] private SmartLocalizedText RebackTxt;
        [SerializeField] private Image revocationToolAdIcon;
        [SerializeField] private Image addPipeToolAdIcon;
        [SerializeField] private GameObject PlusAd;
        [SerializeField] private GameObject RemoveAd;
        [SerializeField] private GameObject RebackAd;
        [SerializeField] private List<GameObject> redeemBtn;
        [SerializeField] private ProgressBar _bar = null;
        [SerializeField] private ProgressBarMarkerGenerator _barNumber;


        [SerializeField] private SkeletonGraphic BeginAnim;
        public float _barBegin = 0f;
      
        private void OnEnable()
        {
            if (Game.Instance.Model.IsWangZhuan())
            {
                btnback.SetActive(false);
                btnSetting.gameObject.SetActive(true);
            }
            btnSetting.onClick.AddListener(ClickSetting);
            btnback.onClick.AddListener(ClickBack);
            revocationTool.onClick.AddListener(RevocationTool);
            addNewPipeTool.onClick.AddListener(AddNewPipe);
            RemoveTool.onClick.AddListener(RemoveBall);

            SetBackground(false);
            Game.Instance.CurrencyModel.RegisterToolChangeAction(GoodType.Tool, GoodSubType.AddPipe, RefreshUI);
            Game.Instance.CurrencyModel.RegisterToolChangeAction(GoodType.Tool, GoodSubType.RevocationTool, RefreshUI);
            Game.Instance.CurrencyModel.RegisterToolChangeAction(GoodType.Tool, GoodSubType.RemoveTool, RefreshUI);

            Game.Instance.Model.CanRedeem.OnValueChange += HandleCanRedeemChange;
            EventDispatcher.instance.Regist(AppEventType.PlayerStepCountChange, RefreshUI);
        }

        private void OnDisable()
        {
            btnSetting.onClick.RemoveListener(ClickSetting);
            btnback.onClick.RemoveListener(ClickBack);
            revocationTool.onClick.RemoveListener(RevocationTool);
            addNewPipeTool.onClick.RemoveListener(AddNewPipe);
            RemoveTool.onClick.RemoveListener(RemoveBall);

            Game.Instance.Model.CanRedeem.OnValueChange -= HandleCanRedeemChange;
            EventDispatcher.instance.UnRegist(AppEventType.PlayerStepCountChange, RefreshUI);
            Game.Instance.CurrencyModel.UnregisterToolChangeAction(GoodType.Tool, GoodSubType.AddPipe, RefreshUI);
            Game.Instance.CurrencyModel.UnregisterToolChangeAction(GoodType.Tool, GoodSubType.RevocationTool, RefreshUI);
            Game.Instance.CurrencyModel.UnregisterToolChangeAction(GoodType.Tool, GoodSubType.RemoveTool, RefreshUI);
        }

        public void SetBar()
        {
            float temp = Context.GetCompletionRate(Game.Instance.LevelModel.TypeNumber);
            Debug.Log($"真正进度条222：{_barBegin}%" + "  " + temp);
            _barBegin += temp;
            Debug.Log($"真正进度条：{_barBegin}%");
            _bar.UpdateProgressSmooth(_barBegin / 100f);
        }

        public void ShowGUide(int temp)
        {
            switch (temp)
            {
                case 5:
                    if(Game.Instance.LevelModel.GuideGroup.Value<1)
                    DialogManager.Instance.GetDialog<GuideDialog>().InitDialog(1);
                    break;
                case 10:
                    if (Game.Instance.LevelModel.GuideGroup.Value<2)
                        DialogManager.Instance.GetDialog<GuideDialog>().InitDialog(2);
                    break;
                case 20:
                    if (Game.Instance.LevelModel.GuideGroup.Value <3)
                        DialogManager.Instance.GetDialog<GuideDialog>().InitDialog(3);
                    break;
                case 30:
                    if (Game.Instance.LevelModel.GuideGroup.Value <4)
                        DialogManager.Instance.GetDialog<GuideDialog>().InitDialog(4);
                    break;
                case 40:
                    if (Game.Instance.LevelModel.GuideGroup.Value <5)
                        DialogManager.Instance.GetDialog<GuideDialog>().InitDialog(5);
                    break;
            }
        }

        public void SetBarNumberTo(int temp)
        {
            _barNumber.GenerateEqualMarkers(temp, 10f);
        }

        public void SetBarToZero()
        {
            _bar.UpdateProgress(0);
         //   TypeNumber = Context.GetBallsType();
          //  Debug.Log("数量为"+TypeNumber);
        }

        private void RefreshUI(int arg1, int arg2)
        {
            RefreshUI();
        }

        private void RefreshUI(object[] objs)
        {
            RefreshUI();
        }
        public void PlayBeginAnimation(Action action)
        {
            BeginAnim.gameObject.SetActive(true);
           
            BeginAnim.AnimationState.SetAnimation(0, "animation", false);
            var trackEntry = BeginAnim.AnimationState.SetAnimation(0, "animation", false);
            // 动画播放完成后执行回调
            trackEntry.Complete += (entry) =>
            {
                action?.Invoke(); // 空值判断，避免空引用错误
            };

        }
        private void ClickSetting()
        {
            DialogManager.Instance.GetDialog<OptionDialog>().ShowDialog();
        }

        public void SetTimeText(string text)
        {
            TimeTxt.text = text;
        }

        private void ClickBack()
        {
            TransitionManager.Instance.Transition(0.5f, () => { SceneManager.LoadScene("InGame"); },
                   0.5f);
        }

        public void SetTimeActive(bool b)
        {
        }

        public void SetTime(int secound)
        {
        }

        public void SetBackground(bool b)
        {
            background.SetActive(b);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        private void RefreshAdImage()
        {
            var addPipeToolCount = Game.Instance.CurrencyModel.GetGoodNumber(GoodType.Tool, GoodSubType.AddPipe);
            var revocationToolCount = Game.Instance.CurrencyModel.GetGoodNumber(GoodType.Tool, GoodSubType.RevocationTool);
            var removeToolCount = Game.Instance.CurrencyModel.removeTool.Value;

            if (addPipeToolCount < 1)
            {
                PlusAd.SetActive(true);
            }
            else
            {
                PlusAd.SetActive(false);
            }

            if (revocationToolCount < 1)
            {
                RebackAd.SetActive(true);
            }
            else
            {
                RebackAd.SetActive(false);
            }

            if (removeToolCount < 1)
            {
                RemoveAd.SetActive(true);
            }
            else
            {
                RemoveAd.SetActive(false);
            }
        }

        public void Restart()
        {
            if (Game.Instance.LevelModel.CopiesType == CopiesType.Thread)
            {
                var enterId = Game.Instance.LevelModel.EnterLevelID;
                if (enterId == default)
                {
                    enterId = Game.Instance.LevelModel.MaxUnlockLevel.Value;
                }

                Game.Instance.RestartGame("RestartCurrentLevel", enterId,
                    forceShowAd: true);
            }
            else if (Game.Instance.LevelModel.CopiesType == CopiesType.SpecialLevel)
            {
                Game.Instance.RestartGame("RestartCurrentLevel", Game.Instance.LevelModel.EnterCopies1ID,
                    CopiesType.SpecialLevel, forceShowAd: true);
            }
        }

        private void RemoveColor()
        {
        }

        private void RevocationTool()
        {
            if (Game.Instance.CurrencyModel.CanUseTool(GoodType.Tool, GoodSubType.RevocationTool))
            {
                Context.GetController<InGameMatchController>().RevocationTool();
            }
            else
            {
                ADMudule.ShowRewardedAd("WatchAd_GetRevocationTool", (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        RewardClaimHandle.ClaimReward(
                            new RewardData(GoodType.Tool, 5, (int)GoodSubType.RevocationTool),
                            "InGame", IapStatus.Free);
                    }
                });
            }
        }

        private void AddNewPipe()
        {
            if (Game.Instance.CurrencyModel.CanUseTool(GoodType.Tool, GoodSubType.AddPipe))
            {
                Context.GetController<InGameMapController>().AddNewPipe(() =>
                {
                    Game.Instance.CurrencyModel.ConsumeGoodNumber(GoodType.Tool, (int)GoodSubType.AddPipe, 1);
                });
            }
            else
            {
                ADMudule.ShowRewardedAd("WatchAd_GetNewPipe", (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        RewardClaimHandle.ClaimReward(new RewardData(GoodType.Tool, 1, (int)GoodSubType.AddPipe),
                            "InGame", IapStatus.Free);
                    }
                });
            }
        }

        private void RemoveBall()
        {
            if (Game.Instance.CurrencyModel.CanUseTool(GoodType.Tool, GoodSubType.RemoveTool))
            {
                Context.GetController<InGameMatchController>().ClearRandomColorTool(() =>
                {
                    Game.Instance.CurrencyModel.ConsumeGoodNumber(GoodType.Tool, (int)GoodSubType.RemoveTool, 1);
                    Debug.Log("我是否被消除");
                    RefreshUI();
                });
            }
            else
            {
                ADMudule.ShowRewardedAd("WatchAd_GetremovePipe", (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        RewardClaimHandle.ClaimReward(new RewardData(GoodType.Tool, 1, (int)GoodSubType.RemoveTool),
                            "InGame", IapStatus.Free);
                        RefreshUI();
                    }
                });
            }
        }

        public void RefreshUI()
        {
            levelTxt.text = Game.Instance.LevelModel.CopiesType == CopiesType.Thread
                ? $"第 {Game.Instance.LevelModel.EnterLevelID} 关"
                : "SPECIAL LEVEL";

            var addPipeToolCount = Game.Instance.CurrencyModel.GetGoodNumber(GoodType.Tool, GoodSubType.AddPipe);
            var revocationToolCount = Game.Instance.CurrencyModel.GetGoodNumber(GoodType.Tool, GoodSubType.RevocationTool);
            var removeToolCount = Game.Instance.CurrencyModel.removeTool.Value;

            addPipeTxt.text = $"{addPipeToolCount}";
            RebackTxt.text = $"{revocationToolCount}";
            removeToolTxt.text = $"{removeToolCount}";

            addPipeToolAdIcon.SetActiveVirtual(addPipeToolCount <= 0);
            revocationToolAdIcon.SetActiveVirtual(revocationToolCount <= 0);

            addNewPipeTool.enabled = AddPipeOtherJug();
            addNewPipeTool.interactable = AddPipeOtherJug();
            revocationTool.enabled = RevocationToolOtherJug();
            revocationTool.interactable = RevocationToolOtherJug();
           // RemoveTool.enabled = Context.GetCompletionRate() < 0.99;
           // RemoveTool.interactable = Context.GetCompletionRate() < 0.99;

            RefreshAdImage();
        }

        private bool AddPipeOtherJug()
        {
            return Context.GetModel<InGameModel>().CanAddPipe();
        }

        private bool RevocationToolOtherJug()
        {
            return Context.GetController<InGameMatchController>().GetPlayerStep().Count > 0 ||
                   !Game.Instance.CurrencyModel.CanUseTool(GoodType.Tool, GoodSubType.RevocationTool);
        }

        private void HandleCanRedeemChange(bool oldValue, bool newValue)
        {
            foreach (var item in redeemBtn)
            {
                item.SetActiveVirtual(false);
            }
        }

        public void ShowSlotItemHideBigTurn(bool showSlot)
        {
        }
    }
}