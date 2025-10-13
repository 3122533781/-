
using System.Collections;
using System.Collections.Generic;
using Prime31;
using UnityEngine;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
public class QuestManager : MonoSingleton<QuestManager>, Prime31.IObjectInspectable
{
    protected override void HandleAwake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        InitListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void InitListeners()
    {
        EventManager.On("Completebowlplating", CompletebowlplatingListener);
        //EventManager.On("Completeplateplating", CollectStarListener);
        //EventManager.On("Completecupplating", UseBoosterListener);
        //EventManager.On("Completechopsticksplating", SolitaireLevelClearListener);
        //EventManager.On("Completeknifeplating", UseSoliBoosterListener);
    }

    private void RemoveListeners()
    {
        EventManager.Off("Completebowlplating", CompletebowlplatingListener);
        //EventManager.Off("win-dialog", CollectStarListener);
        //EventManager.Off("use-booster", UseBoosterListener);
        //EventManager.Off("solitaire-level-clear", SolitaireLevelClearListener);
        //EventManager.Off("solitaire-use-booster", UseSoliBoosterListener);
    }



    private void CompletebowlplatingListener(Object param)
    {
        AddQuestFinishCount(QuestType.Completebowlplating);
        if(GetQuestStatus(QuestType.Completebowlplating))
        DialogManager.Instance.GetDialog<GetRewardDialog>().Init(GoodSubType2.AddPipe);
    }

    private void SolitaireLevelClearListener(Object param)
    {
        // 通关一次Solitaire关卡，增加1点进度
       // AddQuestFinishCount(QuestType.PassSolitaireLevel, 1);
    }

    private void CollectStarListener(Object param)
    {
        //Score score = (Score)param;
        //if (score)
        //{
        //    AddQuestFinishCount(QuestType.CollectStar, score.GetStar());
        //    int addStar = Mathf.Max(0, (score.GetStar() - PlayerDataStorage.GetLevelStars(LevelManager.currentLevelIndex)));
        //    //            App.Instance.GetSystem<StarChestSystem>().Model.ProgressInt += addStar;
        //    App.Instance.GetSystem<StarChestSystem>().Model.NotDisplayInt.Value += addStar;
        //    PlayerDataStorage.SetLevelStars(LevelManager.currentLevelIndex, score.GetStar());
        //}
    }

    private void UseBoosterListener(Object param)
    {
      //  AddQuestFinishCount(QuestType.UseBooster, 1);
    }

    private void UseSoliBoosterListener(Object param)
    {
      //  AddQuestFinishCount(QuestType.UseSoliBooster, 1);
    }

    private void AddQuestFinishCount(QuestType questType)
    {
        List<Quest> quests = GameDataManager.Instance.GetQuest();
        List<Quest> passLevelQuests = quests.FindAll(i => i.questType == questType);
        foreach (Quest quest in passLevelQuests)
        {
            if (quest.status == QuestStatus.Finished || quest.status == QuestStatus.Claimed) continue;
            quest.finishedCount += 1;

            if (quest.finishedCount >= quest.targetCount)
            {
                quest.status = QuestStatus.Finished;
            }
        }

        GameDataManager.Instance.SaveData();
    }
    public bool GetQuestStatus(QuestType questType)
    {
        List<Quest> quests = GameDataManager.Instance.GetQuest();
        Quest quest = quests.Find(q => q.questType == questType);
        if (quest.status==QuestStatus.Finished)
        {
            return true;
        }

        return false;
    }

}