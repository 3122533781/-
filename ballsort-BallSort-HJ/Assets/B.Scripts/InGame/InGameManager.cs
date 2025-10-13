using System;
using System.Collections.Generic;
using _02.Scripts.LevelEdit;
using ProjectSpace.BubbleMatch.Scripts.Util;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _02.Scripts.InGame
{
    [Serializable]
    public class PipeSizeData
    {
        public int pipeId;
        public bool isUseTile;
        public bool isNotUseContentSizeFitter;

        public List<int> bodySizeContent;
    }

    [Serializable]
    public class PipeSizeConfig
    {
        [SerializeField] private List<PipeSizeData> dataList;
        [SerializeField] private List<Vector2> headSize;

        public PipeSizeData GetPipeSizeData(PipeCapacity pipeCapacity)
        {
            var id = PlayerPrefs.GetInt("ClickTubeSkin");
#if UNITY_IPHONE
            if (id == 0)
            {
                id = 5;
            }else if(id == 5)
            {
                id = 0;
            }
#endif
            var configData = dataList.Find(x => x.pipeId == id + 1);
            if (configData == null)
            {
                configData = dataList[0];
            }

            return configData;
        }

        public float GetPipeBodyHigh(PipeCapacity capacity)
        {
            var data = GetPipeSizeData(capacity);
            return data.bodySizeContent[(int)capacity - 1];
        }

        public float GetTotalHigh(PipeCapacity capacity)
        {
            var id = PlayerPrefs.GetInt("ClickTubeSkin");
            if (id <= 0)
            {
                id = 1;
            }
            else if (id >= headSize.Count)
            {
                id = headSize.Count - 1;
            }

            var data = GetPipeSizeData(capacity);
            return (data.bodySizeContent[(int)capacity - 1] + headSize[id].y);
        }

        public float GetWidth()
        {
            var id = PlayerPrefs.GetInt("ClickTubeSkin") - 1;
            if (id <= 0)
            {
                id = 1;
            }

            return headSize[id].x;
        }

        public Vector2 GetPipeHeadSize()
        {
            var id = PlayerPrefs.GetInt("ClickTubeSkin") - 1;
            if (id <= 0)
            {
                id = 1;
            }

            return headSize[id];
        }
    }

    public class InGameManager : MonoSingleton<InGameManager>
    {
        public PipeSizeConfig pipeSizeConfig;
        public float h1 = 1.3f;
        public float h2 = 0.8f;
        public float scale = 120 / 106f;
        public float maxStar = 10;

        public RectTransform root;

        [SerializeField] public Image bg;

        private void OnEnable()
        {
            SpriteManager.Instance.SetBallSortBg(PlayerPrefs.GetInt("ClickThemeSkin"));
        }
    }
}