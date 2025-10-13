using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
public class AudioClipHelper : MonoSingleton<AudioClipHelper>
{
    public void PlayShowDialogClip()
    {
        AudioManager.Instance.PlaySoundEffect(_clipList[(int)AudioClipEnum.ShowDialog]);
    }

    public void PlayButtonTap()
    {
        AudioManager.Instance.PlaySoundEffect(_clipList[(int)AudioClipEnum.ButtonTap]);
    }

    public void PlayClose()
    {
        AudioManager.Instance.PlaySoundEffect(_clipList[(int)AudioClipEnum.Close]);
    }

    public void PlayOpenChest()
    {
        AudioManager.Instance.PlaySoundEffect(_clipList[(int)AudioClipEnum.ClickAward]);
    }

    public void PlaySkinButtonTap()
    {
        AudioManager.Instance.PlaySoundEffect(_clipList[(int)AudioClipEnum.Tab]);
    }

    public void PlaySound(AudioClipEnum type, float duration = 2.5f, float volume = 1)
    {
        if (type == AudioClipEnum.Seagull) AudioManager.Instance.PlayLoopSound(_clipList[(int)type], volume);
        AudioManager.Instance.PlaySoundEffect(_clipList[(int)type], duration, volume);
    }

    public void StopLoop(AudioClipEnum type)
    {
        if (type == AudioClipEnum.Seagull) AudioManager.Instance.StopLoopSound();
    }

    [SerializeField] private List<AudioClip> _clipList = null;
}

public enum AudioClipEnum
{
    Close,//返回音效
    ButtonTap,//按钮音效
    Tips,//提示条音效
    GetCoin,//金币音效
    BuyGoods,//购买物品
    ClickAward,//点击领取
    ShowDialog,//打开弹窗
    Win,
    ClickPipe,
    FullPipe,
    
    Tab,//点击tab切换时播放音效

    Seagull,//海浪海鸥
    RevocationTool,//撤回
}