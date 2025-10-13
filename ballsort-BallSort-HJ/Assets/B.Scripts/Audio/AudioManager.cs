using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioManagerModel Model { get; private set; }

    public void StopBGM()
    {
        //  _bgmSource.Stop();
    }

    public void ContinueBGM()
    {
        _bgmSource.Play();
    }

    public void CloseBGM()
    {
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }

    public void PlayBGM(AudioClip clip, float volume = 1.0f)
    {
        if (!_bgmSource.isPlaying && Model.IsBgmOpen)
        {
            _bgmSource.Play();
        }

        if (_bgmSource.clip == clip)
            return;

        if (clip != null)
        {
            _bgmSource.clip = clip;
            _bgmSource.loop = true;
            _bgmSource.volume = volume;
            _bgmSource.enabled = Model.IsBgmOpen;
            if (Model.IsBgmOpen)
                _bgmSource.Play();
        }
    }

    public void PlaySoundEffect(AudioClip clip, float duration = 2.5f, float volume = 1)
    {
        if (clip != null && Model.IsSoundOpen)
        {
            SoundPoint.PlayClip(duration, clip, volume);
        }
    }

    public void PlayLoopSound(AudioClip clip, float volume = 1)
    {
        if (clip != null && Model.IsSoundOpen)
        {
            _loopSoundSource.clip = clip;
            _loopSoundSource.loop = true;
            _loopSoundSource.volume = volume;
            _loopSoundSource.Play();
        }
    }

    public void StopLoopSound()
    {
        _loopSoundSource.Stop();
    }

    protected override void HandleAwake()
    {
        Model = new AudioManagerModel();
    }

    private void OnEnable()
    {
        _bgmSource.enabled = Model.IsBgmOpen;
        _loopSoundSource.enabled = Model.IsSoundOpen;
        Model.OnBGMSwtich += OnBGMSwtich;
        Model.OnSoundSwtich += OnSoundSwtich;
    }

    private void OnDisable()
    {
        Model.OnBGMSwtich -= OnBGMSwtich;
        Model.OnSoundSwtich -= OnSoundSwtich;
    }

    private void OnBGMSwtich(bool isBGMOpen)
    {
        _bgmSource.enabled = isBGMOpen;
    }

    private void OnSoundSwtich(bool isSoundOpen)
    {
        _loopSoundSource.enabled = isSoundOpen;
    }

    [SerializeField] private AudioSource _bgmSource = null;
    [SerializeField] private AudioSource _loopSoundSource = null;
}