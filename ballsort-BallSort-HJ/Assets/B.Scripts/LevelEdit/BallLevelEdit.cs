using _02.Scripts.Common;
using ProjectSpace.BubbleMatch.Scripts.Util;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _02.Scripts.LevelEdit
{
    public class BallLevelEdit : MonoBehaviour
    {
        [SerializeField] private Image icon;
        public BallData ballData;

        public void InitBall(BallData data)
        {
            ballData = data;
            icon.sprite = SpriteManager.Instance.GetBallIcon(false, ballData.type);
        }

        public void UpdateSkin()
        {
            icon.sprite = SpriteManager.Instance.GetBallIcon(false, ballData.type);
        }
    }
}