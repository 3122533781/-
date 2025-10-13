using _02.Scripts.Common;
using _02.Scripts.Util;
using ProjectSpace.BubbleMatch.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.LevelEdit
{
    public class PipeLevelEdit : MonoBehaviour
    {
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
        [SerializeField] private Image pipe;
        [SerializeField] private Image head;
        [SerializeField] private BallLevelEdit ballPrefab;
        public PipeData _pipeData;
        public Stack<BallLevelEdit> _ballLevelEdits = new Stack<BallLevelEdit>();

        public void Init(PipeData pipeData)
        {
            _pipeData = pipeData;
            pipe.sprite = SpriteManager.Instance.GetPipeIcon();
            pipe.rectTransform.sizeDelta = UtilClass.GetPipeSize(_pipeData.pipeCapacity);
            head.sprite = SpriteManager.Instance.HeadIcon(pipeData.exclusiveType);
            if (pipeData.exclusiveType == Typeexclusive.None)
                head.gameObject.SetActive(false);
           // pipe.rectTransform.sizeDelta = UtilClass.GetPipeSize(_pipeData.pipeCapacity);
            foreach (var data in _pipeData.ballDataStack)
            {
                var obj = Instantiate(ballPrefab, verticalLayoutGroup.transform);
                obj.InitBall(data);
                obj.name = $"Ball{data.type}";
                _ballLevelEdits.Push(obj);
            }
        }

        public void AddBallLimitColor(BallData ballData)
        {
            if (CanPushBallLimit(ballData))
            {
                AddBall(ballData);
            }
        }

        public void AddBall(BallData ballData)
        {
            if (_pipeData.CanPushBall())
            {
                _pipeData.Push(ballData);
                var obj = Instantiate(ballPrefab, verticalLayoutGroup.transform);
                obj.InitBall(ballData);
                obj.name = $"Ball{ballData.type}";
                _ballLevelEdits.Push(obj);
            }
        }

        public BallLevelEdit PopBall()
        {
            var obj = _ballLevelEdits.Pop();
            _pipeData.Pop();
            return obj;
        }

        public bool PeekBall()
        {
            var obj = _ballLevelEdits.Count > 0;
            return obj;
        }


        public void DeleteBall()
        {
            var result = PeekBall();
            if (result)
            {
                var obj = PopBall();
                DestroyImmediate(obj.gameObject);
            }
        }

        public void CleanBall()
        {
            while (_pipeData.ballDataStack.Count > 0)
            {
                DeleteBall();
            }
        }

        public void FullBall(BallData data)
        {
            while (_pipeData.CanPushBall())
            {
                AddBall(data);
            }
        }


        public bool CanPushBallLimit(BallData ballData)
        {
            return _pipeData.ballDataStack.Count == 0 ||
                   (_pipeData.ballDataStack.Count < (int) _pipeData.pipeCapacity &&
                    ballData.type != _ballLevelEdits.Peek().ballData.type);
        }
    }
}