using UnityEngine;
using UnityEngine.UI;

namespace ProjectSpace.Lei31Utils.Scripts.Common
{
    public class RedDotItem : MonoBehaviour
    {
        [SerializeField] private Text numText = default;
        public AppriseType appriseType;
        public bool canShow = true;
        private RedDotSystem RedDotSystem { get; set; }

        public void Awake()
        {
            RedDotSystem = RedDotSystem.Instance;
            RedDotSystem.SetRedPointNodeCallback(appriseType, SetShow);
            SetShow(RedDotSystem.GetRedPoint(appriseType));
        }

        public void Refresh(AppriseType type)
        {
            appriseType = type;
            Refresh();
        }

        public void Refresh()
        {
            var node = RedDotSystem.Instance.GetRedPoint(appriseType);
            RedDotSystem.Instance.SetRedPointNodeCallback(appriseType, SetShow);
            SetShow(node);
        }

        private void SetShow(RedPointNode node)
        {
            if (this == null)
            {
                return;
            }

            if (node == null)
            {
                gameObject.SetActiveVirtual(false);
                return;
            }

            if (node.PointNum <= 0)
            {
                gameObject.SetActive(false);
                return;
            }


            gameObject.SetActive(canShow);


            if (numText)
            {
                numText.text = node.PointNum.ToString();
            }
        }
    }
}