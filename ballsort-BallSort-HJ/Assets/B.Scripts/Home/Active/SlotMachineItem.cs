using System;
using _02.Scripts.InGame;
using DG.Tweening;
using Lei31.Localizetion;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.Home.Active
{
    public class SlotMachineItem : DialogButtonItem
    {
        [SerializeField] private Slider progress;
        [SerializeField] private IntNumberDisplayer text;
        [SerializeField] private Text maxStar;

        protected override void CheckIsCanShow(out bool isCanShow)
        {
            isCanShow = false;
        }

        private void Awake()
        {
            Game.Instance.CurrencyModel.RegisterStarFunc(RefreshStarText);
        }

        private void OnDestroy()
        {
            Game.Instance.CurrencyModel.UnregisterStarFunc(RefreshStarText);
        }

        protected override void ShowDialog()
        {
           
        }

        protected override void RefreshItem()
        {
            RefreshStarText(-1, default);
        }

        private void RefreshStarText(int a, int b)
        {
            var starCount = Game.Instance.CurrencyModel.starNumber.Value;
            progress.DOValue(starCount, 0.5f);
            progress.maxValue = InGameManager.Instance.maxStar;
            maxStar.text = $"{InGameManager.Instance.maxStar}";
            if (a == -1)
            {
                text.ResetNumber(starCount);
            }
            else
            {
                text.Number = starCount;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)progress.transform);
        }
    }
}