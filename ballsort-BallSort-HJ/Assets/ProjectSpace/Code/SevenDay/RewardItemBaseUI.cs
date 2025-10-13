using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemBaseUI : MonoBehaviour
{
    [SerializeField] protected Image _rewardImg = null;
    [SerializeField] protected Text _amount = null;

    protected Reward _rewardData = null;
    public virtual void SetData(Reward reward)
    {
        _rewardData = reward;
        _amount.text = $"+{reward.Count}";
        SetReward(reward.type, false);
    }

    protected virtual void SetReward(GoodSubType2 rewardType, bool setNativeSize = true)
    {
        Sprite sprite = Resources.Load<Sprite>($"Chest/{rewardType}");
        Debug.Log("图片为" + rewardType);
        _rewardImg.sprite = sprite;
        _rewardImg.SetNativeSize();

        if (setNativeSize)
        {
            _rewardImg.SetNativeSize();
        }
    }
}
