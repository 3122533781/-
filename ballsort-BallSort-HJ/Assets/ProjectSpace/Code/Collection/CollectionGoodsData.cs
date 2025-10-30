using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CollectionGoodsData : IConfig
{
    public int id;//��ƷID
    public string name;//��Ʒ����
    public Kings kind;//����
    public Quality quality;
    public PersistenceData<bool> IsHave { get; private set; }
    public int ID
    {
        get { return id; }
    }




    public void InitIsHave()
    {
        IsHave = new PersistenceData<bool>(name, true);
    }
   





}
public enum Kings
{
    Kitchenware,
    Condiments,
    Ingredients,
    Dishes,
    Beverages
}
public enum Quality
{
    _N,
    _R,
    _SR,
    _SSR,
    _UR

}
