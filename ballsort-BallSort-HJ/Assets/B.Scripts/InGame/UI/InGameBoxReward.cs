using _02.Scripts.Config;
using _02.Scripts.Util;
using DG.Tweening;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.IAPModule;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.InGame.UI
{
    public class InGameBoxReward : MonoBehaviour
    {
        [SerializeField] private RectTransform rewardIconRectTransform;
        [SerializeField] private Text progress;
        [SerializeField] public Button watchAdButton;
        private RewardData _coinData;

        private void OnEnable()
        {
            watchAdButton.onClick.AddListener(WatchAdButton);

            JobUtils.Delay(0.4f, () => { Refresh(_coinData); });
        }

        private void OnDisable()
        {
            watchAdButton.onClick.RemoveListener(WatchAdButton);
        }

        public void Init(RewardData data)
        {
          
        }

        public void Refresh(RewardData rewardData)
        {
         

      
        }

        private void WatchAdButton()
        {
     
        }
    }
}