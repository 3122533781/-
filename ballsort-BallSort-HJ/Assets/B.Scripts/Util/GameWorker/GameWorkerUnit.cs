namespace Soybean.GameWorker
{
    /// <summary>
    /// 一个单元代表了一次点击，一次点击完成，说明成功通过了这个单元的测试
    /// </summary>
    [System.Serializable]
    public class GameWorkerUnit
    {
        public string Name;
        
        /// <summary>
        /// 按钮路径，代表此次点击目标
        /// </summary>
        public string ButtonPath;

        public UnitState State;

        //是否必要，如果是，则必须要通过后才能开始下一个必要单元
        //如果为否，则一系列非必要单元可以依附于必要单元同时激活，通过之后移除自身，但是不影响其他单元，如果依附的必要单元通过，则被清除
        public bool IsNecessary;
//
//        //检测到按钮之后，延迟多少秒去点击
//        public float WaitToClick = 1f;
        
        public GameWorkerUnit Clone()
        {
            return (GameWorkerUnit)this.MemberwiseClone();
        }
    }

    public enum UnitState
    {
        Standby, //等待
        Active, //激活 //是否激活，如果进入了，则开始等待按钮点击
        Pass, //通过  //是否通过，按钮点击后则通过
        Remove, //清除
    }
}