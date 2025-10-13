using _02.Scripts.Common;
using _02.Scripts.Config;
using NPOI.SS.Formula.Functions;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System;
using UnityEngine;

namespace ProjectSpace.Lei31Utils.Scripts.Framework.App
{
    public class GameCurrencyModel
    {
        public int CoinNum => _coin.Value;
        public int DiamondNum => _diamond.Value;
        public PersistenceData<int> ActionPoints { get; private set; } //行动点
        public PersistenceData<int> ManualValue { get; private set; } //体力值

        private PersistenceData<float> Money { get; set; }
        public PersistenceData<int> Diamond { get; private set; }
        public PersistenceData<int> GetMoneyCount { get; private set; }

        // 获得金币记录
        public int coinsRecord = 0;

        public float GetCurrentMoney()
        {
            return Money.Value;
        }

        public void SetCurrentMoney(float newValue)
        {
            Money.Value = newValue;
        }

        public void ResetGameToolByServer()
        {
    
        }

        /// <summary>
        /// 获取当前进度，存于服务器
        /// </summary>
        public void SetGameToolServerProgress()
        {
            // var serialObj = new GameToolProgress(_coin.Value, starNumber.Value, ManualValue.Value, addPipe.Value,
            //     revocationTool.Value, Money.Value, Diamond.Value);
            // ServerLogic.Instance.SetServerProgress(SoyProfileConst.GameToolProgress, serialObj, true);
        }

        public void RegisterCoinChangeAction(Action<int, int> onCoinChange)
        {
            _coin.OnValueChange += onCoinChange;
        }

        public void UnregisterCoinChangeAction(Action<int, int> onCoinChange)
        {
            _coin.OnValueChange -= onCoinChange;
        }

        public void RegisterDiamondChangeAction(Action<int, int> onCoinChange)
        {
            _diamond.OnValueChange += onCoinChange;
        }

        public void UnregisterDiamondChangeAction(Action<int, int> onCoinChange)
        {
            _diamond.OnValueChange -= onCoinChange;
        }

        public void RegisterMoneyChangeAction(Action<float, float> onCoinChange)
        {
            Money.OnValueChange += onCoinChange;
        }

        public void UnregisterMoneyChangeAction(Action<float, float> onCoinChange)
        {
            Money.OnValueChange -= onCoinChange;
        }

        public void OfflineRecoverManual(int value)
        {
            if (ManualValue.Value + value >= GlobalDef.MANUAL_MAX)
            {
                ManualValue.Value = GlobalDef.MANUAL_MAX;
            }
            else
            {
                ManualValue.Value += value;
            }

            SetGameToolServerProgress();
        }

        /// <summary>
        /// 系统赠与虚拟金币
        /// </summary>
        /// <param name="value">增加值</param>
        /// <param name="reason">赠与原因，途径</param>
        public void RewardCoin(int value, string reason = null, StaticModule.ItemType type = StaticModule.ItemType.Game)
        {
            _coin.Value += value;
            if (!"Debug".Equals(reason)) SoyProfile.Set(SoyProfileConst.HaveCoin, _coin.Value);

            SetGameToolServerProgress();
            StaticModule.GetMaterial(StaticModule.Currency.Coin, value, "Coin", type);
        }

        public void RewadDiamond(int value, string reason = null, StaticModule.ItemType type = StaticModule.ItemType.Game)
        {
            _diamond.Value += value;
            if (!"Debug".Equals(reason)) SoyProfile.Set(SoyProfileConst.HaveCoin, _coin.Value);

            SetGameToolServerProgress();
            StaticModule.GetMaterial(StaticModule.Currency.Diamond, value, "Diamond", type);
        }

        public void ConsumeCoin(int value, string reason = null,
            StaticModule.ItemType type = StaticModule.ItemType.Game)
        {
            _coin.Value -= Mathf.Min(_coin.Value, value);
            if (!"Debug".Equals(reason)) SoyProfile.Set(SoyProfileConst.HaveCoin, _coin.Value);

            SetGameToolServerProgress();
            StaticModule.ConsumeMaterial(StaticModule.Currency.Coin, value, "Coin", type);
        }

        public void ConsumeDiamond(int value, string reason = null,
          StaticModule.ItemType type = StaticModule.ItemType.Game)
        {
            _diamond.Value -= Mathf.Min(_diamond.Value, value);
            if (!"Debug".Equals(reason)) SoyProfile.Set(SoyProfileConst.HaveCoin, _diamond.Value);

            SetGameToolServerProgress();
            StaticModule.ConsumeMaterial(StaticModule.Currency.Diamond, value, "Diamond", type);
        }

        public void RewardMoney(float newValue)
        {
            Money.Value += newValue;
            GetMoneyCount.Value++;
            // App.Instance.CurrencyModel.SetGameToolServerProgress();
        }

        public void RegisterToolChangeAction(GoodType type, GoodSubType subType, Action<int, int> onCoinChange)
        {
            switch (type)
            {
                case GoodType.Tool:
                    if (subType == GoodSubType.RevocationTool)
                    {
                        revocationTool.OnValueChange += onCoinChange;
                    }
                    else if (subType == GoodSubType.AddPipe)
                    {
                        addPipe.OnValueChange += onCoinChange;
                    }

                    break;

                case GoodType.Star:
                    starNumber.OnValueChange += onCoinChange;
                    break;

                case GoodType.Gem:
                    Diamond.OnValueChange += onCoinChange;
                    break;
            }
        }

        public void UnregisterToolChangeAction(GoodType type, GoodSubType subType, Action<int, int> onCoinChange)
        {
            switch (type)
            {
                case GoodType.Tool:
                    if (subType == GoodSubType.RevocationTool)
                    {
                        revocationTool.OnValueChange -= onCoinChange;
                    }
                    else if (subType == GoodSubType.AddPipe)
                    {
                        addPipe.OnValueChange -= onCoinChange;
                    }

                    break;

                case GoodType.Gem:
                    Diamond.OnValueChange -= onCoinChange;
                    break;
            }
        }

        public int GetGoodNumber(GoodType type, GoodSubType subType = GoodSubType.Null)
        {
            var res = 0;
            switch (type)
            {
                case GoodType.Coin:
                    res = _coin.Value;
                    break;

                case GoodType.Tool:
                    if (subType == GoodSubType.RevocationTool)
                    {
                        res = revocationTool.Value;
                    }
                    else if (subType == GoodSubType.AddPipe)
                    {
                        res = addPipe.Value;
                    }
                    else if (subType == GoodSubType.RemoveTool)
                    {
                        res = removeTool.Value;
                    }
                    break;

                case GoodType.Star:
                    res = starNumber.Value;
                    break;

                case GoodType.Gem:
                    res = Diamond.Value;
                    break;
            }

            return res;
        }
        public void ConsumeGoodNumber(GoodType type, int subType, int count)
        {
            AddGoodCount(type, subType, count * -1);

            StaticModule.ConsumeMaterial((StaticModule.Currency)type, count, $"{type}");
        }

        public void AddGoodCount(GoodType type, int subType, int count)
        {
            switch (type)
            {
                case GoodType.Coin:
                    _coin.Value += count;
                    break;

                case GoodType.Star:
                    starNumber.Value += count;
                    break;

                case GoodType.Gem:
                    Diamond.Value += count;
                    break;

                case GoodType.Tool:
                    if (subType == (int)GoodSubType.RevocationTool)
                    {
                        revocationTool.Value += count;
                    }
                    else if (subType == (int)GoodSubType.AddPipe)
                    {
                        addPipe.Value += count;
                    }
                    else if (subType == (int)GoodSubType.RemoveTool)
                    {
                        removeTool.Value += count;
                    }
                    break;
            }

            SetGameToolServerProgress();
            StaticModule.GetMaterial((StaticModule.Currency)type, count, $"{type}");
        }
        public void AddNewGoodCount(GoodSubType2 type)
        {

            switch (type)
            {
                case GoodSubType2.AddPipe:
                    addPipe.Value += 1;
                    break;
                case GoodSubType2.RevocationTool:
                    revocationTool.Value += 1;
                    break;
                case GoodSubType2.Remove:
                    removeTool.Value += 1;
                    break;

            }

        }

        public string OpenCollectionPackage(GoodSubType2 type)
        {
            return CollectionManager.Instance.Unlock(type);

        }




        public bool CanUseTool(GoodType goodType, GoodSubType goodSubType)
        {
            var res = false;
            var count = GetGoodNumber(goodType, goodSubType);
            switch (goodType)
            {
                case GoodType.Tool:
                    if (goodSubType == GoodSubType.RevocationTool)
                    {
                        res = count > 0;
                    }
                    else if (goodSubType == GoodSubType.AddPipe)
                    {
                        res = count > 0;
                    }
                    else if (goodSubType == GoodSubType.RemoveTool)
                    {
                        res = count > 0;
                    }
                    break;
            }

            return res;
        }

        public GameCurrencyModel()
        {
            _coin = new PersistenceData<int>("AppCurrencyModel_Coin",
                GlobalDef.INGAME_GET_COIN); //GlobalDef.INGAME_GET_COIN
            _diamond = new PersistenceData<int>("AppCurrencyModel_Diamond",
                GlobalDef.INGAME_GET_COIN);
            ActionPoints = new PersistenceData<int>("AppCurrencyModel_ActionPoints", 0);
            ManualValue = new PersistenceData<int>("AppCurrencyModel_ManualValue", 5);
            Money = new PersistenceData<float>("AppCurrencyModel_Money", 0);
            Diamond = new PersistenceData<int>("AppCurrencyModel_Diamond", 0);

            addPipe = new PersistenceData<int>("AppCurrencyModel_AddPipe", 2);
            revocationTool = new PersistenceData<int>("AppCurrencyModel_RevocationTool", 5);
            removeTool = new PersistenceData<int>("AppCurrencyModel_RemoveTool", 1);
            starNumber = new PersistenceData<int>("AppCurrencyModel_StarNumber", 0);
            GetMoneyCount = new PersistenceData<int>("AppCurrencyModel_MoneyNumber", 0);
        }

        #region star


        public PersistenceData<int> starNumber;

        public void RegisterStarFunc(Action<int, int> onChange)
        {
            starNumber.OnValueChange += onChange;
        }

        public void UnregisterStarFunc(Action<int, int> onChange)
        {
            starNumber.OnValueChange -= onChange;
        }



        #endregion star

        private PersistenceData<int> _coin;
        public PersistenceData<int> _diamond;
        private PersistenceData<int> addPipe; //添加管子数量
        private PersistenceData<int> revocationTool; //撤回道具
        public PersistenceData<int> removeTool; //消除道具
    }
}