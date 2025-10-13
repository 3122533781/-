using System.Collections.Generic;
using _02.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GoodsRoller : MonoBehaviour
{
    [SerializeField]
    private Image goodsImageTpl;
    [SerializeField]
    private int goodsCount = 32;
    [SerializeField]
    private float moveTime = 0.5f;

    // Start is called before the first frame update
    private void Awake()
    {
        goodsImageTpl.gameObject.SetActive(false);
    }

    /// <summary>
    /// 创建商品
    /// </summary>
    /// <param name="data"></param>
    private void CreateGoods(GoodsData data)
    {
        var goods = GameObject.Instantiate(goodsImageTpl, goodsImageTpl.transform.parent);
        goods.gameObject.SetActive(true);
        goods.sprite = data.GetGoodTypeIcon();
    }

    /// <summary>
    /// 重置商品
    /// </summary>
    /// <param name="data"></param>
    /// <param name="allGoods"></param>
    public void ResetGoods(GoodsData data, List<GoodsData> allGoods)
    {
        UtilClass.DestroyActiveChildren(goodsImageTpl.transform.parent);
        for (int i = 0; i < goodsCount; i++)
        {
            if (i == 1)
            {
                CreateGoods(data);
            }
            else
            {
                CreateGoods(allGoods.ListRandom());
            }
        }
    }

    /// <summary>
    /// 开始摇
    /// </summary>
    /// <param name="end"></param>
    public void StartRolling(float end)
    {
        var rt = transform as RectTransform;
        rt.DOAnchorPosY(end, moveTime);
    }

}
