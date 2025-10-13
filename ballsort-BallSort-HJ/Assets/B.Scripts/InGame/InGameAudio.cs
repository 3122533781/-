using UnityEngine;

public class InGameAudio : MonoSingleton<InGameAudio>
{
    public void PlayTapTile()
    {
        AudioManager.Instance.PlaySoundEffect(_clipTapTile);
    }

    public void PlayMergeTile()
    {
        AudioManager.Instance.PlaySoundEffect(_clipMergeTile);
    }

    public void PlayWinGame()
    {
        AudioManager.Instance.PlaySoundEffect(_clipWinGame);
    }

    public void PlayLoseGame()
    {
        AudioManager.Instance.PlaySoundEffect(_clipLoseGame);
    }

    public void PlayRunningHouse()
    {
        AudioManager.Instance.PlaySoundEffect(_clipRunningHouse);
    }

    public void PlayLoseSureSecond()
    {
        AudioManager.Instance.PlaySoundEffect(_clipLoseSureSecond);
    }

    public void PlayLoseSureExit()
    {
        AudioManager.Instance.PlaySoundEffect(_clipLoseSureExit);
    }

    public void PlayGameWinLastStep()
    {
        AudioManager.Instance.PlaySoundEffect(_clipGameWinLastStep, 5f);
    }

    public void PlayRewardChest()
    {
        //        AudioManager.Instance.PlaySoundEffect(_clipRewardChest);
    }

    public void PlayRewardGet()
    {
        AudioManager.Instance.PlaySoundEffect(_clipRewardGet);
    }

    public void PlayWinningStreak()
    {
        AudioManager.Instance.PlaySoundEffect(_clipWinningStreak);
    }

    public void PlayGetCoin()
    {
        AudioManager.Instance.PlaySoundEffect(_clipGetCoin);
    }

    public void PlaySavingPotOut()
    {
        AudioManager.Instance.PlaySoundEffect(_clipSavingPotOut);
    }

    [SerializeField] private AudioClip _clipTapTile;
    [SerializeField] private AudioClip _clipMergeTile;
    [SerializeField] private AudioClip _clipWinGame;
    [SerializeField] private AudioClip _clipLoseGame;
    [SerializeField] private AudioClip _clipRunningHouse;
    [SerializeField] private AudioClip _clipLoseSureSecond;
    [SerializeField] private AudioClip _clipLoseSureExit;
    [SerializeField] private AudioClip _clipGameWinLastStep;
    [SerializeField] private AudioClip _clipRewardGet;
    [SerializeField] private AudioClip _clipWinningStreak;
    [SerializeField] private AudioClip _clipGetCoin;
    [SerializeField] private AudioClip _clipSavingPotOut;
}