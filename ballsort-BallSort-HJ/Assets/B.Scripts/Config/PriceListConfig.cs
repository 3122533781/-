using System;

public class PriceListConfig : ScriptableConfigGroup<PriceListData, PriceListConfig>
{

}

[Serializable]
public class PriceListData : IConfig
{
    public int ID => rewardId;
    public int rewardId;

    public BuyType currency;
    public float price;
    public float originalPrice;
    public int limit;
    public string storeId;
}

[Serializable]
public enum BuyType
{
    Cash = 1,
    Diamond = 2,
    Coin = 3,
    WatchAd = 4,
    Free = 5,
}
