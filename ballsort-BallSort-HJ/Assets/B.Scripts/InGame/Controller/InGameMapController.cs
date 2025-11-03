using _02.Scripts.Config;
using _02.Scripts.LevelEdit;
using _02.Scripts.Util;
using Fangtang;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using _02.Scripts.InGame.UI;
namespace _02.Scripts.InGame.Controller
{
    public class InGameMapController : ElementBehavior<global::InGame>
    {
        private void OnEnable()
        {
            EventDispatcher.instance.Regist(AppEventType.PlayerPipeSkinChange, RefreshSKin);
        }

        private void OnDisable()
        {
            EventDispatcher.instance.UnRegist(AppEventType.PlayerPipeSkinChange, RefreshSKin);
        }

        private void SetLevelType()
        {
            Game.Instance.LevelModel.CopiesType = CopiesType.Thread;
            Game.Instance.LevelModel.SetEnterLevelID(Game.Instance.LevelModel.EnterLevelID);
        }

        private void SetLevelData()
        {
            SetLevelDataByConfig(LevelConfig.Instance);
            Context.GetModel<InGameModel>().LevelData.SetRandomCoin();
            CleanAllPipe();
            InitAllPipe();
            Game.Instance.LevelModel.TypeNumber = Context.GetBallsType();
            Debug.Log($"[InGameMapController] 当前关卡球种类数：{Game.Instance.LevelModel.TypeNumber}");
            Context.GetView<InGamePlayingUI>().SetBarNumberTo(Game.Instance.LevelModel.TheSmallLevelNumbers);
           
            //  Context.GetView<InGamePlayingUI>().SetBarToZero();
        }

        private void SetLevelDataByConfig(LevelConfig config)
        {
            var filteredLevels = FilterLevelsByTargetTag(config);

            var firstLevel = filteredLevels[Game.Instance.LevelModel.TheSmallLevelID];
            Game.Instance.LevelModel.EnterLevelID = firstLevel.levelId;
            Context.GetModel<InGameModel>().LevelData = firstLevel;
            Game.Instance.LevelModel.TheSmallLevelNumbers= filteredLevels.Count;
            Debug.Log("小关卡数量为"+ Game.Instance.LevelModel.TheSmallLevelNumbers);
            Debug.Log($"[SetLevelDataByConfig] 直接加载筛选列表中的第一个关卡，levelTag={Game.Instance.LevelModel.PassLevelTemp}、ID={firstLevel.levelId}");
        }

        private void SetLevelWhenNotF(LevelConfig config)
        {
            var newId = Random.Range(1, config.All.Count);
            var newLevelData = config.GetConfigByID(newId);
            Context.GetModel<InGameModel>().LevelData = newLevelData;
            Game.Instance.LevelModel.EnterLevelID = newId;
        }

        public void StartGame()
        {
            SetLevelType();
            SetLevelData();
        }

        #region PipeSpawn&Destory
        // 【新增1：添加第4个spawnRectTransform的引用，需在Inspector面板赋值】
        [Header("新增：第4行管子的生成容器")]
        public Transform spawnRectTransform4;

        private void InitAllPipe()
        {
            var model = Context.GetModel<InGameModel>();
            var prefab = GameStage.Instance.cellPrefab;
            var pos1Layout = GameStage.Instance.spawnRectTransform;
            var pos2Layout = GameStage.Instance.spawnRectTransform2;
            var pos3Layout = GameStage.Instance.spawnRectTransform3;
            // 【新增2：获取第4个spawn的RectTransform】
            var pos4Layout = GameStage.Instance.spawnRectTransform4;

            var pos1 = pos1Layout.GetComponent<RectTransform>();
            var pos2 = pos2Layout.GetComponent<RectTransform>();
            var pos3 = pos3Layout.GetComponent<RectTransform>();
            // 【新增3：初始化第4个spawn的RectTransform】
            var pos4 = pos4Layout.GetComponent<RectTransform>();

            var index = 1;
            var totalPipeCount = model.LevelData.pipeDataList.Count;
            const int LINE_MAX_COUNT = 4;
            // 【新增4：定义第4行的索引范围（每行4个，第4行对应13-16）】
            const int LINE4_MIN_INDEX = LINE_MAX_COUNT * 3 + 1; // 13
            const int LINE4_MAX_INDEX = LINE_MAX_COUNT * 4;     // 16

            model.InactivePipeDataList.Clear();
            model.FreezePipeDataList.Clear();
            foreach (var pipeData in model.LevelData.pipeDataList)
            {
                if (pipeData.isactive == IsActive.Istrue)
                {
                    RectTransform spawnPos = pos1;
                    // 【原有逻辑保留：第1-3行的位置分配】
                    if (index > LINE_MAX_COUNT && index <= LINE_MAX_COUNT * 2)
                    {
                        spawnPos = pos2;
                    }
                    else if (index > LINE_MAX_COUNT * 2 && index <= LINE_MAX_COUNT * 3)
                    {
                        spawnPos = pos3;
                    }
                    // 【新增5：第4行的位置分配（13-16）】
                    else if (index >= LINE4_MIN_INDEX && index <= LINE4_MAX_INDEX)
                    {
                        spawnPos = pos4;
                    }

                    var spawnObj = Instantiate(prefab, spawnPos);
                    Context.Views.Add(spawnObj);
                    spawnObj.InitPipe(pipeData);
                    spawnObj.name = $"Pipe_Active_{index}";
                    model.LevelPipeList.Add(spawnObj);
                    model.temp = model.LevelPipeList.Count;
                    index++;
                }
                else
                {
                    model.InactivePipeDataList.Add(pipeData);
                    Debug.Log($"Inactive pipe added to list, current inactive count: {model.InactivePipeDataList.Count}");
                }
            }
            foreach (var pipeData in model.LevelData.pipeDataList)
            {
                if (pipeData.freezetype != FreezeType.None)
                {
                    model.FreezePipeDataList.Add(pipeData);
                }
            }

            // 【原有逻辑保留：补齐第1-3行管子】
            int currentFirstLineCount = model.LevelPipeList.Count(pipe => pipe.transform.parent == pos1);
            if (currentFirstLineCount < LINE_MAX_COUNT)
            {
                int needAdd = LINE_MAX_COUNT - currentFirstLineCount;
                for (int i = 0; i < needAdd; i++)
                {
                    var newPipeData = new PipeData(PipeCapacity.Capacity4)
                    {
                        exclusiveType = Typeexclusive.None,
                        isactive = IsActive.Istrue,
                        isneedad = IsNeedAD.NoNeedAd
                    };

                    var spawnObj = Instantiate(prefab, pos1);
                    Context.Views.Add(spawnObj);
                    spawnObj.InitPipe(newPipeData);
                    spawnObj.name = $"Pipe_Fill_First_{index}";
                    spawnObj.ChangeClickObj();
                    model.LevelPipeList.Add(spawnObj);
                    model.temp = model.LevelPipeList.Count;
                    index++;
                    Debug.Log($"补齐第一行管子：{spawnObj.name}");
                }
            }

            int currentSecondLineCount = model.LevelPipeList.Count(pipe => pipe.transform.parent == pos2);
            if (currentSecondLineCount < LINE_MAX_COUNT)
            {
                int needAdd = LINE_MAX_COUNT - currentSecondLineCount;
                for (int i = 0; i < needAdd; i++)
                {
                    var newPipeData = new PipeData(PipeCapacity.Capacity4)
                    {
                        exclusiveType = Typeexclusive.None,
                        isactive = IsActive.Istrue,
                        isneedad = IsNeedAD.NoNeedAd
                    };

                    var spawnObj = Instantiate(prefab, pos2);
                    Context.Views.Add(spawnObj);
                    spawnObj.InitPipe(newPipeData);
                    spawnObj.name = $"Pipe_Fill_Second_{index}";
                    spawnObj.ChangeClickObj();
                    model.LevelPipeList.Add(spawnObj);
                    model.temp = model.LevelPipeList.Count;
                    index++;
                    Debug.Log($"补齐第二行管子：{spawnObj.name}");
                }
            }

            int currentThirdLineCount = model.LevelPipeList.Count(pipe => pipe.transform.parent == pos3);
            if (currentThirdLineCount < LINE_MAX_COUNT)
            {
                int needAdd = LINE_MAX_COUNT - currentThirdLineCount;
                for (int i = 0; i < needAdd; i++)
                {
                    var newPipeData = new PipeData(PipeCapacity.Capacity4)
                    {
                        exclusiveType = Typeexclusive.None,
                        isactive = IsActive.Istrue,
                        isneedad = IsNeedAD.NoNeedAd
                    };

                    var spawnObj = Instantiate(prefab, pos3);
                    Context.Views.Add(spawnObj);
                    spawnObj.InitPipe(newPipeData);
                    spawnObj.name = $"Pipe_Fill_Third_{index}";
                    spawnObj.ChangeClickObj();
                    model.LevelPipeList.Add(spawnObj);
                    model.temp = model.LevelPipeList.Count;
                    index++;
                    Debug.Log($"补齐第三行管子：{spawnObj.name}");
                }
            }

            // 【新增6：补齐第4行管子（逻辑与前3行一致）】
            int currentFourthLineCount = model.LevelPipeList.Count(pipe => pipe.transform.parent == pos4);
            if (currentFourthLineCount < LINE_MAX_COUNT)
            {
                int needAdd = LINE_MAX_COUNT - currentFourthLineCount;
                for (int i = 0; i < needAdd; i++)
                {
                    var newPipeData = new PipeData(PipeCapacity.Capacity4)
                    {
                        exclusiveType = Typeexclusive.None,
                        isactive = IsActive.Istrue,
                        isneedad = IsNeedAD.NoNeedAd
                    };

                    var spawnObj = Instantiate(prefab, pos4);
                    Context.Views.Add(spawnObj);
                    spawnObj.InitPipe(newPipeData);
                    spawnObj.name = $"Pipe_Fill_Fourth_{index}";
                    spawnObj.ChangeClickObj();
                    model.LevelPipeList.Add(spawnObj);
                    model.temp = model.LevelPipeList.Count;
                    index++;
                    Debug.Log($"补齐第四行管子：{spawnObj.name}");
                }
                model.LevelPipeListTemp = model.LevelPipeList;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GameStage.Instance.spawnPanel.transform);
            // 【新增7：补齐第4行后也需重建布局】
            LayoutRebuilder.ForceRebuildLayoutImmediate(pos4);
        }


        /// <summary>
        /// 按目标levelTag筛选关卡列表
        /// </summary>
        /// <param name="config">关卡配置文件</param>
        /// <returns>筛选后的关卡列表（空列表表示无匹配）</returns>
        private List<LevelData> FilterLevelsByTargetTag(LevelConfig config)
        {
            if (config == null || config.All == null || config.All.Count == 0)
            {
                Debug.LogError("[FilterLevelsByTargetTag] 关卡配置为空，无法筛选！");
                return new List<LevelData>();
            }

            var filteredLevels = config.All
                .Where(levelData => levelData != null && levelData.levelTag == Game.Instance.LevelModel.PassLevelTemp)
                .ToList();

            Context.GetModel<InGameModel>().TheSmallLevelNumber = filteredLevels.Count;

            if (filteredLevels.Count == 0)
            {
                Debug.LogWarning($"[FilterLevelsByTargetTag] 未找到levelTag={2}的关卡，请检查关卡配置！");
            }
            else
            {
                Debug.Log($"[FilterLevelsByTargetTag] 成功筛选出{filteredLevels.Count}个levelTag={Game.Instance.LevelModel.PassLevelNumber.Value}的关卡");
            }

            return filteredLevels;
        }



        public void ActivateInactivePipe(int inactiveIndex, Action callBack = null)
        {
            var model = Context.GetModel<InGameModel>();
            if (inactiveIndex < 0 || inactiveIndex >= model.InactivePipeDataList.Count)
            {
                Debug.LogError($"Activate failed: Inactive pipe index {inactiveIndex} out of range");
                callBack?.Invoke();
                return;
            }

            var prefab = GameStage.Instance.cellPrefab;
            var pos1Layout = GameStage.Instance.spawnRectTransform;
            var pos2Layout = GameStage.Instance.spawnRectTransform2;
            var pos3Layout = GameStage.Instance.spawnRectTransform3;
            // 【新增8：激活管子时也需引用第4个spawn】
            var pos4Layout = GameStage.Instance.spawnRectTransform4;

            var pos1 = pos1Layout.GetComponent<RectTransform>();
            var pos2 = pos2Layout.GetComponent<RectTransform>();
            var pos3 = pos3Layout.GetComponent<RectTransform>();
            // 【新增9：初始化第4个spawn的RectTransform】
            var pos4 = pos4Layout.GetComponent<RectTransform>();

            var totalActiveCount = model.LevelPipeList.Count;
            const int LINE_MAX_COUNT = 4;
            // 【新增10：第4行索引范围】
            const int LINE4_MIN_INDEX = LINE_MAX_COUNT * 3 + 1;
            const int LINE4_MAX_INDEX = LINE_MAX_COUNT * 4;

            var pipeData = model.InactivePipeDataList[inactiveIndex];
            RectTransform spawnPos = pos1;
            int nextActiveIndex = totalActiveCount + 1;
            // 【原有逻辑保留：第1-3行位置分配】
            if (nextActiveIndex > LINE_MAX_COUNT && nextActiveIndex <= LINE_MAX_COUNT * 2)
            {
                spawnPos = pos2;
            }
            else if (nextActiveIndex > LINE_MAX_COUNT * 2 && nextActiveIndex <= LINE_MAX_COUNT * 3)
            {
                spawnPos = pos3;
            }
            // 【新增11：第4行位置分配】
            else if (nextActiveIndex >= LINE4_MIN_INDEX && nextActiveIndex <= LINE4_MAX_INDEX)
            {
                spawnPos = pos4;
            }

            var spawnObj = Instantiate(prefab, spawnPos);
            Context.Views.Add(spawnObj);
            spawnObj.InitPipe(pipeData);
            spawnObj.name = $"Pipe_Activated_{nextActiveIndex}";
            model.LevelPipeList.Add(spawnObj);
            model.InactivePipeDataList.RemoveAt(inactiveIndex);
            model.temp = model.LevelPipeList.Count;

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GameStage.Instance.spawnPanel.transform);
            // 【新增12：激活第4行管子后重建布局】
            LayoutRebuilder.ForceRebuildLayoutImmediate(pos4);
            Debug.Log($"Pipe activated successfully, current active count: {model.LevelPipeList.Count}");
            callBack?.Invoke();
        }

        public void AddNewPipe(Action callBack)
        {
            var matchController = Context.GetController<InGameMatchController>();
            if (matchController == null || (!matchController.CanUseTool() && matchController._isDropAnime))
            {
                Debug.LogWarning("AddNewPipe: 操作时机不合法，跳过解锁");
                callBack?.Invoke();
                return;
            }

            var model = Context.GetModel<InGameModel>();
            if (model == null || model.LevelPipeList == null || model.LevelPipeList.Count == 0)
            {
                Debug.LogError("AddNewPipe: 无激活的管子可解锁");
                callBack?.Invoke();
                return;
            }

            bool unlockedFirstSuccess = false;
            foreach (var activePipe in model.LevelPipeList)
            {
                if (activePipe == null || activePipe._pipeData == null)
                {
                    Debug.LogWarning("AddNewPipe: 发现空管子实例或空数据，跳过");
                    continue;
                }

                if (activePipe.isAddPipe == true)
                {
                    activePipe.ChangeAdState();
                    unlockedFirstSuccess = true;
                    break;
                }
            }

            if (!unlockedFirstSuccess)
            {
                Debug.Log("AddNewPipe: 未找到任何“需广告”的管子");
            }

            callBack?.Invoke();
        }

        private void CleanAllPipe()
        {
            var model = Context.GetModel<InGameModel>();
            foreach (var activePipe in model.LevelPipeList)
            {
                if (activePipe != null)
                {
                    DestroyImmediate(activePipe.gameObject);
                }
            }

            model.LevelPipeList.Clear();
            model.InactivePipeDataList.Clear();
            model.ResetInactiveIndex();
            model.temp = 0;
            Context.GetController<InGameMatchController>().CleanAllData();
        }
        #endregion PipeSpawn&Destory

        #region sizeController
        private void RefreshSKin(object[] objs)
        {
            SetSizeFitter();
        }

        private void SetSizeFitter()
        {
            var model = Context.GetModel<InGameModel>();
            var pos1Layout = GameStage.Instance.spawnRectTransform;
            var pos2Layout = GameStage.Instance.spawnRectTransform2;
            var pos3Layout = GameStage.Instance.spawnRectTransform3;
            // 【新增13：尺寸适配时包含第4个spawn】
            var pos4Layout = GameStage.Instance.spawnRectTransform4;

            var spawn1Rect = pos1Layout.GetComponent<RectTransform>();
            var spawn2Rect = pos2Layout.GetComponent<RectTransform>();
            var spawn3Rect = pos3Layout.GetComponent<RectTransform>();
            // 【新增14：初始化第4个spawn的RectTransform】
            var spawn4Rect = pos4Layout.GetComponent<RectTransform>();

            var pipeCapacity = model.LevelPipeList.Count > 0
                ? model.LevelPipeList[0]._pipeData.pipeCapacity
                : PipeCapacity.Capacity3;
            var totalPipeCount = model.LevelData.pipeDataList.Count;
            var pipeConfig = UtilClass.GetSizeFitter((PipeNumber)totalPipeCount, pipeCapacity);

            float spacing = pipeConfig.pipeWSpace - pipeConfig.pipeW;
            InGameManager.Instance.root.localScale = Vector3.one;

            var pipeTotalHeight = InGameManager.Instance.pipeSizeConfig.GetTotalHigh(pipeCapacity);
            // 【原有逻辑保留：第1-3行高度设置】
            spawn1Rect.SetSizeDeltaY(pipeTotalHeight);
            spawn2Rect.SetSizeDeltaY(totalPipeCount > 4 ? pipeTotalHeight : 0);
            spawn3Rect.SetSizeDeltaY(totalPipeCount > 8 ? pipeTotalHeight : 0);
            // 【新增15：第4行高度设置（总管子数>12时显示）】
            spawn4Rect.SetSizeDeltaY(totalPipeCount > 12 ? pipeTotalHeight : 0);

            if (totalPipeCount >= 4)
            {
                InGameManager.Instance.root.localScale *=
                    pipeCapacity == PipeCapacity.Capacity5 ? 0.9f : InGameManager.Instance.scale;
                ResetSpace();
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GameStage.Instance.spawnPanel.transform);
            // 【新增16：第4行尺寸适配后重建布局】
            LayoutRebuilder.ForceRebuildLayoutImmediate(spawn4Rect);
        }

        private void ResetSpace()
        {
            var model = Context.GetModel<InGameModel>();
            var pos1Layout = GameStage.Instance.spawnRectTransform;
            var pos2Layout = GameStage.Instance.spawnRectTransform2;
            var pos3Layout = GameStage.Instance.spawnRectTransform3;

            // 【新增17：间距调整时包含第4个spawn】
            var pos4Layout = GameStage.Instance.spawnRectTransform4;

            var spacing = pos1Layout.spacing;
            var totalPipeCount = model.LevelData.pipeDataList.Count;
            const int LINE_MAX_COUNT = 4;

            var pipeWidth = InGameManager.Instance.pipeSizeConfig.GetWidth();
            var currentTotalWidth = pipeWidth * LINE_MAX_COUNT + spacing * (LINE_MAX_COUNT - 1);
            currentTotalWidth *= InGameManager.Instance.root.localScale.x;
            var screenAvailableWidth = 1080 - 20;

            if (currentTotalWidth - screenAvailableWidth >= 10)
            {
                var overflowWidth = currentTotalWidth - screenAvailableWidth;
                var spacingReduce = overflowWidth / LINE_MAX_COUNT;
                // 【原有逻辑保留：第1-3行间距调整】
                pos1Layout.spacing = spacing - spacingReduce;
                pos2Layout.spacing = spacing - spacingReduce;
                pos3Layout.spacing = spacing - spacingReduce;
                // 【新增18：第4行间距同步调整】
                pos4Layout.spacing = spacing - spacingReduce;
            }
            else if (screenAvailableWidth - currentTotalWidth >= 10)
            {
                var missingWidth = screenAvailableWidth - currentTotalWidth;
                var spacingIncrease = missingWidth / LINE_MAX_COUNT;
                // 【原有逻辑保留：第1-3行间距调整】
                pos1Layout.spacing = spacing + spacingIncrease;
                pos2Layout.spacing = spacing + spacingIncrease;
                pos3Layout.spacing = spacing + spacingIncrease;
                // 【新增19：第4行间距同步调整】
                pos4Layout.spacing = spacing + spacingIncrease;
            }
        }
        #endregion sizeController
    }
}