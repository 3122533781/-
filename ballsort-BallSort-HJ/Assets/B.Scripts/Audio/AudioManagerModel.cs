using System;

public class AudioManagerModel
{
    public Action<bool> OnBGMSwtich = delegate { };
    public Action<bool> OnSoundSwtich = delegate { };

    public bool IsBgmOpen
    {
        get => _isBGMOpen;
        set
        {
            if (_isBGMOpen != value)
            {
                _isBGMOpen = value;
                if (!value)
                {
                    StaticModule.SwitchMusicOff();
                }

                PlayerDataStorage.SetBGMOpen(value);
                OnBGMSwtich.Invoke(value);
            }
        }
    }

    public bool IsSoundOpen
    {
        get => _isSoundOpen;
        set
        {
            if (_isSoundOpen != value)
            {
                _isSoundOpen = value;
                if (!value)
                {
                    StaticModule.SwitchSoundOff();
                }
                PlayerDataStorage.SetSoundOpen(value);
                OnSoundSwtich.Invoke(value);
            }
        }
    }

    public AudioManagerModel()
    {
        _isSoundOpen = PlayerDataStorage.GetSoundOpen();
        _isBGMOpen = PlayerDataStorage.GetBGMOpen();
    }

    private bool _isSoundOpen = true;
    private bool _isBGMOpen = true;
}