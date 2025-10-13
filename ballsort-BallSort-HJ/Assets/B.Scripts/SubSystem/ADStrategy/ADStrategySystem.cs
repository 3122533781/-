using ProjectSpace.Lei31Utils.Scripts.Framework.App;

public class ADStrategySystem : GameSystem
{
    /// <summary>
    /// 累计的游戏中插屏时间
    /// </summary>
    public float AccIntersistalTime = 60f;

    //游戏中插屏
    //累计插屏时间超过2分钟，并且到达子关卡切换时候则播放插屏
    //每次播放激励则会重置这个时间，保证用户不会在很短时间内看到多次广告
    public bool NeedShowSubLevelIntersistal()
    {
        int intersistalEffectLevel = Game.Instance.GetSystem<RemoteControlSystem>().IntersistalEffectLevel.Value;
        if (Game.Instance.LevelModel.MaxUnlockLevel.Value <= intersistalEffectLevel || Game.Instance.Model.IsAdRemoved.Value)
        {
            return false;
        }

        return AccIntersistalTime >= Game.Instance.GetSystem<RemoteControlSystem>().InsertAdShowTime.Value;
    }

    public void PluseIntersistalTime(float value)
    {
        AccIntersistalTime += value;
    }

    public void ResetIntersistalTime()
    {
        AccIntersistalTime = 0;
    }

    public bool NeedShowBanner()
    {
        return Game.Instance.LevelModel.MaxUnlockLevel.Value >
               Game.Instance.GetSystem<RemoteControlSystem>().BannerOpenLevel.Value;
    }

    public bool NeedNextLevelShowIntersistal()
    {
        int slowLevel = Game.Instance.GetSystem<RemoteControlSystem>().DailySlowLevel.Value;
        int intersistalEffectLevel = Game.Instance.GetSystem<RemoteControlSystem>().IntersistalEffectLevel.Value;
        if (Game.Instance.LevelModel.MaxUnlockLevel.Value <= intersistalEffectLevel)
        {
            return false;
        }

        if ((Game.Instance.LevelModel.MaxUnlockLevel.Value > intersistalEffectLevel &&
            Game.Instance.LevelModel.MaxUnlockLevel.Value <= intersistalEffectLevel + 20) ||
            (Game.Instance.LevelModel.TodayHasPlayedLevel.Value < slowLevel))
        {
            if (_accPlayLevel >= 2)
            {
                return true;
            }
        }
        else if (Game.Instance.LevelModel.MaxUnlockLevel.Value > intersistalEffectLevel + 20)
        {
            if (_accPlayLevel >= 1)
            {
                return true;
            }
        }

        return false;
    }

    public bool NeedBackHomeShowIntersistal()
    {
        int slowLevel = Game.Instance.GetSystem<RemoteControlSystem>().DailySlowLevel.Value;
        int intersistalEffectLevel = Game.Instance.GetSystem<RemoteControlSystem>().IntersistalEffectLevel.Value;

        if (Game.Instance.LevelModel.MaxUnlockLevel.Value <= intersistalEffectLevel + 10)
        {
            return false;
        }

        if ((Game.Instance.LevelModel.MaxUnlockLevel.Value > intersistalEffectLevel + 10 &&
            Game.Instance.LevelModel.MaxUnlockLevel.Value <= intersistalEffectLevel + 20) ||
            (Game.Instance.LevelModel.TodayHasPlayedLevel.Value < slowLevel))
        {
            if (_accBackHome >= 2)
            {
                return true;
            }
        }
        else if (Game.Instance.LevelModel.MaxUnlockLevel.Value > intersistalEffectLevel + 20)
        {
            if (_accBackHome >= 1)
            {
                return true;
            }
        }

        return false;
    }

    public void PlusLevelTime()
    {
        _accPlayLevel++;
    }

    public void ResetLevelTime()
    {
        _accPlayLevel = 0;
    }

    public void PlusBackHomeTime()
    {
        _accBackHome++;
    }

    public void ResetBackHomeTime()
    {
        _accBackHome = 0;
    }

    public override void Init()
    {
        _accPlayLevel = 0;
        _accBackHome = 0;
    }

    public override void Destroy()
    {
    }

    /// <summary>
    /// 累计的游玩关卡数
    /// </summary>
    private int _accPlayLevel;

    /// <summary>
    /// 累计的返回主页次数
    /// </summary>
    private int _accBackHome;
}