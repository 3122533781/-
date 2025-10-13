using System;
using System.Linq;
using _02.Scripts.Config;
using _02.Scripts.LevelEdit;
using UnityEngine;

public enum CopiesType
{
    Thread = 0, //主线
    SpecialLevel, //副本一
}

public class GameLevelModel
{
    public int NowLevelID;
    public int EnterLevelID { get; set; }
    public int EnterCopies1ID;

    public PersistenceData<int> MaxUnlockLevel;
    public PersistenceData<int> MaxUnlockCopies1;
    public PersistenceData<int> StoreGold;

    public CopiesType CopiesType { get; set; }

    //当天打过的关卡数
    public PersistenceData<int> TodayHasPlayedLevel;

    //引导组
    public PersistenceData<int> GuideGroup;

    //剧情引导组
    public PersistenceData<int> PlotGuideGroup;

    //当前关卡打过的次数
    public PersistenceData<int> LevelAttemptNum;

    public bool HaveReviveUseAD { get; set; }

    //是否触发过大转盘
    private bool _isTriggerSpecialLevel;

    public void PlusLevelAttemptNum()
    {
        if (CopiesType == CopiesType.Thread) LevelAttemptNum.Value++;
    }

    public void RestLevelAttemptNum()
    {
        if (CopiesType == CopiesType.Thread) LevelAttemptNum.Value = 0;
    }

    public void PlusTodayLevelCount()
    {
        TodayHasPlayedLevel.Value++;
    }

    public void ResetReviveUseAD()
    {
        HaveReviveUseAD = false;
    }

    public void SetEnterLevelID(int value)
    {
        if (CopiesType == CopiesType.Thread) EnterLevelID = value;
        else if (CopiesType == CopiesType.SpecialLevel) EnterCopies1ID = value;
    }

    public void PassCurrentLevel(int pass = 1)
    {
        if (CopiesType == CopiesType.Thread)
        {
            int nextLevel = EnterLevelID + pass;
            if (MaxUnlockLevel.Value < nextLevel && GetMaxLevel >= nextLevel)
            {
                MaxUnlockLevel.Value = nextLevel;
                SoyProfile.DelaySet(SoyProfileConst.NormalLevel, MaxUnlockLevel.Value);
                RestLevelAttemptNum();
            }
        }
        else if (CopiesType == CopiesType.SpecialLevel)
        {
            int nextLevel = EnterCopies1ID + pass;

            MaxUnlockCopies1.Value = nextLevel;
            EnterLevelID = MaxUnlockLevel.Value;
            SoyProfile.DelaySet(SoyProfileConst.SpecialLevel, MaxUnlockCopies1.Value);
        }
    }

    public void AddStoreGold(int number)
    {
        StoreGold.Value += number;
    }



    public void RestartStoreGold()
    {
        StoreGold.Value = 0;
    }

    public int GetMaxLevel
    {
        get
        {
            var maxLevel = LevelConfig.Instance.All.Max(d => d.ID);
            return maxLevel;
        }
    }

    public void SetTrigger(bool set)
    {
        _isTriggerSpecialLevel = set;
    }

    public bool GetTrigger()
    {
        return _isTriggerSpecialLevel;
    }

    public GameLevelModel()
    {
        int dayTimeStamp = DataFormater.GetDayTimeStamp(DateTime.Now);
        TodayHasPlayedLevel = new PersistenceData<int>($"AppModel_tol_{dayTimeStamp}", 0);
        HaveReviveUseAD = false;
        MaxUnlockLevel = new PersistenceData<int>("AppLevelModel_MaxUnlockLevel", 1);
        MaxUnlockCopies1 = new PersistenceData<int>("AppLevelModel_MaxUnlockCopies1", 1);
        GuideGroup = new PersistenceData<int>("AppLevelModel_GuideGroup", 0);
        PlotGuideGroup = new PersistenceData<int>("AppLevelModel_PlotGuideGroup", 0);
        LevelAttemptNum = new PersistenceData<int>("AppLevelModel_LevelAttemptNum", 0);
        StoreGold = new PersistenceData<int>("AppLevelModel_StoreGold", 0);
        EnterLevelID = MaxUnlockLevel.Value;
    }
}