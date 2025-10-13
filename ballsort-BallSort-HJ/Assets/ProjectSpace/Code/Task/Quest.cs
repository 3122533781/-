using UnityEngine;
using System.Collections;
using System;



    [System.Serializable]
    public class Quest
    {
        public int ID;

        public QuestType questType;

        public QuestStatus status;

        public int rewardCount;

        public int targetCount;

        public int finishedCount;

        public string description;

        public string date;
    }
