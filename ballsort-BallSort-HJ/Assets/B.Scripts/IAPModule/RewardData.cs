using UnityEngine;

namespace ProjectSpace.Lei31Utils.Scripts.IAPModule
{
    public enum RewardQuality
    {
        Normal,
    }


    [System.Serializable]
    public class RewardData
    {
        public int count;
        public GoodType goodType;
        public RewardQuality goodQuality;
        public int subType;
        public int skinId;

        public RewardData()
        {
        }

        public RewardData(GoodType goodType, int count)
        {
            this.count = count;
            this.goodType = goodType;
        }


        public RewardData(GoodType goodType, int count, int subType)
        {
            this.count = count;
            this.goodType = goodType;
            this.subType = subType;
        }
    }


    [System.Serializable]
    public class MoneyReward : RewardData
    {
        public float moneyCount;

        public float GetCount()
        {
            if (goodType == GoodType.Money)
            {
                return moneyCount;
            }

            return count;
        }

        public void DoubleCount()
        {
            if (goodType == GoodType.Money)
            {
                moneyCount *= 2;
            }

            count *= 2;
        }

        public int GetFlyAnimeCount()
        {
            if (goodType == GoodType.Money)
            {
                return (int) (Mathf.Max(moneyCount * 100, 1));
            }

            return count;
        }
    }
}