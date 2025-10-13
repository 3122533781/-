using _02.Scripts.InGame.UI;
using Prime31.StateKit;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;

public class InGamePauseState : SKState<InGame>
{
    public override void begin()
    {
        if(Game.Instance.GetSystem<InGameSystem>().IsClickPause) _context.GetView<InGamePauseUI>().Activate();
        HandleComponent();
    }
    
    private void HandleComponent()
    {
        _context.GetController<InGameTimeController>().Deactivate();
    }

    public override void end()
    {
        base.end();
        _context.GetView<InGamePauseUI>().Deactivate();
    }
}