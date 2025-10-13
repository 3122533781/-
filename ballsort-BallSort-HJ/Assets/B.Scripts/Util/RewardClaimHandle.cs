using System.Collections.Generic;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;

public enum IapStatus
{
    CanBuy = 1,
    SoldAll = 2,
    CanWatch = 3,
    CanShare = 4,
    CanUseCoin = 5,
    Free = 6,
    CanUseStar = 7,
    Lock = 8,
    InCoolTime = 9,
}

public class RewardClaimHandle
{
    public static void ConsumeCoin(double consumeCoinAmount, int buyItemAmount, string itemName)
    {
    }

    public static void ClaimReward(List<MoneyReward> rewardList, string pos, IapStatus status)
    {
        foreach (var t in rewardList)
        {
            ClaimReward(t, pos, status);
        }
    }

    public static void ClaimReward(List<RewardData> rewards, string pos, IapStatus status)
    {
        foreach (var t in rewards)
        {
            ClaimReward(t, pos, status);
        }
    }

    public static void ClaimReward(RewardData reward, string pos, IapStatus status)
    {
        switch (reward.goodType)
        {


            case GoodType.RemoveAD:
                //Game.Instance.Model.SetAdRemoved();
                break;

            case GoodType.Coin:
                Game.Instance.CurrencyModel.AddGoodCount(reward.goodType, reward.subType, reward.count);
                break;

            case GoodType.Tool:
                Game.Instance.CurrencyModel.AddGoodCount(reward.goodType, reward.subType, reward.count);
                break;

            case GoodType.Star:
                Game.Instance.CurrencyModel.AddGoodCount(reward.goodType, reward.subType, reward.count);
                break;
        }
    }

    public static void ClaimReward(MoneyReward reward, string pos, IapStatus status)
    {
        switch (reward.goodType)
        {


            case GoodType.RemoveAD:
                //Game.Instance.Model.SetAdRemoved();
                break;

            case GoodType.Coin:
                Game.Instance.CurrencyModel.AddGoodCount(reward.goodType, reward.subType, reward.count);
                break;

            case GoodType.Tool:
                Game.Instance.CurrencyModel.AddGoodCount(reward.goodType, reward.subType, reward.count);
                break;

            case GoodType.Star:
                Game.Instance.CurrencyModel.AddGoodCount(reward.goodType, reward.subType, reward.count);
                break;

            case GoodType.Money:
                Game.Instance.CurrencyModel.RewardMoney(reward.GetCount());
                break;

            case GoodType.Gem:
                Game.Instance.CurrencyModel.AddGoodCount(reward.goodType, reward.subType, reward.count);
                break;
        }
    }
}