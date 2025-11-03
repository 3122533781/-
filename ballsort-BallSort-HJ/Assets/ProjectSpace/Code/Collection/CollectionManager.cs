using Fangtang.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionManager : Singleton<CollectionManager>
{
    // 私有构造，确保单例
    private CollectionManager() { }

    public void Initialize()
    {
        var cards = CollectionConfig.Instance.All;
        foreach (var card in cards)
        {
            card.InitIsHave();
        }
    }






    public int GetProbabilityRandomIndex()
    {
        var cards = CollectionConfig.Instance.All;
      
        float randomPercent = UnityEngine.Random.Range(0f, 100f);
        int startIndex = 0;
        int endIndex = 0;

        // 2. 根据随机数判断所属概率区间，确定索引范围
        if (randomPercent < 50)
        {
            // 50% 概率：索引范围 [0, 20)
            startIndex = 0;
            endIndex = 20;
        }
        else if (randomPercent < 80) // 50+30=80
        {
            // 30% 概率：索引范围 [20, 34)
            startIndex = 20;
            endIndex = 30;
        }
        else if (randomPercent < 95) // 80+15=95
        {
            // 15% 概率：索引范围 [34, 45)
            startIndex = 30;
            endIndex = 36;
        }
        else if (randomPercent < 99) // 95+4=99
        {
            // 4% 概率：索引范围 [45, 49)
            startIndex = 36;
            endIndex = 39;
        }
        else // 99~100
        {
            // 1% 概率：索引范围 [49, 50)
            startIndex = 39;
            endIndex = 40;
        }

        // 3. 调用已有方法，在当前区间内获取随机索引（确保不越界）
        return GetRandomIndexInRange(startIndex, endIndex, cards.Count());
    }



    public  string Unlock2()
    {
        var cards = CollectionConfig.Instance.All;
        int targetIndex = GetProbabilityRandomIndex();
        Debug.Log("随机概率索引为" + targetIndex);
        if (targetIndex >= 0)
        {
            cards[targetIndex].IsHave.Value = true;
        }
        return cards[targetIndex].name;
    }







    public string Unlock(GoodSubType2 collection)
    {
        var cards = CollectionConfig.Instance.All;
        // 安全判断：如果配置表为空，直接返回避免报错
        if (cards == null || cards.Count() == 0)
        {
            Debug.LogError("CollectionConfig.Instance.All 为空！无法解锁藏品");
            return cards[0].name;
        }

        int randomIndex = -1; // 随机索引，默认-1（无效值）

        // 根据枚举确定解锁区间，生成区间内随机索引
        switch (collection)
        {
            case GoodSubType2.collection1:
                // 区间：0~7（左闭右开，所以max=8）
                randomIndex = GetRandomIndexInRange(0, 8, cards.Count());
                break;

            case GoodSubType2.collection2:
                // 区间：8~15
                randomIndex = GetRandomIndexInRange(8, 16, cards.Count());
                break;

            case GoodSubType2.collection3:
                // 区间：16~23
                randomIndex = GetRandomIndexInRange(16, 24, cards.Count());
                break;

            case GoodSubType2.collection4:
                // （可选）如果需要扩展下一个区间（24~31），按规律添加
                randomIndex = GetRandomIndexInRange(24, 32, cards.Count());
                break;

            default:
                Debug.LogWarning($"未知的藏品枚举：{collection}，无法解锁");
                return cards[0].name;
        }

        // 如果随机索引有效（≥0），执行解锁
        if (randomIndex >= 0)
        {
            cards[randomIndex].IsHave.Value = true;
        }
        return cards[randomIndex].name;
    }

    /// <summary>
    /// 辅助方法：在指定区间内生成随机索引，并确保不超出cards列表长度
    /// </summary>
    /// <param name="min">区间起始（包含）</param>
    /// <param name="max">区间结束（不包含，因为Random.Range(int)是左闭右开）</param>
    /// <param name="cardsCount">藏品配置表总长度</param>
    /// <returns>有效随机索引（-1表示无效）</returns>
    private int GetRandomIndexInRange(int min, int max, int cardsCount)
    {
        // 1. 修正max：如果max超出配置表长度，用cardsCount作为max（避免越界）
        int actualMax = Mathf.Min(max, cardsCount);
        // 2. 检查区间是否有效（如果min ≥ actualMax，说明区间内没有可用索引）
        if (min >= actualMax)
        {
            Debug.LogError($"解锁区间无效！min={min}，actualMax={actualMax}，藏品配置表仅{cardsCount}个元素");
            return -1;
        }
        // 3. 生成区间内随机索引
        return Random.Range(min, actualMax);
    }
}