using System.Collections.Generic;

public class GoodsConfig : ScriptableConfigGroup<GoodsData, GoodsConfig>
{
    /// <summary>
    /// 获取所有指定类型的商品
    /// </summary>
    /// <param name="goodType"></param>
    /// <returns></returns>
    public List<GoodsData> FindAll(GoodType goodType)
    {
        return All.FindAll(ret => { return ret.Type == goodType; });
    }


    /// <summary>
    /// 获取所有指定类型的商品
    /// </summary>
    /// <param name="goodType"></param>
    /// <param name="subType"></param>
    /// <returns></returns>
    public GoodsData FindData(GoodType goodType, int subType)
    {
        return All.Find(ret => ret.Type == goodType && ret.subType == subType);
    }

   
}