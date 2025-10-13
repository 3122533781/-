using System.Text;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;

public class RemoteControlSystem : GameSystem
{
    /// <summary>
    /// 关卡测试 AB ，配置文件选择，如果为 true 则选择 B
    /// </summary>
    public PersistenceData<bool> LevelUseConfigB { get; set; }

    //0,1,2
    public PersistenceData<int> RedeemMenoyCount { get; private set; }

    public const bool LevelUseConfigB_Default = false;

    /// <summary>
    /// 开始游戏第一个背景 0 咖啡 1 蓝色
    /// </summary>
    public PersistenceData<int> FirstBgType { get; set; }

    public const int FirstBgType_Default = 0;

    /// <summary>
    /// 单局失败之后广告复活的次数
    /// ab 结束固定为3 1.0.2 版本失效
    /// </summary>
    public PersistenceData<int> LoseADReviveTime { get; set; }

    public const int LoseADReviveTime_Default = 10;

    /// <summary>
    /// 插屏策略，每天每个用户的前面第 N 关使用 2 关一个插屏的策略
    /// </summary>
    public PersistenceData<int> DailySlowLevel { get; set; }

    public const int DailySlowLevel_Default = 0;

    /// <summary>
    /// 游戏中广告，与其他广告之间的间隔时间(秒)
    /// </summary>
    public PersistenceData<float> InsertAdShowTime { get; set; }

    public const int InsertAdShowTime_Default = 120;

    /// <summary>
    /// 已经移除---------------------------------------------不再使用
    /// 結束广告测试 Type=0 猫头鹰 Type=1 第一次出现广告
    /// </summary>
    public PersistenceData<int> EndReviveType { get; set; }

    public const int EndReviveType_Default = 0;

    /// <summary>
    /// 难度机制 AB 测试 0 是归零 1 是每次 -1
    /// </summary>
    public PersistenceData<int> DifficultMode { get; set; }

    public const int DifficultMode_Default = 0;

    /// <summary>
    /// IOS 审核用，是否在商店展示所有礼包
    /// </summary>
    public PersistenceData<bool> IOSDisplayPackageInStore { get; set; }

    public const bool IOSDisplayPackageInStore_Default = false;

    /// <summary>
    /// banner 开启关卡
    /// </summary>
    public PersistenceData<int> BannerOpenLevel { get; set; }

    public const int BannerOpenLevel_Default = 10;

    /// <summary>
    /// 自动插屏广告生效关卡
    /// n 关开始插屏，n+20 关返回主页添加插屏
    /// </summary>
    public PersistenceData<int> IntersistalEffectLevel { get; set; }

    public const int IntersistalEffectLevel_Default = 10;

    /// <summary>
    /// 动态难度正式生效关卡
    /// 10 关  20关 30关
    /// </summary>
    public PersistenceData<int> DynamicDiffEffectLevel { get; set; }

    public const int DynamicDiffEffectLevel_Default = 20;

    /// <summary>
    /// 关卡 AB Test ,0 动态 1 绑定关卡  去除，失效，默认选择动态难度
    /// </summary>
    public PersistenceData<int> UseDiffType { get; set; }

    public const int UseDiffType_Default = 0;

    private string GetDebugString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"LevelUseConfigB:{LevelUseConfigB.Value}\n");
        builder.Append($"FirstBgType:{FirstBgType.Value}\n");
        builder.Append($"LoseADReviveTime:{LoseADReviveTime.Value}\n");
        builder.Append($"DailySlowLevel:{DailySlowLevel.Value}\n");
        builder.Append($"InsertAdShowTime:{InsertAdShowTime.Value}\n");
        //        builder.Append($"EndReviveType:{EndReviveType.Value}\n");
        builder.Append($"DifficultMode:{DifficultMode.Value}\n");
        builder.Append($"UseDiffType:{UseDiffType.Value}\n");
        builder.Append($"DynamicDiffEffectLevel:{DynamicDiffEffectLevel.Value}\n");
        builder.Append($"BannerOpenLevel:{BannerOpenLevel.Value}\n");
        builder.Append($"IntersistalEffectLevel:{IntersistalEffectLevel.Value}\n");
        return builder.ToString();
    }

    public override void Init()
    {
        LevelUseConfigB = new PersistenceData<bool>("RemoteControlSystem_LevelUseConfigB", LevelUseConfigB_Default);
        FirstBgType = new PersistenceData<int>("RemoteControlSystem_FirstBgType", FirstBgType_Default);
        DailySlowLevel = new PersistenceData<int>("RemoteControlSystem_DailySlowLevel", DailySlowLevel_Default);
        InsertAdShowTime = new PersistenceData<float>("RemoteControlSystem_InsertAdShowTime", InsertAdShowTime_Default);
        EndReviveType = new PersistenceData<int>("EndReviveType_DifficultMode", EndReviveType_Default);
        DifficultMode = new PersistenceData<int>("RemoteControlSystem_DifficultMode", DifficultMode_Default);
        UseDiffType = new PersistenceData<int>("RemoteControlSystem_UseDiffType", UseDiffType_Default);
        DynamicDiffEffectLevel =
            new PersistenceData<int>("RemoteControlSystem_DynamicDiffEffectLevel", DynamicDiffEffectLevel_Default);
        IntersistalEffectLevel =
            new PersistenceData<int>("RemoteControlSystem_IntersistalEffectLevel", IntersistalEffectLevel_Default);
        BannerOpenLevel = new PersistenceData<int>("RemoteControlSystem_BannerOpenLevel", BannerOpenLevel_Default);
        IOSDisplayPackageInStore = new PersistenceData<bool>("RemoteControlSystem_IOSDisplayPackageInStore",
            IOSDisplayPackageInStore_Default);
        LoseADReviveTime = new PersistenceData<int>("RemoteControlSystem_LoseADReviveTime", LoseADReviveTime_Default);
        RedeemMenoyCount = new PersistenceData<int>("RemoteControlSystem_RedeemMenoyCount", 0);
    }

    public override void Destroy()
    {
    }
}