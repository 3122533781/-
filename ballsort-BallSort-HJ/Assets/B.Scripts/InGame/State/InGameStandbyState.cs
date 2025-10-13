using Prime31.StateKit;
using System.Linq;
using _02.Scripts.InGame.Controller;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;

public class InGameStandbyState : SKState<InGame>
{
    public override void begin()
    {
        StaticModule.GameFlow_EnterGame();
        HandleStatic();
        HandleComponent();
        // _context.GetController<InGameMapController>().SetLevelData();
        //_context.GetController<InGameBoardController>().CheckMap();
        //_context.GetView<InGameLoseGameUI>().ResetWatchAD();
    }

    private void HandleComponent()
    {
        //_context.GetController<InGameBoardController>().Activate();
        //_context.GetController<InGameMapController>().Activate();
        _context.GetController<InGameTimeController>().Deactivate();
    }

    private void HandleStatic()
    {
        StaticModule.BeginStage(Game.Instance.LevelModel.EnterLevelID, Game.Instance.LevelModel.LevelAttemptNum.Value, Game.Instance.LevelModel.CopiesType);
    }
}