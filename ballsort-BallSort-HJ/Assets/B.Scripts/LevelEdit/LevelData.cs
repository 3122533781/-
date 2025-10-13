using System;
using System.Collections.Generic;
using _02.Scripts.Config;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;

namespace _02.Scripts.LevelEdit
{
    /// <summary>
    /// 关卡数据类
    /// </summary>
    [Serializable]
    public class LevelData : IConfig
    {
        public int levelId;
        public PipeNumber pipeNumber;
        public List<PipeData> pipeDataList;
        public bool blindBox;
        private RewardData _rewardData;

        public int GetPipeCount()
        {
            return (int)pipeNumber;
        }

        public PipeCapacity GetPipeCapacity()
        {
            if (pipeDataList == null || pipeDataList.Count == 0)
            {
                return PipeCapacity.None;
            }

            return pipeDataList[0].pipeCapacity;
        }

        public void Clean()
        {
            pipeNumber = 0;
            pipeDataList.Clear();
        }


        public void InitNewLevelData(int newLevelId, PipeNumber pipeNumber1, PipeCapacity pipeCapacity)
        {
            levelId = newLevelId;
            pipeDataList = new List<PipeData>();
            pipeNumber = pipeNumber1;
            for (int i = 0; i < (int)pipeNumber1; i++)
            {
                pipeDataList.Add(new PipeData(pipeCapacity));
            }

            blindBox = false;
        }


        public void Reset(int levelId, PipeNumber pipeNumber, PipeCapacity pipeCapacity, List<PipeData> pipeDataList)
        {
            this.levelId = levelId;
            this.pipeNumber = pipeNumber;
            this.pipeDataList = pipeDataList;
        }


        public RewardData GetRandomCoin()
        {
            return _rewardData;
        }

        public void SetRandomCoin()
        {
           
        }

        public int ID => levelId;
    }


    /// <summary>
    /// 管子数据类
    /// </summary>
    [Serializable]
    public class PipeData
    {
        public PipeCapacity pipeCapacity;
        public Common.Stack<BallData> ballDataStack;
        public Typeexclusive exclusiveType;
        public FreezeType freezetype;
        public IsActive isactive;
        public CanFull canfull;
        public IsNeedAD isneedad;
        public PipeData(PipeCapacity pipeCapacity)
        {
            this.pipeCapacity = pipeCapacity;
            ballDataStack = new Common.Stack<BallData>();
            exclusiveType = Typeexclusive.None; // 默认非专属
            freezetype = FreezeType.None;
            isneedad = IsNeedAD.NoNeedAd;
            canfull = CanFull.Canfull;
            isactive = IsActive.Istrue;

        }

        public BallData Pop()
        {
            return ballDataStack.Pop();
        }

        public void Push(BallData data)
        {
            ballDataStack.Push(data);
        }

        public bool CanPushBall()
        {
            return ballDataStack.Count < (int)pipeCapacity;
        }

        public bool IsCanAddPipeCapacity()
        {
            return pipeCapacity < PipeCapacity.Capacity5;
        }
    }


    /// <summary>
    /// 管子数据类
    /// </summary>
    [Serializable]
    public class BallData
    {
        public BallType type;

        public BallData(BallType ballType)
        {
            type = ballType;
        }
    }

    [Serializable]
    public enum PipeNumber
    {
        None = 0,
        Number2 = 2,
        Number3 = 3,
        Number4 = 4,
        Number5 = 5,
        Number6 = 6,
        Number7 = 7,
        Number8 = 8,
        Number9 = 9,
        Number10 = 10,
        Number11 = 11,
        Number12 = 12,
        Number13 = 13,
        Number14 = 14,
        Number15 = 15,

    }
    public enum Typeexclusive
    {
        None = 0,
        Number1,
        Number2,
        Number3,
        Number4,
        Number5,
        Number6,
        Number7,
        Number8,
        Number9,
    }
    public enum FreezeType
    {
        None = 0,
        Number1,
        Number2,
        Number3,
        Number4,
        Number5,
        Number6,
        Number7,
        Number8,
        Number9,
    }




    [Serializable]
    public enum IsActive
    {
        Istrue,
        Isfalse
    }
    public enum CanFull
    {
        Canfull,
        Notfull
    }
    public enum IsNeedAD
    {
        NoNeedAd,
        NeedAd

    }

    [Serializable]
    public enum PipeCapacity
    {
        None = 0,
        Capacity1 = 1,
        Capacity2 = 2,
        Capacity3 = 3,
        Capacity4 = 4,
        Capacity5 = 5,
    }

    [Serializable]
    public enum BallType
    {
        ID1 = 1,
        ID2,
        ID3,
        ID4,
        ID5,
        ID6,
        ID7,
        ID8,
        ID9,
        Coin,
        Money,
    }
}