using _02.Scripts.InGame;
using _02.Scripts.InGame.Controller;
using _02.Scripts.InGame.State;
using _02.Scripts.InGame.UI;
using Fangtang;
using ProjectSpace.BubbleMatch.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using _02.Scripts.LevelEdit;
public class InGame : GenericSceneElement<InGame, InGameState>, Prime31.IObjectInspectable
{
    public InGameModel CellMapModel { private set; get; }

    protected override void OnInit(object data)
    {
        ADMudule.ShowBanner();
        CellMapModel = new InGameModel();
        Models.Add(CellMapModel);

        Views.Add(GetComponentInChildren<InGamePlayingUI>(true));
        Views.Add(GetComponentInChildren<InGamePauseUI>(true));
        Views.Add(GetComponentInChildren<InGameSuccessUI>(true));
        Views.Add(GetComponentInChildren<InGameFail>(true));
        Views.Add(GetComponentInChildren<InGameWinReward>(true));

        Views.Add(GetComponentInChildren<InGameBoxDialog>(true));
 
        Controllers.Add(GetComponent<InGameTimeController>());
        Controllers.Add(GetComponentInChildren<InGameMapController>());
        Controllers.Add(GetComponentInChildren<InGameMatchController>());

        AddState(new InGameStandbyState(), InGameState.Standby);
        AddState(new InGamePlayingState(), InGameState.Playing);
        AddState(new InGamePauseState(), InGameState.Pause);
        AddState(new InGameWinState(), InGameState.Win);
        AddState(new InGameFailedState(), InGameState.Failed);

        AddStateTransition<InGameStandbyState, InGamePlayingState>();
        AddStateTransition<InGamePlayingState, InGameStandbyState>();
        AddStateTransition<InGamePlayingState, InGamePauseState>();
        AddStateTransition<InGamePlayingState, InGameStandbyState>();
        AddStateTransition<InGamePauseState, InGamePlayingState>();
        AddStateTransition<InGamePlayingState, InGameWinState>();
        AddStateTransition<InGamePlayingState, InGameFailedState>();
        AddStateTransition<InGameWinState, InGameStandbyState>();
        AddStateTransition<InGameWinState, InGamePlayingState>();
        AddStateTransition<InGameFailedState, InGamePlayingState>();
        AddStateTransition<InGameStandbyState, InGamePauseState>();
        AddStateTransition<InGamePauseState, InGameWinState>();
        AddStateTransition<InGameFailedState, InGameStandbyState>();
        AddStateTransition<InGamePauseState, InGameStandbyState>();

        Restart();

        CheckGuide();

        // AudioClipHelper.Instance.PlaySound(AudioClipEnum.Seagull);
    }

    private void Start()
    {
        SpriteManager.Instance.InitSkin();
    }

    private void SetPlaying()
    {
        Game.Instance.GetSystem<InGameSystem>().IsPlaying = StateModel.CurrentState == InGameState.Playing;
    }

    public void Restart()
    {
        Debug.Log("Restart");
        Game.Instance.LevelModel.PlusLevelAttemptNum();

        StateModel.CurrentState = InGameState.Standby;
        StateModel.CurrentState = InGameState.Playing;

        SetPlaying();
        InitData();
        GetView<InGamePlayingUI>().RefreshUI();
        Game.Instance.GameStatic.PlusOpenGameTime();
        // JobUtils.Delay(0.5f, StartGame);
    }

    /// <summary>
    /// 10?????????????β???
    /// ???????????е?
    /// ????????е?
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="forceShow"></param>
    public void CheckInterpolationAd(string pos, bool forceShow = false)
    {
        //App.Instance.LevelModel.MaxUnlockLevel.Value >= ConstantConfig.Instance.GetInterpolationAd() ||????????
        if (forceShow)
        {
            ADMudule.ShowInterstitialAds(pos, _ => { Restart(); });
        }
        else
        {
            Restart();
        }
    }

    public void StartGame()
    {
        Debug.Log("StartGame");
        StateModel.CurrentState = InGameState.Playing;
        SetPlaying();
        //JobUtils.Delay(0.2f, () => { GetController<InGameGuideController>().StratGuid(); });
    }

    public void Continue()
    {
        Debug.Log("Continue");
        StateModel.CurrentState = InGameState.Playing;
        SetPlaying();
    }

    public void Pause(bool isPause = true)
    {
        Debug.Log("Pause");
        Game.Instance.GetSystem<InGameSystem>().IsClickPause = isPause;
        StateModel.CurrentState = InGameState.Pause;
        SetPlaying();
    }

    public void Failed()
    {
        Debug.Log("Failed");
        StateModel.CurrentState = InGameState.Failed;
        SetPlaying();
    }

    public void Win()
    {
        Debug.Log("Win");
        StateModel.CurrentState = InGameState.Win;
        SetPlaying();
    }

    public bool IsWin()
    {
        return StateModel.CurrentState == InGameState.Win;
    }

    private void OnDestroy()
    {

        GetModel<InGameModel>().Dispose();
    }

    private void InitData()
    {
        GetController<InGameMapController>().StartGame();
    }

    public void CheckGuide()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Win();
        }
    }

    /// <summary>
    /// 修改后：完成度 =（已完成的球种类数 / 总球种类数）× 100%
    /// 已完成的球种类：至少有一个管子“装满且同色”的球类型
    /// 总球种类：当前关卡中所有独特的球类型
    /// </summary>
    public float GetCompletionRate()
    {
        // 1. 防护：管子列表为空或无效时，返回0%
        if (CellMapModel == null || CellMapModel.LevelPipeList == null || CellMapModel.LevelPipeList.Count <= 0)
        {
            return 0f;
        }

        // 2. 统计“总球种类数”（去重）和“已完成的球种类数”
        HashSet<BallType> allBallTypes = new HashSet<BallType>(); // 所有独特的球类型
        HashSet<BallType> completedBallTypes = new HashSet<BallType>(); // 已完成的球类型（有管子装满同色）

        foreach (var pipe in CellMapModel.LevelPipeList)
        {
            if (pipe == null) continue; // 跳过空管子

            // 2.1 收集当前管子中的所有球类型，统计“总球种类”
            foreach (var ball in pipe.BallLevelEdits)
            {
                if (ball != null)
                {
                    BallType currentType = ball.GetBallData().type;
                    allBallTypes.Add(currentType); // HashSet自动去重
                }
            }

            // 2.2 若管子“装满且同色”（完成状态），收集该球类型到“已完成种类”
            if (pipe.PipeFullOrEmpty() && pipe.BallLevelEdits.Count > 0)
            {
                BallType completedType = pipe.BallLevelEdits.Peek().GetBallData().type;
                completedBallTypes.Add(completedType);
            }
        }

        // 3. 防护：总球种类为0时（无球关卡），返回0%（避免除以0）
        int totalBallTypeCount = allBallTypes.Count;
        if (totalBallTypeCount <= 0)
        {
            return 0f;
        }

        // 4. 计算完成度（四舍五入保留1位小数）
        float completionRate = (float)completedBallTypes.Count / totalBallTypeCount * 100f;
        return Mathf.Round(completionRate * 10f) / 10f;
    }

    public void CheckIsOver()
    {
        var levelData = CellMapModel.LevelPipeList;
        var isOver = levelData.Find(x => !x.PipeFullOrEmpty()) == null;
        Debug.Log("当前进度为" + GetCompletionRate() + "%"); // 补充百分号，显示更直观
        if (isOver)
        {
            Win();
        }
    }

#if UNITY_EDITOR

    [Button]
    public void JumpToLevel(int a)
    {
        Game.Instance.LevelModel.EnterLevelID = a;
        Game.Instance.LevelModel.MaxUnlockLevel.Value = a;
        SoyProfile.Set(SoyProfileConst.NormalLevel, a);
    }

    [Button]
    public void TestCoin()
    {
     
    }

#endif
}

public enum InGameState
{
    Null,
    Standby,
    Playing,
    Pause,
    Win,
    Failed,
}