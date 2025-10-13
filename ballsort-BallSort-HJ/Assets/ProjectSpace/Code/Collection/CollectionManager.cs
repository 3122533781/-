using Fangtang.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionManager : Singleton<CollectionManager>
{
    // ˽�й��죬ȷ������
    private CollectionManager() { }

    public void Initialize()
    {
        var cards = CollectionConfig.Instance.All;
        foreach (var card in cards)
        {
            card.InitIsHave();
        }
    }

    public string Unlock(GoodSubType2 collection)
    {
        var cards = CollectionConfig.Instance.All;
        // ��ȫ�жϣ�������ñ�Ϊ�գ�ֱ�ӷ��ر��ⱨ��
        if (cards == null || cards.Count() == 0)
        {
            Debug.LogError("CollectionConfig.Instance.All Ϊ�գ��޷�������Ʒ");
            return cards[0].name;
        }

        int randomIndex = -1; // ���������Ĭ��-1����Чֵ��

        // ����ö��ȷ���������䣬�����������������
        switch (collection)
        {
            case GoodSubType2.collection1:
                // ���䣺0~7������ҿ�������max=8��
                randomIndex = GetRandomIndexInRange(0, 8, cards.Count());
                break;

            case GoodSubType2.collection2:
                // ���䣺8~15
                randomIndex = GetRandomIndexInRange(8, 16, cards.Count());
                break;

            case GoodSubType2.collection3:
                // ���䣺16~23
                randomIndex = GetRandomIndexInRange(16, 24, cards.Count());
                break;

            case GoodSubType2.collection4:
                // ����ѡ�������Ҫ��չ��һ�����䣨24~31�������������
                randomIndex = GetRandomIndexInRange(24, 32, cards.Count());
                break;

            default:
                Debug.LogWarning($"δ֪�Ĳ�Ʒö�٣�{collection}���޷�����");
                return cards[0].name;
        }

        // ������������Ч����0����ִ�н���
        if (randomIndex >= 0)
        {
            cards[randomIndex].IsHave.Value = true;
        }
        return cards[randomIndex].name;
    }

    /// <summary>
    /// ������������ָ�����������������������ȷ��������cards�б���
    /// </summary>
    /// <param name="min">������ʼ��������</param>
    /// <param name="max">�������������������ΪRandom.Range(int)������ҿ���</param>
    /// <param name="cardsCount">��Ʒ���ñ��ܳ���</param>
    /// <returns>��Ч���������-1��ʾ��Ч��</returns>
    private int GetRandomIndexInRange(int min, int max, int cardsCount)
    {
        // 1. ����max�����max�������ñ��ȣ���cardsCount��Ϊmax������Խ�磩
        int actualMax = Mathf.Min(max, cardsCount);
        // 2. ��������Ƿ���Ч�����min �� actualMax��˵��������û�п���������
        if (min >= actualMax)
        {
            Debug.LogError($"����������Ч��min={min}��actualMax={actualMax}����Ʒ���ñ��{cardsCount}��Ԫ��");
            return -1;
        }
        // 3. �����������������
        return Random.Range(min, actualMax);
    }
}