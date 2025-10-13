using System.IO;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class GameData
    {
        // 当前的关卡
        public int currentLevelIndex;

        public int highestLevelIndex;

        // 金币
        public int coin;

        // 生命
        public int life;

        public bool disableShowNumberHit;

        public bool disableMusic;
        public bool disableSound;
        public bool disableVibrate;
        public List<Quest> quests;

        public int themeIndex = 0;
        public int themeBGIndex = 1;
        public bool selectedTheme = false;

        public int lastIntertialLevel = 0;
        public long lastIntertialTime = System.DateTime.Now.Ticks;
        public long lastRewardedTime = System.DateTime.Now.Ticks;

  
    }

    public class GameDataManager : MonoSingleton<GameDataManager>
    {
        public GameData gameData;

        protected override void HandleAwake()
        {
            //DontDestroyOnLoad(this);
            Init();
        }

        public void Init()
        {
            gameData = Load<GameData>();
            if (gameData == null)
            {
                gameData = new GameData();
                gameData.currentLevelIndex = 1;
                gameData.highestLevelIndex = 1;
            }
        CollectionManager.Instance.Initialize();
        }

        public void ClearAllData()
        {
            string saveFileName = "gamesave.dat"; // 与 Save/Load 方法的文件名一致
            string saveFilePath = Application.persistentDataPath + "/" + saveFileName;

            // 1. 第一步：删除本地的 gamesave.dat 文件（清除持久化数据）
            try
            {
                if (File.Exists(saveFilePath))
                {
                    File.Delete(saveFilePath); // 删除本地文件
                    Debug.Log($"已删除本地数据文件，路径：{saveFilePath}");
                }
                else
                {
                    Debug.Log($"本地数据文件不存在，无需删除，路径：{saveFilePath}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"删除本地数据文件失败！错误：{ex.Message}");
                return; // 文件删除失败时，不继续重置内存（避免数据不一致）
            }

            // 2. 第二步：重置内存中的 gameData 为初始状态（与 Init 方法的默认值保持一致）

            gameData = new GameData();

        gameData.quests = new List<Quest>();
        gameData.currentLevelIndex = 1;       // 初始关卡
        gameData.highestLevelIndex = 1;       // 初始最高关卡
            gameData.coin = 0;                    // 初始金币（可根据项目默认值调整）
            gameData.life = 5;                    // 初始生命（可根据项目默认值调整）
            gameData.disableShowNumberHit = false;// 显示数字命中（默认开启）
            gameData.disableMusic = false;        // 音乐（默认开启）
            gameData.disableSound = false;        // 音效（默认开启）
            gameData.disableVibrate = false;      // 震动（默认开启）
           // gameData.quests = null;               // 任务列表清空
           // gameData.boosterData = null;          // 道具列表清空
            gameData.themeIndex = 0;              // 初始主题索引
            gameData.themeBGIndex = 1;            // 初始背景索引
            gameData.selectedTheme = false;       // 主题未选中
            gameData.lastIntertialLevel = 0;      // 插屏广告关卡记录重置
            gameData.lastIntertialTime = System.DateTime.Now.Ticks; // 插屏广告时间重置
            gameData.lastRewardedTime = System.DateTime.Now.Ticks;  // 激励广告时间重置
            //gameData.gameMode = GameMode.Solitaire; // 默认游戏模式
            Save(gameData);
            Debug.Log("内存数据已重置为初始状态");
        }

        //保存档案
        public void SaveData()
        {
            Save(gameData);
        }

    // 任务列表

    public List<Quest> GetQuest()
    {
        if (gameData == null) return null;
        return gameData.quests;
    }

    //// 保存任务列表
    public void SetQuest(List<Quest> quests)
    {
        gameData.quests = quests;
        SaveData();
    }
    // 保存道具
    //public void SetBoosterData(List<BoosterData> data)
    //{
    //    gameData.boosterData = data;
    //    SaveData();
    //}

    //public List<BoosterData> GetBoosterData()
    //{
    //    if (gameData == null) return null;
    //    return gameData.boosterData;
    //}

    public int GetCurrentLevelIndex()
        {
            if (gameData == null) return 1;
            return gameData.currentLevelIndex;
        }

        //public void AddBoosterCountByType(BoostType boostType, int count)
        //{
        //    BoosterData boosterData = gameData.boosterData.Find(i => i.type == boostType);
        //    if (boosterData != null)
        //    {
        //        boosterData.count += count;
        //        SaveData();
        //    }
        //}

        //public void PassLevel(int index)
        //{
        //    if (index + 1 > gameData.highestLevelIndex)
        //    {
        //        gameData.highestLevelIndex = index + 1;
        //        SaveData();
        //        App.Instance.Model.CurrentMaxLevel += 1;
        //        App.Instance.GetSystem<LevelChestSystem>().Model.ProgressInt += 1;
        //    }
        //}

        //public void PassLevel2(int index)
        //{
        //    gameData.highestLevelIndex += index;
        //    SaveData();
        //    App.Instance.Model.CurrentMaxLevel += index;
        //    App.Instance.GetSystem<LevelChestSystem>().Model.ProgressInt += index;
        //}

        private static void Save(object gameState, string fileName = "gamesave.dat")
        {
            var serializedData = JsonUtility.ToJson(gameState);
            var filePath = Application.persistentDataPath + "/" + fileName;

            StreamWriter streamWriter = new StreamWriter(filePath);

            // 将Json格式的字符串写入
            streamWriter.Write(serializedData);
            // 关闭文件读写流
            streamWriter.Close();
        }

        private static T Load<T>(string fileName = "gamesave.dat")
        {
            var filePath = Application.persistentDataPath + "/" + fileName;

            try
            {
                StreamReader streamReader = new StreamReader(filePath);
                // 读取Json格式字符串
                string jsonStr = streamReader.ReadToEnd();
                // 关闭文件读写流
                streamReader.Close();
                return JsonUtility.FromJson<T>(jsonStr);
            }
            catch (System.IO.FileNotFoundException)
            {
                return default;
            }
        }
    }
