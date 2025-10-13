using System;
using Fangtang;
using Fangtang.Utils;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Framework.ElementKit;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : Singleton<Game>
{
    public GameModel Model { get; private set; }
    public GameLevelModel LevelModel { get; private set; }

    public GameCurrencyModel CurrencyModel { get; private set; }
    public SceneManager SceneManager { get; private set; }
    public SystemGroup SystemGroup { get; private set; }
    public LocalUserData LocalUser { get; private set; }
    public GameStatic GameStatic { get; private set; }
    public AppRemoteConfig RemoteConfig { get; private set; }

    public bool IsBasicComponentLoaded { get; set; }
    public bool isSDKInitCompleted = false;

    public Transform ManualTrans { get; set; }
    public Transform CoinTrans { get; set; }
    public Transform DiamondTrans { get; set; }
    public Transform MagicTrans { get; set; }
    public Transform MoneyTrans { get; set; }
    public Transform StarTrans { get; set; }
    public Transform BigTurnTrans { get; set; }

    public void InitCreateInstance()
    {
    }

    public void BackHome(Action backHomeAction = null)
    {
        Game.Instance.GetSystem<ADStrategySystem>().PlusBackHomeTime();
        if (GetSystem<ADStrategySystem>().NeedBackHomeShowIntersistal())
        {
            ADMudule.ShowInterstitialAds("BackHome",
                (isSuccess) => { GetSystem<ADStrategySystem>().ResetBackHomeTime(); });
        }

        ADMudule.HideBanner();
        TransitionManager.Instance.Transition(0.5f,
            () => { SceneManager.LoadScene(SceneManager.HomeScene); },
            0.5f, () => { backHomeAction?.Invoke(); });
    }

    private void SetEnterGameLevel(int levelValue, CopiesType type = CopiesType.Thread)
    {
        LevelModel.CopiesType = type;
        GameStatic.PlusPlayTime();
        LevelModel.SetEnterLevelID(levelValue);
        LevelModel.NowLevelID = levelValue;
    }

    public void EnterGame(int levelValue = default, CopiesType type = CopiesType.Thread)
    {
        SetEnterGameLevel(levelValue == default ? LevelModel.MaxUnlockLevel.Value : levelValue, type);
        TransitionManager.Instance.Transition(0.5f,
            () => { SceneManager.LoadScene(SceneManager.GameStageScene); },
            0.5f);
    }

    public void RestartGame(string pos, int levelValue, CopiesType type = CopiesType.Thread,
        bool forceShowAd = false)
    {
        SetEnterGameLevel(levelValue, type);

        var inGame = SceneElementManager.Instance.Resolve<InGame>();
        Debug.Log("11");
        if (inGame)
        {
            Debug.Log("1122");
            inGame.CheckInterpolationAd(pos, forceShowAd);
        }
        else
        {
            Debug.Log("112233");
        }
    }

    public void NotUseCode()
    {
        if (Game.Instance.LevelModel.CopiesType == CopiesType.Thread)
        {
            if (Instance.LevelModel.EnterLevelID ==
                Game.Instance.LevelModel.MaxUnlockLevel.Value - 1)
            {
                Instance.RestartGame("InGameWin_Redeem", Game.Instance.LevelModel.EnterLevelID + 1);
                if (DialogManager.Instance.GetDialog<LevelUIDialog>() != null)
                    DialogManager.Instance.GetDialog<LevelUIDialog>().PassLastLevel();
            }
            else
            {
                Game.Instance.RestartGame("InGameWin_Redeem", Game.Instance.LevelModel.EnterLevelID + 1);
            }
        }
        else
        {
            Game.Instance.RestartGame("InGameWin_Redeem", Game.Instance.LevelModel.EnterLevelID);
        }
    }

    public T GetSystem<T>() where T : GameSystem
    {
        return SystemGroup.GetSystem<T>();
    }

    private Game()
    {
        int seed = DateTime.Now.Millisecond;
        LDebug.Log($"App instantiate. seed:{seed}");
        Random.InitState(seed);
        SceneElementManager.Instance.Init();
        Application.targetFrameRate = 60;
        SystemGroup = new SystemGroup();
        Model = new GameModel();
        CurrencyModel = new GameCurrencyModel();
        LevelModel = new GameLevelModel();
        SceneManager =
            new SceneManager(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        LocalUser = new LocalUserData();
        GameStatic = new GameStatic();
        RemoteConfig = new AppRemoteConfig();
        InitSystem();
    }

    private void InitSystem()
    {
        SystemGroup.RegisterSystem(new CheckinSystem());
        SystemGroup.RegisterSystem(new LocalUserSystem());
        SystemGroup.RegisterSystem(new RemoteControlSystem());
        SystemGroup.RegisterSystem(new ADStrategySystem());
        SystemGroup.RegisterSystem(new RatingSystem());
        SystemGroup.RegisterSystem(new InGameSystem());
  
    }

    private void UnloadSystem()
    {
        SystemGroup.UnregisterSystem<CheckinSystem>();
        SystemGroup.UnregisterSystem<LocalUserSystem>();
        SystemGroup.UnregisterSystem<RemoteControlSystem>();
        SystemGroup.UnregisterSystem<ADStrategySystem>();
        SystemGroup.UnregisterSystem<RatingSystem>();
        SystemGroup.UnregisterSystem<InGameSystem>();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        UnloadSystem();
        LDebug.Log("App destory.");
    }
}