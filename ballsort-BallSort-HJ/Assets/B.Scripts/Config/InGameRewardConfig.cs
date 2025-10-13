using System;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02.Scripts.Config
{
    public class InGameRewardConfig : ScriptableConfigGroup<InGameRewardData, InGameRewardConfig>
    {
        public RewardData GetRandomRewardData()
        {
            int totalWeight = 0;
            foreach (var cardData in All)
            {
                totalWeight += cardData.Probability;
            }

            int ranNum = Random.Range(0, totalWeight + 1); //当前抽的权重

            int counter = 0;

            foreach (var temp in All)
            {
                counter += temp.Probability;
                if (ranNum <= counter)
                {
                 if (temp.Category == GoodType.Tool)
                    {
                        if (temp.ItemId == 0)
                        {
                            return new RewardData(GoodType.Coin, temp.Number);
                        }
                        else
                        {
                            return new RewardData(temp.Category, temp.Number, temp.ItemId);
                        }
                    }

                    return new RewardData(temp.Category, temp.Number, temp.ItemId);
                }
            }

            return null;
        }
    }

    [Serializable]
    public class InGameRewardData : IConfig
    {
        public int intID;
        public int ItemId;
        public GoodType Category;
        public int Number;
        public int Probability;
        public int ID => intID;
    }
}