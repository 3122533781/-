using _02.Scripts.InGame.UI;
using _02.Scripts.LevelEdit;
using _02.Scripts.Util;
using DG.Tweening;
using Fangtang;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher = _02.Scripts.Util.EventDispatcher;

namespace _02.Scripts.InGame.Controller
{
    public class InGameMatchController : ElementBehavior<global::InGame>
    {
        [SerializeField] public RectTransform empty;
        [SerializeField, Range(0.1f, 2f)] private float dropSpeed = 0.5f;

        private InGameBallUI _popBallUI;
        private List<InGameBallUI> _popBallUIs = new List<InGameBallUI>();
        private InGamePipeUI _popPipeUI; // ԭ�ܵ���������Ĺܵ���
        private InGamePipeUI _pushPipeUI;

        private Stack<StepData> _playerStep = new Stack<StepData>();

        private bool _isPushOnAnime;
        private bool _isPopOnAnime;
        private bool _isCoercion;
        private bool _isStartTwoAnime;
        public bool _isDropAnime;

        public class StepData
        {
            public InGamePipeUI fromPipe;
            public InGamePipeUI toPipe;
            public List<InGameBallUI> balls;
        }

        public void ClickPipe(InGamePipeUI clickPipeUI, bool isRevocation = false)
        {
            Debug.Log("����״̬" + _isStartTwoAnime + " " + _isPushOnAnime + " " + _isPopOnAnime + "���Ƿ񳷻�ģʽ��" + isRevocation);

            if (!isRevocation)
            {
                if (!_isStartTwoAnime && _isPushOnAnime)
                {
                    return;
                }
            }

            if (!clickPipeUI.CanClick)
            {
                if (!isRevocation)
                    FloatingWindow.Instance.Show("�ùܵ�����ʹ��");
                return;
            }
            if (clickPipeUI.isAddPipe)
            {
                if (!isRevocation)
                    FloatingWindow.Instance.Show("���Ƚ���");
                return;
            }
            if (clickPipeUI.isFreezePipe)
            {
                if (!isRevocation)
                    FloatingWindow.Instance.Show("������ɸò;ߵ���ϴ");
                return;
            }

            if (!isRevocation && clickPipeUI.BallLevelEdits.Count > 0 && _popBallUIs.Count > 0)
            {
                if (_popBallUIs[0].GetBallData().type != clickPipeUI.BallLevelEdits.Peek().GetBallData().type && !clickPipeUI.isJustUse)
                {
                    FloatingWindow.Instance.Show("�����ͬ�����͵ĳ���");
                    return;
                }
            }

            if (_popBallUIs != null && _popBallUIs.Count > 0)
            {
                var firstBallData = _popBallUIs[0].GetBallData();
                bool canPush = isRevocation ? true : clickPipeUI.PdColor(firstBallData);

                if (canPush)
                {
                    int maxCapacity = (int)clickPipeUI._pipeData.pipeCapacity;
                    int currentBallCount = clickPipeUI.BallLevelEdits.Count;
                    int remainingSpace = maxCapacity - currentBallCount;
                    int canPutCount = Mathf.Min(remainingSpace, _popBallUIs.Count);

                    if (isRevocation || canPutCount > 0 || clickPipeUI == _popPipeUI || _isCoercion)
                    {
                        _pushPipeUI = clickPipeUI;
                        SetPushIsAnime(true);
                        List<InGameBallUI> ballsToPush = _popBallUIs.GetRange(0, canPutCount);
                        List<InGameBallUI> remainingBalls = _popBallUIs.GetRange(canPutCount, _popBallUIs.Count - canPutCount);

                        PushBall(_popPipeUI, clickPipeUI, ballsToPush, remainingBalls);
                    }
                    else if (!isRevocation)
                    {
                        _isStartTwoAnime = true;
                        _pushPipeUI = clickPipeUI;
                    }
                }
            }
            else
            {
                _popPipeUI = clickPipeUI; // ��¼ԭ�ܵ���������Ĺܵ���
                PopBall(_popPipeUI);
            }
        }

        private void PopBall(InGamePipeUI inGamePipeUI)
        {
            if (inGamePipeUI == null)
            {
                Debug.LogWarning("[PopBall] Ŀ�����Ϊ�գ��޷�������");
                return;
            }
            inGamePipeUI.isJustUse = true;
            var polBall = PopBallAndFly(inGamePipeUI);
            if (polBall != null && polBall.Count > 0)
            {
                _popBallUIs = polBall;
                _popBallUI = polBall[0];
                SetPopIsAnime(true);

                for (int i = 0; i < polBall.Count; i++)
                {
                    if (polBall[i] != null)
                    {
                        polBall[i].SGLight();
                        polBall[i].StopPushAnime();
                    }
                }
            }
            else
            {
                _popBallUIs = new List<InGameBallUI>();
                _popBallUI = null;
                SetPopIsAnime(false);
            }
        }

        public InGameBallUI FlyBall()
        {
            return _popBallUI;
        }

        // �������޸���������ɺ�ʣ���������Ż�ԭ�ܵ���
        private void PushBall(InGamePipeUI popPipeUI, InGamePipeUI targetPipeUI, List<InGameBallUI> ballsToPush, List<InGameBallUI> remainingBalls)
        {
            popPipeUI.isJustUse = false;
            PushBallAndFly(popPipeUI, targetPipeUI, ballsToPush, remainingBalls);

            // 1. ����ʣ���������Ż�ԭ�ܵ�������+UIͬ����
            if (remainingBalls.Count > 0 && popPipeUI != null)
            {
                foreach (var ball in remainingBalls)
                {
                    if (ball != null)
                    {
                        // ���ݲ㣺��ʣ�������¼���ԭ�ܵ������б�
                        popPipeUI.PushBall(ball);
                        // UI�㣺��ʣ����ĸ��ڵ���Ϊԭ�ܵ��Ĳ��ֽڵ�
                        ball.transform.SetParent(popPipeUI.ballVerticalLayout.transform, false);
                        // �ر�ʣ����ĸ���Ч�����ָ�����״̬
                        ball.CloseSG();
                    }
                }
                // �ؽ�ԭ�ܵ����֣�����UI����
                if (popPipeUI.ballVerticalLayout != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(popPipeUI.ballVerticalLayout.GetComponent<RectTransform>());
                }
                // ����ԭ�ܵ��չ���ʾ����������߼���
                popPipeUI.ControllerEmptyList();
            }

            // 2. ��մ������б�����ʣ�������
            _popBallUIs.Clear();
            _popBallUI = null;
            SetPopIsAnime(false);
        }

        public void Win()
        {
            Context.Win();
        }

        public void CleanAllData()
        {
            _popPipeUI = null;
            _pushPipeUI = null;
            _popBallUI = null;
            _popBallUIs = new List<InGameBallUI>();
            _playerStep = new Stack<StepData>();
            _isPushOnAnime = false;
            _isPopOnAnime = false;
            _isCoercion = false;
            _isStartTwoAnime = false;
        }

        public Stack<StepData> GetPlayerStep()
        {
            return _playerStep;
        }

        #region Anime

        private void SetPushIsAnime(bool setAnime)
        {
            _isPushOnAnime = setAnime;
        }

        private void SetIsDropAnime(bool setAnime)
        {
            _isDropAnime = setAnime;
        }

        private void SetPopIsAnime(bool setAnime)
        {
            _isPopOnAnime = setAnime;
        }

        private void PushBallAndFly(InGamePipeUI popPipeUI, InGamePipeUI targetPipeUI, List<InGameBallUI> ballsToPush, List<InGameBallUI> remainingBalls)
        {
            if (ballsToPush == null || ballsToPush.Count == 0)
            {
                Debug.LogWarning("[PushBallAndFly] Ҫ��������б�Ϊ�գ��޷�����");
                SetPushIsAnime(false);
                return;
            }

            SetPushIsAnime(true);

            for (int i = 0; i < ballsToPush.Count; i++)
            {
                int currentIndex = i;
                var currentBall = ballsToPush[currentIndex];

                if (currentBall == null)
                {
                    Debug.LogWarning($"[PushBallAndFly] �� {currentIndex} ����Ϊ�գ�����");
                    if (currentIndex == ballsToPush.Count - 1)
                    {
                        SetPushIsAnime(false);
                    }
                    continue;
                }

                var emptyRectTransform = targetPipeUI.GetAndInitPushToPos(currentBall, currentBall.GetBallData());
                emptyRectTransform.gameObject.GetComponent<Image>().SetAlpha(0);
                targetPipeUI.PushBall(currentBall);

                FlyToTopPos(targetPipeUI, currentBall, () =>
                {
                    currentBall.transform.SetParent(targetPipeUI.ballVerticalLayout.transform, false);
                    currentBall.SetPushAnime(null);
                    currentBall.CloseSG();

                    if (currentIndex == ballsToPush.Count - 1)
                    {
                        targetPipeUI.TriggerFullEff();

                        if (CalculateIsUnFreeze(targetPipeUI, currentBall) == 4)
                        {
                            Context.CellMapModel.UnFreezePipe(currentBall.GetBallData().type);
                        }

                        Context.CheckIsOver();
                        Context.GetView<InGamePlayingUI>().SetBar();
                        AddPlayStep(popPipeUI, targetPipeUI, ballsToPush);
                        _isCoercion = false;
                        _isStartTwoAnime = false;
                        SetPushIsAnime(false);
                        if (popPipeUI != null)
                        {
                            popPipeUI.CheckTop(); // ���Ѵ�Դ���ӽ���Ŀ����ӣ�Դ���Ӹ���ջ��״̬
                        }
                        // ԭ�ܵ��չ�����߼����䣨����ԭ�ܵ�����ʱ������
                        if (popPipeUI != null && popPipeUI.BallLevelEdits.Count == 0)
                        {
                            if (Context != null && Context.CellMapModel != null)
                            {
                                bool fillSuccess = Context.CellMapModel.TryFillEmptyPipe(popPipeUI);
                                if (fillSuccess)
                                {
                                    Debug.Log($"[InGameMatchController] ԭ���ӣ�{popPipeUI.name}��Ϊ�գ����Զ��������");
                                }
                            }
                            else
                            {
                                Debug.LogWarning("[InGameMatchController] Context �� Model δ��ʼ�����޷����չ�");
                            }
                        }
                    }
                });
            }
        }

        private int CalculateIsUnFreeze(InGamePipeUI ingamepipeui, InGameBallUI ingameballui)
        {
            int tempnumber = 0;
            foreach (var tempball in ingamepipeui.BallLevelEdits)
            {
                if (tempball.GetBallData().type == ingameballui.GetBallData().type)
                {
                    Debug.Log("����Ϊ" + tempball.GetType() + "  " + ingameballui.GetType());
                    tempnumber += 1;
                }
            }
            return tempnumber;
        }

        public void ClearRandomColorTool(Action action)
        {
            Debug.Log("״̬" + _isPushOnAnime + " " + _isPopOnAnime + " " + _isDropAnime);
            if (_isPushOnAnime || _isPopOnAnime || _isDropAnime)
            {
                FloatingWindow.Instance.Show("���������У��޷�ʹ�õ��ߣ�");
                return;
            }

            var inGameModel = Context.GetModel<InGameModel>();
            if (inGameModel == null || inGameModel.LevelPipeList.Count == 0)
            {
                FloatingWindow.Instance.Show("û�пɲ����Ĺ��ӣ�");
                return;
            }

            HashSet<BallType> existColors = new HashSet<BallType>();
            foreach (var pipe in inGameModel.LevelPipeList)
            {
                foreach (var ballUI in pipe.BallLevelEdits)
                {
                    if (ballUI != null)
                    {
                        BallType ballColor = ballUI.GetBallData().type;
                        existColors.Add(ballColor);
                    }
                }
            }

            if (existColors.Count == 0)
            {
                FloatingWindow.Instance.Show("����û�����������");
                return;
            }

            List<BallType> colorList = new List<BallType>(existColors);
            int randomIndex = UnityEngine.Random.Range(0, colorList.Count);
            BallType targetColor = colorList[randomIndex];

            foreach (var pipe in inGameModel.LevelPipeList)
            {
                if (pipe != null)
                {
                    pipe.RemoveAllBallsOfType(targetColor);
                }
            }

            Context.CheckIsOver();
            FloatingWindow.Instance.Show($"���������С�{targetColor}����ɫ����");

            action?.Invoke();
        }

        private List<InGameBallUI> PopBallAndFly(InGamePipeUI inGamePipeUI)
        {
            if (inGamePipeUI == null)
            {
                Debug.LogWarning("[PopBallAndFly] Ŀ�����Ϊ��");
                SetPopIsAnime(false);
                return new List<InGameBallUI>();
            }

            var popBall = inGamePipeUI.PopBalls();
            if (popBall != null && popBall.Count > 0)
            {
                for (int i = 0; i < popBall.Count; i++)
                {
                    inGamePipeUI.ControllerEmptyList();
                }

                int callbackCount = 0;
                for (int i = 0; i < popBall.Count; i++)
                {
                    int currentIndex = i;
                    var currentBall = popBall[currentIndex];
                    if (currentBall == null)
                    {
                        callbackCount++;
                        continue;
                    }

                    FlyToTopPos(inGamePipeUI, currentBall, () =>
                    {
                        callbackCount++;
                        if (callbackCount == popBall.Count)
                        {
                            SetPopIsAnime(false);
                        }
                    });
                }
            }
            else
            {
                SetPopIsAnime(false);
            }

            return popBall ?? new List<InGameBallUI>();
        }

        private void FlyToTopPos(InGamePipeUI inGamePipeUI, InGameBallUI popBall, Action callBack = null)
        {
            callBack?.Invoke();
            if (_isStartTwoAnime)
            {
                _isStartTwoAnime = false;
            }
        }

        #endregion Anime

        #region ToolLogic

        private bool BallIsFlyToPipe()
        {
            if (_popBallUI)
            {
                var pos = _popBallUI.transform.parent.name;
                return pos == "FlyToPos";
            }

            return false;
        }

        public bool CanUseTool()
        {
            bool hasStep = _playerStep.Count > 0;
            bool noPopAnime = !_isPopOnAnime;
            bool noStartTwoAnime = !_isStartTwoAnime;
            bool noPushAnime = !_isPushOnAnime;
            bool noCoercion = !_isCoercion;
            bool ballNotFlying = !BallIsFlyToPipe();

            Debug.Log($"===== CanUseTool ������� =====");
            Debug.Log($"1. _playerStep.Count���ɳ��ز���������{_playerStep.Count} �� �в��裿{hasStep}");
            Debug.Log($"2. _isPopOnAnime�����������У���{_isPopOnAnime} �� ���ڵ���������{noPopAnime}");
            Debug.Log($"3. _isStartTwoAnime��˫���������У���{_isStartTwoAnime} �� ����˫������{noStartTwoAnime}");
            Debug.Log($"4. _isPushOnAnime�����򶯻��У���{_isPushOnAnime} �� �������򶯻���{noPushAnime}");
            Debug.Log($"5. _isCoercion��ǿ�Ʋ����У���{_isCoercion} �� ����ǿ�Ʋ�����{noCoercion}");
            Debug.Log($"6. BallIsFlyToPipe��������У���{!ballNotFlying} �� ���ڷ��У�{ballNotFlying}");
            bool canUse = hasStep && noStartTwoAnime && noPushAnime && noCoercion && ballNotFlying && noPopAnime;
            Debug.Log($"===== ���գ������Ƿ���ã�{canUse} =====");

            return canUse;
        }

        private void UndoStep()
        {
            if (_playerStep.Count == 0)
            {
                FloatingWindow.Instance.Show("û�пɳ��صĲ���");
                return;
            }

            var step = _playerStep.Pop();
            string fromPipeName = step.fromPipe != null ? step.fromPipe.name : "�չ��ӣ���ʧЧ��";
            string toPipeName = step.toPipe != null ? step.toPipe.name : "�չ��ӣ���ʧЧ��";

            int ballCount = step.balls.Count;

            for (int i = 0; i < ballCount; i++)
            {
                step.toPipe.PopBall();
            }

            foreach (var ball in step.balls)
            {
                if (ball != null)
                {
                    step.fromPipe.PushBall(ball);
                    ball.transform.SetParent(step.fromPipe.ballVerticalLayout.transform, false);
                }
            }

            if (step.fromPipe.ballVerticalLayout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(step.fromPipe.ballVerticalLayout.GetComponent<RectTransform>());
            }
            if (step.fromPipe.emptyVerticalLayout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(step.fromPipe.emptyVerticalLayout.GetComponent<RectTransform>());
            }
            if (step.toPipe.ballVerticalLayout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(step.toPipe.ballVerticalLayout.GetComponent<RectTransform>());
            }
            if (step.toPipe.emptyVerticalLayout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(step.toPipe.emptyVerticalLayout.GetComponent<RectTransform>());
            }

            EventDispatcher.instance.DispatchEvent(AppEventType.PlayerStepCountChange);
        }

        public void RevocationTool()
        {
            if (CanUseTool())
            {
                UndoStep();
                Game.Instance.CurrencyModel.ConsumeGoodNumber(GoodType.Tool, (int)GoodSubType.RevocationTool, 1);
            }
            else
            {
                FloatingWindow.Instance.Show("���������У����߲���ʹ��");
            }
        }

        private void AddPlayStep(InGamePipeUI popPipeUI, InGamePipeUI targetPipeUI, List<InGameBallUI> ballsToRecord)
        {
            if (_pushPipeUI != _popPipeUI && !_isCoercion && !_isStartTwoAnime && ballsToRecord.Count > 0)
            {
                var step = new StepData
                {
                    fromPipe = popPipeUI,
                    toPipe = targetPipeUI,
                    balls = new List<InGameBallUI>(ballsToRecord)
                };
                _playerStep.Push(step);
                EventDispatcher.instance.DispatchEvent(AppEventType.PlayerStepCountChange);
            }
        }

        #endregion ToolLogic
    }
}