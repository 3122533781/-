using UnityEngine;
using UnityEngine.Events;

namespace ProjectSpace.BubbleMatch.Scripts.UtilFunction
{
    public class TabContent : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private UnityEvent onSelect = default;
        [SerializeField] private UnityEvent onUnSelect = default;


        public virtual void Select()
        {
            canvasGroup.SetCanvasGroupActive(true);
            gameObject.SetActive(true);
            onSelect?.Invoke();

            // TODO delete （char panel default active will trigger error）
        }

        public virtual void Unselect()
        {
            onUnSelect?.Invoke();
            canvasGroup.SetCanvasGroupActive(false);
            gameObject.SetActive(false);
        }
    }
}