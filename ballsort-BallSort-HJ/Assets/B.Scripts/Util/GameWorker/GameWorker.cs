using Sirenix.OdinInspector;

namespace Soybean.GameWorker
{
    /// <summary>
    /// 流程测试，集成测试 ，按照设计通过一个个设计好的关卡流程，验证有效性
    /// </summary>
    public class GameWorker : MonoSingleton<GameWorker>
    {
        public GameWorkerModel Model { get; private set; }

        [Button]
        public void StopWork()
        {
            Model.IsWorking = false;
        }
        
        [Button]
        public void StartWork()
        {
            Model.IsWorking = true;
        }

        protected override void HandleAwake()
        {
            Model = new GameWorkerModel();
            _gui = new GameWorkerGUI();
            _gui.Init(this);
            _control = new GameWorkerControl();
            _control.Init(this);
        }

        private void OnGUI()
        {
            if (_gui != null && Model.IsWorking)
            {
                _gui.Draw();
            }
        }

        private void Update()
        {
            if (Model.IsWorking && !Model.IsFinish)
            {
                _control.Update();
            }
        }

        private GameWorkerGUI _gui;
        private GameWorkerControl _control;
    }
}