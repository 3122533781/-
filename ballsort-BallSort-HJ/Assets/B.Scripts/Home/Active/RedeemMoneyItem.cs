using System;
using _02.Scripts.InGame;
using DG.Tweening;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.Home.Active
{
    public class RedeemMoneyItem : DialogButtonItem
    {
        [SerializeField] private FloatNumberDisplayer text;
        [SerializeField] private IntNumberDisplayer gemTxt;

        protected override void CheckIsCanShow(out bool isCanShow)
        {
            isCanShow = Game.Instance.Model.IsWangZhuan();
        }

        private void Awake()
        {
            Game.Instance.CurrencyModel.RegisterMoneyChangeAction(RefreshStarText);
            Game.Instance.CurrencyModel.RegisterToolChangeAction(GoodType.Gem, GoodSubType.Null, RefreshGemText);
        }


        private void OnDestroy()
        {
            Game.Instance.CurrencyModel.UnregisterMoneyChangeAction(RefreshStarText);
            Game.Instance.CurrencyModel.UnregisterToolChangeAction(GoodType.Gem, GoodSubType.Null, RefreshGemText);
        }


        protected override void RefreshItem()
        {
            RefreshStarText(-1, default);
        }


        private void RefreshStarText(float a, float b)
        {
            var num = Game.Instance.CurrencyModel.GetCurrentMoney();
            if (a <= 0)
            {
                text.ResetNumber(num);
            }
            else
            {
                text.Number = num;
            }
        }


        private void RefreshGemText(int arg1, int arg2)
        {
            gemTxt.Number = Game.Instance.CurrencyModel.GetGoodNumber(GoodType.Gem, GoodSubType.Null);
        }
    }
}