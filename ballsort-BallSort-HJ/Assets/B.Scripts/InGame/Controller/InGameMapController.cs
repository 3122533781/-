using _02.Scripts.Config;
using _02.Scripts.LevelEdit;
using _02.Scripts.Util;
using Fangtang;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
            SetLevelDataByConfig(LevelConfig.Instance, Game.Instance.LevelModel.EnterLevelID);
            Context.GetModel<InGameModel>().LevelData.SetRandomCoin();
            CleanAllPipe();
            InitAllPipe();
        }

        private void SetLevelDataByConfig(LevelConfig config, int enterLevel)
        {
            if (config == null)
            {
                Debug.LogError("[SetLevelDataByConfig] LevelConfig.Instance 为 null！");
                return;
            }
            var isHave = config.TryGetConfigByID(enterLevel, out var levelData);
            if (isHave)
            {
                Context.GetModel<InGameModel>().LevelData = levelData;
                Debug.Log($"[SetLevelDataByConfig] 成功获取关卡{enterLevel}，管子数量：{levelData.pipeDataList.Count}");
            }
            else
            {
                Debug.LogError($"[SetLevelDataByConfig] 未找到关卡{enterLevel}，进入随机 fallback");
                SetLevelWhenNotF(config);
            }
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
            Debug.Log("触发");
            SetLevelType();
            SetLevelData();
        }

        #region PipeSpawn&Destory
        private void InitAllPipe()
        {
            var model = Context.GetModel<InGameModel>();
            var prefab = GameStage.Instance.cellPrefab;
            var pos1Layout = GameStage.Instance.spawnRectTransform;
            var pos2Layout = GameStage.Instance.spawnRectTransform2;
            var pos3Layout = GameStage.Instance.spawnRectTransform3;
            var pos1 = pos1Layout.GetComponent<RectTransform>();
            var pos2 = pos2Layout.GetComponent<RectTransform>();
            var pos3 = pos3Layout.GetComponent<RectTransform>();
            var index = 1;
            var totalPipeCount = model.LevelData.pipeDataList.Count;
            const int LINE_MAX_COUNT = 5;

            model.InactivePipeDataList.Clear();
            model.FreezePipeDataList.Clear();
            foreach (var pipeData in model.LevelData.pipeDataList)
            {
                if (pipeData.isactive == IsActive.Istrue)
                {
                    RectTransform spawnPos = pos1;
                    if (index > LINE_MAX_COUNT && index <= LINE_MAX_COUNT * 2)
                    {
                        spawnPos = pos2;
                    }
                    else if (index > LINE_MAX_COUNT * 2 && index <= LINE_MAX_COUNT * 3)
                    {
                        spawnPos = pos3;
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

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GameStage.Instance.spawnPanel.transform);
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
            var pos1 = pos1Layout.GetComponent<RectTransform>();
            var pos2 = pos2Layout.GetComponent<RectTransform>();
            var pos3 = pos3Layout.GetComponent<RectTransform>();
            var totalActiveCount = model.LevelPipeList.Count;
            const int LINE_MAX_COUNT = 5;

            var pipeData = model.InactivePipeDataList[inactiveIndex];
            RectTransform spawnPos = pos1;
            int nextActiveIndex = totalActiveCount + 1;
            if (nextActiveIndex > LINE_MAX_COUNT && nextActiveIndex <= LINE_MAX_COUNT * 2)
            {
                spawnPos = pos2;
            }
            else if (nextActiveIndex > LINE_MAX_COUNT * 2 && nextActiveIndex <= LINE_MAX_COUNT * 3)
            {
                spawnPos = pos3;
            }

            var spawnObj = Instantiate(prefab, spawnPos);
            Context.Views.Add(spawnObj);
            spawnObj.InitPipe(pipeData);
            spawnObj.name = $"Pipe_Activated_{nextActiveIndex}";
            model.LevelPipeList.Add(spawnObj);
            model.InactivePipeDataList.RemoveAt(inactiveIndex);
            model.temp = model.LevelPipeList.Count;

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GameStage.Instance.spawnPanel.transform);
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
            var spawn1Rect = pos1Layout.GetComponent<RectTransform>();
            var spawn2Rect = pos2Layout.GetComponent<RectTransform>();
            var spawn3Rect = pos3Layout.GetComponent<RectTransform>();

            var pipeCapacity = model.LevelPipeList.Count > 0
                ? model.LevelPipeList[0]._pipeData.pipeCapacity
                : PipeCapacity.Capacity3;
            var totalPipeCount = model.LevelData.pipeDataList.Count;
            var pipeConfig = UtilClass.GetSizeFitter((PipeNumber)totalPipeCount, pipeCapacity);

            float spacing = pipeConfig.pipeWSpace - pipeConfig.pipeW;
            InGameManager.Instance.root.localScale = Vector3.one;

            var pipeTotalHeight = InGameManager.Instance.pipeSizeConfig.GetTotalHigh(pipeCapacity);
            spawn1Rect.SetSizeDeltaY(pipeTotalHeight);
            spawn2Rect.SetSizeDeltaY(totalPipeCount > 5 ? pipeTotalHeight : 0);
            spawn3Rect.SetSizeDeltaY(totalPipeCount > 10 ? pipeTotalHeight : 0);

            if (totalPipeCount >= 5)
            {
                InGameManager.Instance.root.localScale *=
                    pipeCapacity == PipeCapacity.Capacity5 ? 0.9f : InGameManager.Instance.scale;
                ResetSpace();
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GameStage.Instance.spawnPanel.transform);
        }

        private void ResetSpace()
        {
            var model = Context.GetModel<InGameModel>();
            var pos1Layout = GameStage.Instance.spawnRectTransform;
            var pos2Layout = GameStage.Instance.spawnRectTransform2;
            var pos3Layout = GameStage.Instance.spawnRectTransform3;
            var spacing = pos1Layout.spacing;
            var totalPipeCount = model.LevelData.pipeDataList.Count;
            const int LINE_MAX_COUNT = 5;

            var pipeWidth = InGameManager.Instance.pipeSizeConfig.GetWidth();
            var currentTotalWidth = pipeWidth * LINE_MAX_COUNT + spacing * (LINE_MAX_COUNT - 1);
            currentTotalWidth *= InGameManager.Instance.root.localScale.x;
            var screenAvailableWidth = 1080 - 20;

            if (currentTotalWidth - screenAvailableWidth >= 10)
            {
                var overflowWidth = currentTotalWidth - screenAvailableWidth;
                var spacingReduce = overflowWidth / LINE_MAX_COUNT;
                pos1Layout.spacing = spacing - spacingReduce;
                pos2Layout.spacing = spacing - spacingReduce;
                pos3Layout.spacing = spacing - spacingReduce;
            }
            else if (screenAvailableWidth - currentTotalWidth >= 10)
            {
                var missingWidth = screenAvailableWidth - currentTotalWidth;
                var spacingIncrease = missingWidth / LINE_MAX_COUNT;
                pos1Layout.spacing = spacing + spacingIncrease;
                pos2Layout.spacing = spacing + spacingIncrease;
                pos3Layout.spacing = spacing + spacingIncrease;
            }
        }
        #endregion sizeController
    }
}