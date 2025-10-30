using System.Collections.Generic;
using System.Linq;
using _02.Scripts.InGame.Controller;
using _02.Scripts.LevelEdit;
using _02.Scripts.Util;
using ProjectSpace.BubbleMatch.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Spine.Unity;

namespace _02.Scripts.InGame.UI
{
    public class InGamePipeUI : ElementUI<global::InGame>
    {
        [SerializeField] public VerticalLayoutGroup ballVerticalLayout;
        [SerializeField] public VerticalLayoutGroup emptyVerticalLayout;
        [SerializeField] public VerticalLayoutGroup bodyVerticalLayout;

        [SerializeField] public ContentSizeFitter ballSizeFitter;
        [SerializeField] public ContentSizeFitter emptySizeFitter;
        [SerializeField] public ContentSizeFitter bodySizeFitter;

        [FormerlySerializedAs("pipe")]
        [SerializeField]
        private PipeSizeController pipeController;

        [SerializeField] public InGameBallUI ballPrefab;
        [SerializeField] private Button pipeButton;

        [SerializeField] public RectTransform popToPos;
        [SerializeField] public RectTransform emptyPanel;
        [SerializeField] public RectTransform pipeControllerPanel;

        [SerializeField] private ParticleSystem fullPipeEff;
        [SerializeField] private SkeletonAnimation skeletonGraphic;
        [SerializeField] private SkeletonAnimation skeletonGraphic2;
        [SerializeField] private SkeletonAnimation FreezeskeletonGraphic;
        [SerializeField] private GameObject SpinegameObject;
        [SerializeField] private Image Typeimage;
        [SerializeField] private Image BottomImage;
        [SerializeField] private Image FreezeImage;
        [SerializeField] private Image BodyImage;
        [SerializeField] private SkeletonAnimation BottomskeletonGraphic;
        [SerializeField] private Text NumberTxt;
        [SerializeField] private RectTransform FenJieXian;
        public readonly Common.Stack<InGameBallUI> BallLevelEdits = new Common.Stack<InGameBallUI>();
        public bool isAddPipe;
        public bool isFreezePipe;
        public PipeData _pipeData;
        private bool _isPlayed;
        private bool _isPlayedFreeze = false;
        public bool isJustUse;
        public bool CanClick;
        [SerializeField] private GameObject AdObject;
        [SerializeField] private GameObject ClickObject;
        [SerializeField] private GameObject PanelObjs;
        [SerializeField] private Sprite Sprite1;
        private void OnEnable()
        {
            pipeButton.onClick.AddListener(ClickPipe);
            SpriteManager.Instance.AddPipeData(this);
            EventDispatcher.instance.Regist(AppEventType.PlayerPipeSkinChange, RefreshSKin);
            InitVerticalLayoutSettings();
        }

        private void OnDisable()
        {
            pipeButton.onClick.RemoveListener(ClickPipe);
            SpriteManager.Instance.RemovePipeData(this);
            EventDispatcher.instance.UnRegist(AppEventType.PlayerPipeSkinChange, RefreshSKin);
        }
        private void DisAppearObjs()
        {
            // 1. 将BottomImage的透明度设为0（保留RGB颜色，仅修改alpha通道）
            if (BodyImage != null) // 空引用防护：避免组件未赋值导致报错
            {
                Color bottomImageColor = BodyImage.color;
                bottomImageColor.a = 0f; // alpha设为0，完全透明
                BodyImage.color = bottomImageColor;

                // 原有逻辑：设置点击检测最小透明度阈值（0表示透明时不响应点击，可保留）
                BodyImage.alphaHitTestMinimumThreshold = 0;
            }

            // 2. 将PanelObjs下所有子物体（含自身）的Image透明度设为0
            if (PanelObjs != null) // 空引用防护：避免PanelObjs未赋值导致报错
            {
                // 获取PanelObjs及其所有子物体上的Image组件（includeInactive=true：包含未激活的子物体）
                Image[] panelImages = PanelObjs.GetComponentsInChildren<Image>(includeInactive: true);

                foreach (Image img in panelImages)
                {
                    if (img != null) // 二次防护：避免个别子物体无Image组件导致报错
                    {
                        Color imgColor = img.color;
                        imgColor.a = 0f; // alpha设为0，完全透明
                        img.color = imgColor;

                        // 可选：如果子物体也需要禁用点击检测，可添加这行（按需选择）
                        // img.alphaHitTestMinimumThreshold = 0;
                    }
                }
            }
        }
        private void InitVerticalLayoutSettings()
        {
            void SetLayoutCommon(VerticalLayoutGroup layout, ContentSizeFitter fitter)
            {
                if (layout == null) return;
                layout.childForceExpandWidth = false;
                layout.childForceExpandHeight = false;
                layout.spacing = 0f;
                layout.padding = new RectOffset(0, 0, 0, 0);

                if (fitter == null) return;
                fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fitter.enabled = false;
            }

            SetLayoutCommon(bodyVerticalLayout, bodySizeFitter);
        }

        public int PipeLength()
        {
            return bodyVerticalLayout != null ? bodyVerticalLayout.transform.childCount : 0;
        }

        public void ChangeAdState()
        {
            AdObject.SetActive(false);
            isAddPipe = false;
        }

        public PipeData GetPipeData()
        {
            return _pipeData;
        }

        public void ChangeClickObj()
        {
            ClickObject.SetActive(true);
            CanClick = false;
        }

        public void SetPipeSprite()
        {
            pipeController.SetPipe(_pipeData);
        }

        public bool PdColor(BallData balldata)
        {
            int pipeExclusiveValue = (int)_pipeData.exclusiveType;
            int ballTypeValue = (int)balldata.type;

            if (_pipeData.exclusiveType == Typeexclusive.None || pipeExclusiveValue == ballTypeValue)
                return true;
            else
            {
               
                return false;
            }
             
        }

        public void UndoInit()
        {
            CanClick = true;
            isJustUse = false;
        }

        public void InitPipe(PipeData initPipeData)
        {
            CanClick = true;
            _pipeData = initPipeData;
            isJustUse = false;
            if (initPipeData.freezetype != FreezeType.None)
            {
                FreezeskeletonGraphic.gameObject.SetActive(true);
                isFreezePipe = true;
                FreezeImage.sprite = SpriteManager.Instance.FreezeIcon(initPipeData.freezetype);
                FreezeImage.SetNativeSize();
            }
            else
            {
                isFreezePipe = false;
            }

            if (initPipeData.canfull != CanFull.Canfull)
            {
            }
            else
            {
                BottomImage.gameObject.SetActive(false);
            }


            if (initPipeData.exclusiveType != Typeexclusive.None)
            {
                Typeimage.sprite = SpriteManager.Instance.HeadIcon(initPipeData.exclusiveType);
                Debug.Log("类型" + initPipeData.exclusiveType);
                Typeimage.SetNativeSize();
            }
            else Typeimage.transform.parent.gameObject.SetActive(false);

            AdObject.SetActive(initPipeData.isneedad == IsNeedAD.NeedAd);

            if (initPipeData.pipeCapacity != PipeCapacity.Capacity4)
            {
                BodyImage.sprite = Sprite1;
                BodyImage.SetNativeSize();
              //  NumberTxt.text = $"{(int)initPipeData.pipeCapacity}";
            }
            else
            {
                NumberTxt.gameObject.SetActive(false);
            }
            isAddPipe = initPipeData.isneedad == IsNeedAD.NeedAd;
            pipeController.SetPipe(_pipeData);
            SetPipeSize();

            for (int i = 0; i < _pipeData.ballDataStack.Count; i++)
            {
                var data = _pipeData.ballDataStack.GetDataByIndex(i);
                var obj = Instantiate(ballPrefab, ballVerticalLayout.transform);

                _context.Views.Add(obj);
                obj.InitBall(data);
                obj.name = $"Ball{data.type}_{i + 1}";

                PushBall(obj);
                GetAndInitPushToPos(ballPrefab, data);
            }

            CheckTop();
        }

        public void FullNumber()
        {
            BottomImage.gameObject.SetActive(false);
            BottomskeletonGraphic.gameObject.SetActive(true);
            BottomskeletonGraphic.AnimationState.SetAnimation(0, "animation", false);
        }

        public void UnFreeze()
        {
            if (!_isPlayedFreeze)
            {
                FreezeImage.gameObject.SetActive(false);
                FreezeskeletonGraphic.AnimationState.SetAnimation(0, "animation", false);
                _isPlayedFreeze = true;
                isFreezePipe = false;
            }
        }

        public bool CanAddPipeSize()
        {
            var model = Context.GetModel<InGameModel>();
            if (model == null || model.LevelPipeList == null)
            {
                return false;
            }

            int needAdCount = model.LevelPipeList
                .Count(pipe => pipe != null
                           && pipe._pipeData != null
                           && pipe._pipeData.isneedad == IsNeedAD.NeedAd);

            return needAdCount > 0;
        }

        public void RemoveAllBallsOfType(BallType temp)
        {
            List<InGameBallUI> allBalls = BallLevelEdits.ToList();
            List<InGameBallUI> reservedBalls = new List<InGameBallUI>();
            List<RectTransform> reservedEmptyUI = new List<RectTransform>();

            for (int i = 0; i < allBalls.Count; i++)
            {
                InGameBallUI currentBall = allBalls[i];

                if (currentBall != null && currentBall.GetBallData().type == temp)
                {
                    DestroyImmediate(currentBall.gameObject);
                }
                else
                {
                    if (currentBall != null) reservedBalls.Add(currentBall);
                }
            }

            while (BallLevelEdits.Count > 0)
            {
                BallLevelEdits.Pop();
            }

            foreach (var ball in reservedBalls)
            {
                BallLevelEdits.Push(ball);
                ball.transform.SetParent(ballVerticalLayout.transform, false);
            }

            if (ballVerticalLayout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(ballVerticalLayout.GetComponent<RectTransform>());
            }

            if (emptyVerticalLayout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(emptyVerticalLayout.GetComponent<RectTransform>());
            }
        }

        public void AddPipeSize()
        {
            if (CanAddPipeSize())
            {
                _pipeData.pipeCapacity++;
            }
        }

        public bool CanPushBallLimit(BallData ballData)
        {
            var judgment1 = BallLevelEdits.Count == 0;
            if (judgment1)
            {
                return true;
            }

            var judgment2 = BallLevelEdits.Count < (int)_pipeData.pipeCapacity;
            var judgment3 = ballData.type == BallLevelEdits.Peek().GetBallData().type;

            return judgment2 && judgment3;
        }

        public void PushBall(InGameBallUI ballUI)
        {
            BallLevelEdits.Push(ballUI);

            if (ballVerticalLayout != null)
            {
            }
        }

        public void PushBalls(List<InGameBallUI> ballUIs)
        {
            for (int i = 0; i < ballUIs.Count; i++)
            {
                BallLevelEdits.Push(ballUIs[i]);
            }
        }

        public InGameBallUI PopBall()
        {
            InGameBallUI ballUI = null;

            if (BallLevelEdits.Count > 0)
            {
                ballUI = BallLevelEdits.Pop();
                if (ballVerticalLayout != null)
                {
                }
            }

            return ballUI;
        }

        public List<InGameBallUI> PopBalls()
        {
            List<InGameBallUI> poppedBalls = new List<InGameBallUI>();
            if (BallLevelEdits.Count == 0)
            {
                Debug.LogWarning("[PopBalls] 管子中无球可弹");
                return poppedBalls;
            }

            InGameBallUI topBall = BallLevelEdits.Peek();
            if (topBall == null || topBall._isBlackBall)
            {
                Debug.LogWarning("[PopBalls] 栈顶球为黑色球（_isBlackBall=true），不执行批量弹出");
                return poppedBalls;
            }

            BallType targetType = topBall.GetBallData().type;
            Debug.Log($"[PopBalls] 开始批量弹出类型为 {targetType} 的非黑色球");

            while (BallLevelEdits.Count > 0)
            {
                InGameBallUI currentTopBall = BallLevelEdits.Peek();
                if (currentTopBall == null)
                {
                    Debug.LogWarning("[PopBalls] 栈顶球为空，停止弹出");
                    break;
                }

                bool isSameType = currentTopBall.GetBallData().type == targetType;
                bool isNotBlack = !currentTopBall._isBlackBall;
                if (isSameType && isNotBlack)
                {
                    InGameBallUI poppedBall = BallLevelEdits.Pop();
                    poppedBalls.Add(poppedBall);
                    Debug.Log($"[PopBalls] 弹出球：{poppedBall.name}（类型：{targetType}，是否黑色：{poppedBall._isBlackBall}）");
                    ControllerEmptyList();
                    if (ballVerticalLayout != null)
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(ballVerticalLayout.GetComponent<RectTransform>());
                    }
                }
                else
                {
                    string stopReason = isSameType ? "同类型但为黑色球" : "不同类型球";
                    Debug.Log($"[PopBalls] 遇到{stopReason}（类型：{currentTopBall.GetBallData().type}，是否黑色：{currentTopBall._isBlackBall}），停止弹出");
                    break;
                }
            }

            if (emptyVerticalLayout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(emptyVerticalLayout.GetComponent<RectTransform>());
            }

            Debug.Log($"[PopBalls] 批量弹出完成，共弹出 {poppedBalls.Count} 个非黑色同类型球");
            return poppedBalls;
        }

        private void ClickPipe()
        {
            _context.GetController<InGameMatchController>().ClickPipe(this);
        }
        private void DeleteStepData()
        {
            _context.GetController<InGameMatchController>().ClearAllRevocationData();
        }
        public bool PipeFullOrEmpty()
        {
            if (BallLevelEdits.Count == 0)
            {
                return true;
            }

            var array = BallLevelEdits.ToList();
            var isHaveOther = array.Find(x => x.GetBallData().type != array[0].GetBallData().type) == null;

            return BallLevelEdits.Count == (int)_pipeData.pipeCapacity && isHaveOther;
        }

        public void CheckTop()
        {
            if (BallLevelEdits.Count != 0 && Context.CellMapModel.LevelData.blindBox)
            {
                BallLevelEdits.Peek().SetAlready();
            }
        }

        private bool IsFull()
        {
            return BallLevelEdits.Count == (int)_pipeData.pipeCapacity && BallLevelEdits.Count > 0;
        }

        private bool IsHaveOtherBall()
        {
            var type = BallLevelEdits.Peek().GetBallData().type;
            var ballList = BallLevelEdits.ToList();
            var isHaveOtherBall = ballList.Find(x => x.GetBallData().type != type) != null;
            return isHaveOtherBall;
        }

        public bool IsFullAndOneType()
        {
            return IsFull() && !IsHaveOtherBall() && !_isPlayed &&
                   (!isAddPipe || Context.CellMapModel.LevelData.GetPipeCapacity() == _pipeData.pipeCapacity);
        }

        public void TriggerFullEff()
        {
            if (_pipeData.pipeCapacity != PipeCapacity.Capacity4)
            {
                return;
            }
            if (IsFullAndOneType())
            {
                //  fullPipeEff.Play();
               
                SpinegameObject.SetActive(true);
                skeletonGraphic.AnimationState.SetAnimation(0, "animation", false);
                Context.GetModel<InGameModel>().EndFinishNumber += 1;
                Context.GetView<InGamePlayingUI>().SetBar();
                CanClick = false;
                DeleteStepData();
                if (BallLevelEdits.Count > 0)
                {
                    InGameBallUI topBall = BallLevelEdits.Peek();
                    var tempType = topBall.GetBallData().type;
                    Debug.Log("类型为" + topBall.GetBallData().type);
                    switch (tempType)
                    {
                        case BallType.ID1:
                            Debug.Log("ID1 对应的输出数据：比如「普通红球」或 100 分");
                            EventManager.Emit("Completebowlplating", null);
                            break;

                        case BallType.ID2:
                            Debug.Log("ID2 对应的输出数据：比如「特殊蓝球」或 200 分");
                            Debug.Log("ID2 附加数据：额外获得 1 个道具");
                            break;

                        case BallType.ID3:
                            Debug.Log("ID3 对应的输出数据：比如「稀有紫球」或 300 分");
                            break;

                        case BallType.ID4:
                            Debug.Log("ID4 对应的输出数据：比如「炸弹球」或 400 分");
                            break;

                        case BallType.ID5:
                            Debug.Log("ID5 对应的输出数据：比如「冰冻球」或 500 分");
                            break;

                        case BallType.ID6:
                            Debug.Log("ID6 对应的输出数据：比如「加速球」或 600 分");
                            break;

                        case BallType.ID7:
                            Debug.Log("ID7 对应的输出数据：比如「终极金球」或 1000 分");
                            Debug.Log("ID7 奖励列表：[钻石*5, 金币*1000]");
                            break;

                        default:
                            Debug.Log("未定义的球类型：" + tempType + "，请检查枚举配置");
                            break;
                    }
                }
                DisAppearObjs();
                skeletonGraphic.AnimationState.Complete += OnAnimationComplete;
               // skeletonGraphic2.AnimationState.Complete += OnAnimationComplete2;
                _isPlayed = true;

                foreach (var ball in BallLevelEdits)
                {
                }

                if (IsSpecialModel())
                {
                    SpecialPipeTrigger();
                }
            }
            else
            {
                Debug.Log("返回");
            }
        }

        void OnAnimationComplete(Spine.TrackEntry trackEntry)
        {
            SpinegameObject.SetActive(false);
            skeletonGraphic2.gameObject.SetActive(true);
            skeletonGraphic2.AnimationState.SetAnimation(0, "animation", false);
        }

        void OnAnimationComplete2(Spine.TrackEntry trackEntry)
        {
            skeletonGraphic2.AnimationState.SetAnimation(0, "loops", true);
        }

        private bool IsSpecialModel()
        {
            var ballType = BallLevelEdits.Peek().GetBallData().type;
            return (ballType == BallType.Coin || ballType == BallType.Money);
        }

        private void SpecialPipeTrigger()
        {
        }

        public void ControllerEmptyList()
        {
        }

        public RectTransform GetAndInitPushToPos(InGameBallUI prefab, BallData data)
        {
            var emptypre = Instantiate(prefab, emptyPanel.transform);
            var spawnEmpty = emptypre.gameObject.GetComponent<RectTransform>();
            emptypre.InitBall2(data);
            return spawnEmpty;
        }

        public RectTransform GetLastPushPos()
        {
            int lastIndex = ballVerticalLayout.transform.childCount - 1;
            Transform lastChild;
            if (lastIndex < 0)
            {
                lastChild = ballVerticalLayout.transform;
                Debug.Log("大小" + lastIndex);
            }
            else lastChild = ballVerticalLayout.transform.GetChild(lastIndex);
            Debug.Log("大小" + lastIndex);
            RectTransform lastRect = lastChild.GetComponent<RectTransform>();
            return lastRect;
        }

        [Header("管子大小属性控制")]
        [SerializeField] private RectTransform rootRectTransform;

        private List<RectTransform> pipeControllerEmpty = new List<RectTransform>();

        private void SetPipeSize()
        {
            var h = InGameManager.Instance.pipeSizeConfig.GetTotalHigh(_pipeData.pipeCapacity);
            var w = InGameManager.Instance.pipeSizeConfig.GetWidth();
            var currentAlreadySpawn = pipeControllerEmpty.Count;
          //  rootRectTransform.sizeDelta = new Vector2(w, h);

         

            var pipeConfig = UtilClass.GetSizeFitter(Context.CellMapModel.LevelData.pipeNumber, _pipeData.pipeCapacity);
            pipeController.RefreshSKin();
        }

        private void RefreshSKin(object[] objs)
        {
            SetPipeSize();
        }
    }
}