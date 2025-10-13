using Fangtang;
using System; // 解决 Enum 未定义
using System.Collections.Generic;
using System.Linq;
using _02.Scripts.InGame.UI;
using _02.Scripts.LevelEdit;
using UnityEngine; // 必须加，否则无法访问 Unity API

public class InGameModel : ElementModel
{
    public LevelData LevelData;

    public List<InGamePipeUI> LevelPipeList;
    public List<PipeData> InactivePipeDataList = new List<PipeData>();
    public List<PipeData> FreezePipeDataList = new List<PipeData>();

    public int temp = 0;

    // 未激活管子索引（原有逻辑保留）
    private int _currentInactiveIndex = 0;

    public InGameModel()
    {
        LevelPipeList = new List<InGamePipeUI>();
    }

    /// <summary>
    /// 顺序获取未激活管子数据（原有逻辑保留）
    /// </summary>
    public PipeData GetNextInactivePipeData()
    {
        if (InactivePipeDataList == null || InactivePipeDataList.Count == 0)
        {
            Debug.LogWarning("[InGameModel] 未激活管子列表为空，无法获取元素");
            return null;
        }

        if (_currentInactiveIndex >= InactivePipeDataList.Count)
        {
            Debug.LogWarning("[InGameModel] 未激活管子已全部获取，无更多元素");
            return null;
        }

        PipeData targetPipe = InactivePipeDataList[_currentInactiveIndex];
        _currentInactiveIndex++;
        Debug.Log($"[InGameModel] 成功获取未激活管子，索引：{_currentInactiveIndex - 1}，容量：{targetPipe.pipeCapacity}");
        return targetPipe;
    }

    /// <summary>
    /// 重置未激活管子索引（原有逻辑保留）
    /// </summary>
    public void ResetInactiveIndex()
    {
        _currentInactiveIndex = 0;
        Debug.Log("[InGameModel] 未激活管子获取索引已重置");
    }

    /// <summary>
    /// 核心功能：空管时用未激活数据生成新球（完全适配 InGamePipeUI）
    /// </summary>
    /// <param name="emptyPipe">空管子</param>
    /// <returns>是否填充成功</returns>
    public bool TryFillEmptyPipe(InGamePipeUI emptyPipe)
    {

        if (emptyPipe == null)
        {
            Debug.LogError("[InGameModel] 传入的管子为空，无法填充新球");
            return false;
        }
        if (emptyPipe.ballPrefab == null)
        {
            Debug.LogError("[InGameModel] InGamePipeUI 的 ballPrefab 未赋值，请检查序列化字段");
            return false;
        }

        if (emptyPipe._pipeData == null || emptyPipe._pipeData.canfull != CanFull.Notfull)
        {
            Debug.LogWarning($"[InGameModel] 管子 {emptyPipe.name} 不满足“不可装满”条件，无需填充");
            return false;
        }
        emptyPipe.FullNumber();

        PipeData newPipeData = GetNextInactivePipeData();
        if (newPipeData == null)
        {
            Debug.LogWarning("[InGameModel] 无可用未激活管子数据，放弃填充空管");
            return false;
        }

        if (newPipeData.ballDataStack == null || newPipeData.ballDataStack.Count == 0)
        {
            Debug.LogError($"[InGameModel] 未激活管子数据（容量：{newPipeData.pipeCapacity}）的 ballDataStack 为空，无球可填充");
            return false;
        }


        ClearPipeAllBall(emptyPipe);


        List<BallData> targetBallDatas = newPipeData.ballDataStack.ToList();
        targetBallDatas.Reverse();
        for (int i = 0; i < targetBallDatas.Count; i++)
        {
            BallData currentBallData = targetBallDatas[i];
            if (currentBallData == null)
            {
                Debug.LogError($"[InGameModel] 未激活管子的第 {i + 1} 个球数据为空，跳过生成");
                continue;
            }


            GameObject ballObj = UnityEngine.Object.Instantiate(
                emptyPipe.ballPrefab.gameObject,
                emptyPipe.ballVerticalLayout.transform
            );
            if (ballObj == null)
            {
                Debug.LogError($"[InGameModel] 实例化第 {i + 1} 个球失败，跳过");
                continue;
            }


            InGameBallUI newBall = ballObj.GetComponent<InGameBallUI>();
            if (newBall == null)
            {
                UnityEngine.Object.Destroy(ballObj);
                Debug.LogError($"[InGameModel] 第 {i + 1} 个球预制体缺少 InGameBallUI 组件，跳过");
                continue;
            }

            newBall.InitBall(currentBallData);
            newBall.name = $"PipeBall_{newPipeData.GetHashCode()}_{i + 1}_{currentBallData.type}";
            emptyPipe.PushBall(newBall);

            emptyPipe.GetAndInitPushToPos(newBall, currentBallData);
        }


        emptyPipe._pipeData = newPipeData;
        emptyPipe.SetPipeSprite();
        emptyPipe.CheckTop();
        Debug.Log($"[InGameModel] 空管填充完成！管子：{emptyPipe.name}，原管子状态：不可装满，新管子容量：{newPipeData.pipeCapacity}，填充球数量：{targetBallDatas.Count}，球类型列表：{string.Join(",", targetBallDatas.Select(b => b.type.ToString()))}");
        return true;
    }

    public void UnFreezePipe(BallType balltype)
    {
        foreach (var tempPipe in LevelPipeList)
        {
            Debug.Log("数据1为" + (int)tempPipe._pipeData.freezetype + "数据2为" + (int)balltype);
            if ((int)tempPipe._pipeData.freezetype == (int)balltype)
            {
                tempPipe.UnFreeze();
            }
        }
    }


    /// <summary>
    /// 辅助方法：清空管子所有球（用 InGamePipeUI 的 PopBall 方法）
    /// </summary>
    private void ClearPipeAllBall(InGamePipeUI pipe)
    {
        if (pipe.BallLevelEdits == null) return;

        // 循环弹出所有球并销毁（普通类必须用 UnityEngine.Object.Destroy）
        while (pipe.BallLevelEdits.Count > 0)
        {
            InGameBallUI ballToRemove = pipe.PopBall();
            if (ballToRemove != null && ballToRemove.gameObject != null)
            {
                UnityEngine.Object.Destroy(ballToRemove.gameObject);
            }
        }

    }

    /// <summary>
    /// 辅助方法：根据管子类型生成对应球数据（解决 Random 歧义）
    /// </summary>
    private BallData GenerateBallDataByPipeType(PipeData pipeData)
    {
        // 专属类型管子：生成对应类型的球
        if (pipeData.exclusiveType != Typeexclusive.None)
        {
            BallType matchType = BallType.ID1;
            switch (pipeData.exclusiveType)
            {
                case Typeexclusive.Number2:
                    matchType = BallType.ID2;
                    break;
                // 可扩展其他专属类型映射
                default:
                    matchType = (BallType)Enum.Parse(typeof(BallType), pipeData.exclusiveType.ToString());
                    break;
            }
            return new BallData(matchType);
        }
        // 普通管子：随机生成不重复类型的球（用 UnityEngine.Random，解决歧义）
        else
        {
            List<BallType> usedTypes = new List<BallType>();
            int maxType = Enum.GetValues(typeof(BallType)).Length - 1; // 排除 None 类型

            BallType randomType;
            do
            {
                // 明确指定 UnityEngine.Random，解决 System.Random 和 Unity.Random 的歧义
                randomType = (BallType)UnityEngine.Random.Range(1, maxType + 1);
            } while (usedTypes.Contains(randomType) && usedTypes.Count < maxType);

            usedTypes.Add(randomType);
            return new BallData(randomType);
        }
    }

    // 原有方法保留（修复空引用风险）
    public bool IsUseAddPipeTool()
    {
        if (LevelData == null || LevelPipeList == null)
        {
            Debug.LogWarning("[InGameModel] LevelData 或 LevelPipeList 未初始化，无法判断是否使用加管子工具");
            return false;
        }
        return LevelPipeList.Count > (int)LevelData.GetPipeCount();
    }

    public bool CanAddPipe()
    {
        // 1. 原有空值/空列表检查：避免LevelPipeList未初始化导致崩溃
        if (LevelPipeList == null || LevelPipeList.Count == 0)
        {
            Debug.LogWarning("[InGameModel] LevelPipeList 未初始化或为空，无法判断是否存在需广告的管子");
            return false;
        }

        // 2. 核心逻辑：遍历所有激活管子，检查是否存在“需广告”的管子
        // 注：需加空值保护（pipe或pipe._pipeData为null时跳过，避免NullReferenceException）
        bool hasNeedAdPipe = LevelPipeList.Any(pipe =>
            pipe != null &&                  // 管子实例不为null
            pipe._pipeData != null &&        // 管子数据不为null
            pipe.isAddPipe == true // 满足“需广告”条件
        );

        // 3. 返回结果：存在则返回true，否则返回false
        return hasNeedAdPipe;
    }

    public void Dispose()
    {
        LevelPipeList?.Clear();
        InactivePipeDataList?.Clear();
        LevelData = null;
    }
}