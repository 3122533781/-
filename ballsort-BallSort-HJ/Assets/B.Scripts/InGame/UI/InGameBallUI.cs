using _02.Scripts.LevelEdit;
using DG.Tweening;
using ProjectSpace.BubbleMatch.Scripts.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class UIEvents
{
    // 皮肤UI打开事件
    public static event Action OnDressUpDialogOpened;

    // 皮肤UI关闭事件
    public static event Action OnDressUpDialogClosed;

    // 触发皮肤UI打开事件
    public static void TriggerDressUpDialogOpened()
    {
        OnDressUpDialogOpened?.Invoke();
    }

    // 触发皮肤UI关闭事件
    public static void TriggerDressUpDialogClosed()
    {
        OnDressUpDialogClosed?.Invoke();
    }
}

namespace _02.Scripts.InGame.UI
{
    public class InGameBallUI : ElementUI<global::InGame>
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image grayIcon;
        [SerializeField] private Image obj1;
        [SerializeField] private Image obj2;
        private BallData _ballData;
        public bool _isBlackBall;
        private Sequence _pushSequence;
        private const float ScaleUpValue = 1.2f; // 放大到1.2倍
        private const float ScaleNormalValue = 1f; // 恢复到1倍
        private const float AnimDuration = 0.3f; // 动画时长（秒）
        private void OnEnable()
        {
            SpriteManager.Instance.AddBallData(this);
        }

        private void OnDisable()
        {
            SpriteManager.Instance.RemoveBallData(this);
        }

        public void InitBall(BallData data)
        {
            _ballData = data;
            _isBlackBall = LevelConfig.Instance.GetConfigByID(Game.Instance.LevelModel.EnterLevelID).blindBox;
            SetIcon();
        }
        public void InitBall2(BallData data)
        {
            _ballData = data;
            _isBlackBall = LevelConfig.Instance.GetConfigByID(Game.Instance.LevelModel.EnterLevelID).blindBox;
            SetIcon();
            icon.color = new Color(0, 0, 0, 0);
            obj1.gameObject.SetActive(false);
            obj2.gameObject.SetActive(false);
        }
        public BallData GetBallData()
        {
            return _ballData;
        }

        public void SetAlready()
        {
            if (!_isBlackBall || !Context.CellMapModel.LevelData.blindBox) return;
            _isBlackBall = false;
            grayIcon.SetActiveVirtual(true);
            grayIcon.DOFade(0, 0.5f).OnComplete(() => { grayIcon.SetActiveVirtual(false); });
            SetIcon();
        }

        public void SetIcon()
        {
            if (_ballData != null)
            {
                icon.sprite = SpriteManager.Instance.GetBallIcon(_isBlackBall, _ballData.type);
                obj1.sprite = SpriteManager.Instance.GetBallIcon(_isBlackBall, _ballData.type);
                obj2.sprite = SpriteManager.Instance.GetBallIcon(_isBlackBall, _ballData.type);
                icon.SetNativeSize();
                obj1.SetNativeSize();
                obj2.SetNativeSize();
            }
            // gameObject.transform.localScale = new Vector3(1, _isBlackBall ? -1 : 1, 1);
        }

        public void SGLight()
        {
            obj1.gameObject.SetActive(true);
            // 激活obj2
            if (obj2 != null)
                obj2.gameObject.SetActive(true);

            // 对icon执行放大动画（带渐变效果）
            if (icon != null)
            {
                // 先清除icon上的现有动画，避免冲突
                icon.DOKill();

                // 放大动画：从当前缩放过渡到ScaleUpValue，使用缓动函数让效果更自然
                icon.transform.DOScale(ScaleUpValue, AnimDuration)
                    .SetEase(Ease.OutQuad); // 缓出效果，开始快结束慢

            
            }
        }

        public void CloseSG()
        {
            obj1.gameObject.SetActive(false);
            // 对icon执行缩小动画（带渐变效果）
            if (icon != null)
            {
                // 先清除icon上的现有动画，避免冲突
                icon.DOKill();

                // 缩小动画：从当前缩放过渡到ScaleNormalValue
                icon.transform.DOScale(ScaleNormalValue, AnimDuration)
                    .SetEase(Ease.InQuad); // 缓入效果，开始慢结束快

             
            }

            // 缩小动画完成后隐藏obj2（确保动画播放完再隐藏）
            if (obj2 != null)
            {
                // 延迟等于动画时长，确保缩放完成后再隐藏
                DOVirtual.DelayedCall(AnimDuration, () =>
                {
                    obj2.gameObject.SetActive(false);
                });
            }
        }

        public void StopPushAnime()
        {
            if (_pushSequence != null && _pushSequence.IsPlaying())
            {
                _pushSequence.Kill();
            }
        }

        public void SetPushAnime(Sequence newSequence)
        {
            _pushSequence = newSequence;
        }
    }
}