using System;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;

namespace _02.Scripts.Home.Active
{
    public class PaintBtn : MonoBehaviour
    {
        [SerializeField] private IntNumberDisplayer coinANime;

        private void OnEnable()
        {
            Game.Instance.CurrencyModel.RegisterCoinChangeAction(RefreshCoinText);
            // RefreshCoinText(-1, default);
        }

        private void OnDisable()
        {
            Game.Instance.CurrencyModel.UnregisterCoinChangeAction(RefreshCoinText);
        }


        private void RefreshCoinText(int a, int b)
        {
            var num = Game.Instance.CurrencyModel.CoinNum;

            coinANime.Number = num;
        }
    }
}