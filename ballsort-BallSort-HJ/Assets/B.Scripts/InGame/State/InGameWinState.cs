using _02.Scripts.InGame.UI;
using Prime31.StateKit;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;

namespace _02.Scripts.InGame.State
{
    public class InGameWinState : SKState<global::InGame>
    {
        public override void begin()
        {
            HandleStatic();

            Game.Instance.LevelModel.PassCurrentLevel();
            Game.Instance.LevelModel.EnterLevelSecond = false;
            Game.Instance.LevelModel.TheSmallLevelID = 0;
            _context.GetView<InGameSuccessUI>().Activate();  
            HandleComponent();
        }

        private void HandleStatic()
        {
         
            StaticModule.CompletedStage(Game.Instance.LevelModel.EnterLevelID,
                Game.Instance.LevelModel.LevelAttemptNum.Value,
                0, Game.Instance.LevelModel.CopiesType);
        }

        private void HandleComponent()
        {
            _context.GetController<InGameTimeController>().Deactivate();
        }

        public override void end()
        {
            base.end();
            if (!Game.Instance.Model.IsWangZhuan())
            {
                Game.Instance.CurrencyModel.ConsumeDiamond(1);
                if (Game.Instance.CurrencyModel.DiamondNum < 1)
                {
                    TransitionManager.Instance.Transition(0.5f, () => { SceneManager.LoadScene("InGame"); },
               0.5f);
                }
            }

            //_context.GetView<InGameWinGameUI>().Deactivate();
        }
    }
}