using System.Collections.Generic;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;

namespace ProjectSpace.BubbleMatch.Scripts.SubSystem.ActivitySystem
{
    public abstract class ActivitySystem : GameSystem
    {
        public override void Init()
        {
        }

        public override void Destroy()
        {
        }

        //检测当前是否有新功能
        public UnlockActiveData CheckNewFunction()
        {
            var passLevel = Game.Instance.LevelModel.MaxUnlockLevel.Value;
            var active = UnlockActiveConfig.Instance.All.Find(x => x.unlockLevel == passLevel);
            return active;
        }

        public bool IsUnlockByLevel(DialogName type)
        {
            var res = false;
            //遍历关卡解锁中是否有
            foreach (var data in UnlockActiveConfig.Instance.All)
            {
                if (type == data.id) //是通过关卡解锁的功能
                {
                    res = true;
                }
            }

            return res;
        }

        public List<UnlockActiveData> GetAllUnlockActive()
        {
            List<UnlockActiveData> res = new List<UnlockActiveData>();
            foreach (var activeData in UnlockActiveConfig.Instance.All)
            {
                if (activeData.unlockLevel <= Game.Instance.LevelModel.MaxUnlockLevel.Value)
                {
                    res.Add(activeData);
                }
            }

            return res;
        }

        public bool IsUnlockActive(DialogName type)
        {
            if (!IsUnlockByLevel(type))//不是通过关卡解锁的活动
            {
                return true;
            }

            return GetAllUnlockActive().Find(x => x.ID == (int) type) != null;
            
        }
    }
}