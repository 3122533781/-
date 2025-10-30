using _02.Scripts.InGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.InGame
{
    public class GameStage : MonoSingleton<GameStage>
    {
        public InGamePipeUI cellPrefab;
        public VerticalLayoutGroup spawnPanel;
        public HorizontalLayoutGroup spawnRectTransform;
        public HorizontalLayoutGroup spawnRectTransform2;
        public HorizontalLayoutGroup spawnRectTransform3;
        public HorizontalLayoutGroup spawnRectTransform4;
        public RectTransform gameCanvas;
    }
}