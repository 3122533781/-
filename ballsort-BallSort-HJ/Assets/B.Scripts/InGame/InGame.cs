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
    }

    public void CheckInterpolationAd(string pos, bool forceShow = false)
    {
        if (forceShow)
        {
            ADMudule.ShowInterstitialAds(pos, _ => { Restart(); });
        }
        else
        {
            Restart();
        }
    }
    public void ResetLevel()
    {
        Game.Instance.LevelModel.EnterLevelID += 1;
        Game.Instance.LevelModel.MaxUnlockLevel.Value += 1;
        // Game.Instance.LevelModel.EnterLevelSecond = true;
        Game.Instance.LevelModel.TheSmallLevelID += 1;

        if (Game.Instance.LevelModel.TheSmallLevelNumbers == Game.Instance.LevelModel.TheSmallLevelID+1)
            Game.Instance.LevelModel.EnterLevelSecond = true;
        Debug.Log("小关和大关分别为" + Game.Instance.LevelModel.TheSmallLevelID + " " + Game.Instance.LevelModel.TheSmallLevelNumbers);

       
        SoyProfile.Set(SoyProfileConst.NormalLevel, Game.Instance.LevelModel.EnterLevelID);
        Restart();
    }
    public void StartGame()
    {
        Debug.Log("StartGame");
        StateModel.CurrentState = InGameState.Playing;
        SetPlaying();
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
    public int GetBallsType()
    {
        HashSet<BallType> allBallTypes = new HashSet<BallType>();
        Debug.Log("此时" + CellMapModel.LevelPipeList.Count);
        foreach (var pipe in CellMapModel.LevelPipeList)
        {
            if (pipe == null) continue;

            foreach (var ball in pipe.BallLevelEdits)
            {
                if (ball != null)
                {
                    BallType currentType = ball.GetBallData().type;
                    allBallTypes.Add(currentType);
                }
            }
        }
        return allBallTypes.Count;


    }
    public float GetCompletionRate(int BallsType)
    {


        int totalBallTypeCount = Game.Instance.LevelModel.TypeNumber;
        if (totalBallTypeCount <= 0)
        {
            Debug.Log("总球种类为0，完成率为0%"+ CellMapModel.LevelPipeList.Count);
            return 0f;
        }
        int NowLevelNumberCount = 1;
        int NowCompletedCount = CellMapModel.EndFinishNumber;
        Debug.Log("大小为" + NowCompletedCount + " " + Game.Instance.LevelModel.PassLevelNumber.Value + "  " + CellMapModel.TheSmallLevelNumber);
        Debug.Log("分母为" + totalBallTypeCount);
        float completionRate = (float)NowLevelNumberCount / totalBallTypeCount * 100f;
        return Mathf.Round(completionRate * 10f / CellMapModel.TheSmallLevelNumber) / 10f;
    }

    public void CheckIsOver()
    {
        var levelData = CellMapModel.LevelPipeList;
        var isOver = levelData.Find(x => !x.PipeFullOrEmpty()) == null;
       // Debug.Log("当前进度为" + GetCompletionRate() + "%");
        if (isOver && !Game.Instance.LevelModel.EnterLevelSecond)
        {
            Debug.Log("游戏流程结束");
            ResetLevel();
        }
        else if (isOver && Game.Instance.LevelModel.EnterLevelSecond)
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