using ProjectSpace.Lei31Utils.Scripts.Framework.App;

public class InGameLoseGameUI : ElementUI<InGame>
{

    private void RestartStage()
    {
        Game.Instance.LevelModel.PlusLevelAttemptNum();
        StaticModule.RestartStage(Game.Instance.LevelModel.EnterLevelID, Game.Instance.LevelModel.LevelAttemptNum.Value, Game.Instance.LevelModel.CopiesType);
    }
}
