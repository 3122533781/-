using System;
using System.Collections.Generic;
using _02.Scripts.LevelEdit;
using _02.Scripts.Util;
using ProjectSpace.BubbleMatch.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.InGame.UI
{
    /// <summary>
    /// 特殊第五管不能修改球的大小了，整体只能放大120/106 =1.132
    /// </summary>
    public class PipeSizeController : MonoBehaviour
    {
        [SerializeField] private Image pipeHead;
        [SerializeField] private Image pipeBody;
        [SerializeField] private RectTransform pipeBodyRectTransform;
        [SerializeField] private ContentSizeFitter contentSizeFitter;
        [SerializeField] private RectTransform rootRectTransform;
        private PipeData _currentData;

        /// <summary>
        /// 五球变其他情况会放大
        /// </summary>
        /// <param name="data"></param>
        public void SetPipe(PipeData data)
        {
            _currentData = data;
            RefreshSKin();
        }

        public void RefreshSKin()
        {
            var id = PlayerPrefs.GetInt("ClickThemeSkin");
            //   Debug.Log("大小" + id);
            var spriteList = SpriteManager.Instance.GetPipeSkin(id);
            pipeHead.sprite = spriteList[0];
            var headRect = (RectTransform)pipeHead.transform;
            //headRect.sizeDelta = InGameManager.Instance.pipeSizeConfig.GetPipeHeadSize();
            //  pipeHead.SetNativeSize();
            pipeBody.sprite = spriteList[1];
            //  pipeBody.SetNativeSize();
            var newHigh = InGameManager.Instance.pipeSizeConfig.GetPipeBodyHigh(_currentData.pipeCapacity);
            //  pipeBodyRectTransform.SetSizeDeltaY(newHigh);

            var getData = InGameManager.Instance.pipeSizeConfig.GetPipeSizeData(_currentData.pipeCapacity);
            if (getData.isUseTile)
            {
                pipeBody.type = Image.Type.Sliced;
            }
            else
            {
                pipeBody.type = Image.Type.Sliced;
            }
            contentSizeFitter.enabled = true;
            //  contentSizeFitter.enabled = !getData.isNotUseContentSizeFitter;
        }
    }
}