using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public enum QuestType
{
    Completebowlplating,
    Completeplateplating,
    Completecupplating,
    Completechopsticksplating,
    Completeknifeplating
}
public enum QuestStatus
{
    Pendding,
    Finished,
    Claimed
}
[System.Serializable]
    public struct QuestConfig
    {
    public int ID;

    public GoodSubType2 rewardType;

    public QuestType questType;

    public QuestStatus status;


    public int rewardCount;

    public int targetCount;

    public string description;

}
    public class QuestConfigs : ScriptableObject
    {
        public QuestConfig[] quests;

        //[MenuItem("Mahjong/Russian/CreateQuestConfig")]
        //static void CreateLevelConfigAssetInstance()
        //{
        //    var ConfigAsset = CreateInstance<QuestConfigs>();

        //    AssetDatabase.CreateAsset(ConfigAsset, "Assets/Russian/Resources/Configs/QuestConfig.asset");
        //    AssetDatabase.Refresh();
        //}
    }


