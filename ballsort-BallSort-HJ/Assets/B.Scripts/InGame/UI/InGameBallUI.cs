using _02.Scripts.LevelEdit;
using DG.Tweening;
using ProjectSpace.BubbleMatch.Scripts.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class UIEvents
{
    // Ƥ��UI���¼�
    public static event Action OnDressUpDialogOpened;

    // Ƥ��UI�ر��¼�
    public static event Action OnDressUpDialogClosed;

    // ����Ƥ��UI���¼�
    public static void TriggerDressUpDialogOpened()
    {
        OnDressUpDialogOpened?.Invoke();
    }

    // ����Ƥ��UI�ر��¼�
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
            obj2.gameObject.SetActive(true);
        }
        public void CloseSG()
        {
            if (obj1 != null) obj1.gameObject.SetActive(false);
            if (obj2 != null) obj2.gameObject.SetActive(false);
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