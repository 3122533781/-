using Fangtang;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Framework.ElementKit;
using Soybean.GameWorker;
using UnityEngine;

public class DebugLevel : IDebugPage
{
    public string Title
    {
        get { return "Level"; }
    }

    public void Draw()
    {
        GUILayout.Label($"MaxUnlock:{Game.Instance.LevelModel.MaxUnlockLevel.Value}");

        if (GUILayout.Button("Open GameWorker"))
        {
            GameWorker.Instance.StartWork();
            // DebugConsoleControl.Instance.SwitchDebug();
        }
        
        if (_inGame == null)
        {
            _inGame = SceneElementManager.Instance.Resolve<InGame>();
        }

        if (_inGame == null)
            return;

//
//        GUILayout.Label("累计的插屏时长：" +
//                        DataFormater.ToTimeString(App.Instance.GetSystem<ADStrategySystem>().AccIntersistalTime));
//        GUILayout.Label("思考时长：" + DataFormater.ToTimeString(_inGame.LevelModel.ThinkDuration));
//        GUILayout.Label("当前关卡时长：" + DataFormater.ToTimeString(_inGame.LevelModel.GameDuration));
//        GUILayout.Label("剩余棋子:" + _inGame.MatchModel.RemainBlockCount);
//        GUILayout.Label("(动态难度等级)Dynamic:" + App.Instance.GetSystem<DynamicDiffSystem>().DiffLevelNum.Value);
//        GUILayout.Label("(绑定难度等级)Bind:" + _inGame.DiffModel.BindRuleData.Difficulty_Lv);
//        GUILayout.Label("(连胜场次)WinningRound:" + App.Instance.GetSystem<WinningStreakSystem>().WinningRound.Value);
//        GUILayout.Label("当天打过关卡数：" + App.Instance.LevelModel.TodayHasPlayedLevel.Value);
//
//        if (GUILayout.Button("Start Next Level"))
//        {
//            App.Instance.LevelModel.PassCurrentLevel();
//            App.Instance.EnterGame();
//        }
//
//        string value = GUILayout.TextField(_skipTo.ToString());
//        if (int.TryParse(value, out int toValue))
//        {
//            _skipTo = toValue;
//            if (GUILayout.Button("SkipToLevel n"))
//            {
//                App.Instance.LevelModel.EnterLevelID = _skipTo;
//                App.Instance.LevelModel.MaxUnlockLevel.Value = _skipTo + 1;
//                App.Instance.EnterGame();
//            }
//            
//            if (GUILayout.Button("SkipToLevel challenge"))
//            {
//                App.Instance.EnterDailyChallengeLevel();
//                App.Instance.LevelModel.EnterLevelID = _skipTo;
//            }
//        }
    }

    private int _skipTo;
    private InGame _inGame;
}