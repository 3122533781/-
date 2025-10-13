using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectSpace.BubbleMatch.Scripts.UtilFunction
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField] public Button bgImage;
        [SerializeField] private List<Sprite> bgImageList = default;
        [HideInInspector] public int index;


        private TabButtonHub _buttonHub;
        private RectTransform _rectTransform;

        private void Awake()
        {
            if (_buttonHub == null)
            {
                _buttonHub = transform.parent.GetComponent<TabButtonHub>();
            }

            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
        }

        private void OnEnable()
        {
            
            if (bgImage)
            {
                bgImage.onClick.AddListener(ButtonAction);
            }
        }

        private void OnDisable()
        {
            bgImage.onClick.RemoveAllListeners();
        }

        private void ButtonAction()
        {
            _buttonHub.Select(index);
        }

        public virtual void Select()
        {
            if (_buttonHub == null)
            {
                _buttonHub = transform.parent.GetComponent<TabButtonHub>();
            }

            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }


            if (bgImage != null)
            {
                bgImage.image.sprite = bgImageList[1];
            }


            // bgImage.sprite = _buttonHub.GetButtonBg(true);

            // ResetSize(widget, height);
        }

        public virtual void Unselect()
        {
            if (bgImage != null)
            {
                bgImage.image.sprite = bgImageList[0];
            }
            // bgImage.sprite = _buttonHub.GetButtonBg(false);

            // ResetSize(widget, height);
        }

        private void ResetSize(float widget, float height)
        {
            _rectTransform.sizeDelta = new Vector2(widget, height);
        }

        public virtual void InitPosData()
        {
        }
    }
}