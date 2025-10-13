using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.Common
{
    public class TopBarUIMoney : MonoBehaviour
    {
        [SerializeField] private Text moneyTxt;
        [SerializeField] private RectTransform topUI;
        [SerializeField] private Text gemTxt;

        private int _originalHudLayerOrder;
        private string _originalLayerName;
        private bool isActive;

        //特殊处理顶部有异物的情况
        private void SetOffset()
        {
            float safeAreaOffset = Screen.height - Screen.safeArea.yMax; //Screen.safeArea.yMax
            topUI.offsetMax = new Vector3(0, -safeAreaOffset);
        }

        private void OnEnable()
        {
            Game.Instance.CurrencyModel.RegisterMoneyChangeAction(RefreshStarText);
            Game.Instance.CurrencyModel.RegisterToolChangeAction(GoodType.Gem, GoodSubType.Null, RefreshGemText);
            Start();
        }


        private void OnDisable()
        {
            Game.Instance.CurrencyModel.UnregisterMoneyChangeAction(RefreshStarText);
            Game.Instance.CurrencyModel.UnregisterToolChangeAction(GoodType.Gem, GoodSubType.Null, RefreshGemText);
        }


        private void Start()
        {
            RefreshStarText(-1, default);
            RefreshGemText(-1, default);
            // SetOffset();
        }

        public void RefreshStarText(float a, float b)
        {
            var num = Game.Instance.CurrencyModel.GetCurrentMoney();
            if (a == -1) moneyTxt.GetComponent<FloatNumberDisplayer>().ResetNumber(num);
            else moneyTxt.GetComponent<FloatNumberDisplayer>().Number = num;
        }

        public void RefreshGemText(int arg1, int arg2)
        {
            int num = Game.Instance.CurrencyModel.GetGoodNumber(GoodType.Gem);
            if (arg1 == -1) gemTxt.GetComponent<IntNumberDisplayer>().ResetNumber(num);
            else gemTxt.GetComponent<IntNumberDisplayer>().Number = num;
        }
    }
}