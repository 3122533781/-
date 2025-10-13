using Prime31.StateKit;
using UnityEngine;
using _02.Scripts.InGame.UI;
public class InGamePlayingState : SKState<InGame>
{
    public override void begin()
    {
        HandleComponent();
    }

    private void HandleComponent()
    {
        Debug.Log("开始游戏");
        _context.GetController<InGameTimeController>().Activate();
        _context.GetController<InGameTimeController>().seconds = 180;
        _context.GetView<InGamePlayingUI>().SetBarToZero();
    }

    public override void end()
    {
    }
}