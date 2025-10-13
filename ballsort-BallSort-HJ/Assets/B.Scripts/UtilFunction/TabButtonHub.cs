using System;
using System.Collections.Generic;
using ProjectSpace.BubbleMatch.Scripts.Util;
using UnityEngine;

namespace ProjectSpace.BubbleMatch.Scripts.UtilFunction
{
    public enum TabMenuType
    {
        Null = 0,
        CardPageMenu = 1,
        GuildPageMenu = 2,
        ShopPageMenu = 3,
    }

    public class TabButtonHub : MonoBehaviour
    {
        [SerializeField] public List<TabButton> tabs;
        [SerializeField] public List<TabContent> contents;
        [SerializeField] private TabMenuType tabMenuType;
        [SerializeField] private int menuCount;
        [SerializeField] private bool isFreezing;
        public int SelectedIndex { get; set; }
        private string _tipContext;
        private int currentIndex;

        private int jumpChangeSelectCount;

        private void Awake()
        {

            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].index = i;
                tabs[i].InitPosData();
            }
        }

        private void OnDestroy()
        {
        }


        private void ChangeSelect(object[] objs)
        {
            ValueTuple<TabMenuType> data = (ValueTuple<TabMenuType>) objs[0];
            var page = data.Item1;
            if (page != tabMenuType)
            {
                return;
            }

            if (jumpChangeSelectCount > menuCount - 1) //防止全部锁定导致无限递归
            {
                SelectedIndex = currentIndex;
                jumpChangeSelectCount = 0;
                return;
            }

            SelectedIndex++;
            int index = SelectedIndex % menuCount;
            if (!CheckTabSelectMatch(index)) //自动选择时跳过锁定和隐藏的tab
            {
                jumpChangeSelectCount++;
                ChangeSelect(objs);
                return;
            }

            Select(index);
        }

        public void Select(object[] objs)
        {
            ValueTuple<TabMenuType, int, Action> value = (ValueTuple<TabMenuType, int, Action>) objs[0];

            if (value.Item1 != tabMenuType)
            {
                return;
            }

            Select(value.Item2);
            value.Item3?.Invoke();
        }

        public void SelectCurrentPage()
        {
            Select(currentIndex);
        }

        public void Select(int index)
        {
            if (isFreezing)
            {
                return;
            }

            if (!CheckTabSelectMatch(index))
            {
                return;
            }

            AudioClipHelper.Instance.PlaySound(AudioClipEnum.Tab);
            OnButtonSelect(index);
            OnContentSelect(index);
        }

        private bool CheckTabSelectMatch(int index)
        {
            return index >= 0 && index < tabs.Count && tabs[index].gameObject.activeSelf;
        }

        private void OnContentSelect(int index)
        {
            for (int i = 0; i < contents.Count; i++)
            {
                if (i == index)
                {
                    contents[i].Select();
                    SelectedIndex = index;
                }
                else
                {
                    contents[i].Unselect();
                }
            }
        }

        private void OnButtonSelect(int index)
        {
            for (int i = 0; i < tabs.Count; i++)
            {
                if (i == index)
                {
                    tabs[i].Select();
                    currentIndex = index;
                }
                else
                {
                    tabs[i].Unselect();
                }
            }
        }

        public void OnCurrentPageSelect()
        {
            Select(currentIndex);
        }

        public void OnPageUnSelect()
        {
            tabs[currentIndex].Unselect();
            contents[currentIndex].Unselect();
        }

        public void FreezeButtons(string tip)
        {
            isFreezing = true;
            _tipContext = tip;
        }

        public void ActiveButtons()
        {
            isFreezing = false;
        }

        public void SetTabActiveByIndex(int index, bool active)
        {
            if (index < 0)
            {
                return;
            }

            if (index < tabs.Count)
            {
                tabs[index].SetActiveVirtual(active);
            }


            if (active || currentIndex != index)
            {
                return;
            }

            //如果隐藏的刚好是现在选择的，则重新找一个选择
            int curIndex = 0;
            for (int i = 0; i < tabs.Count; i++)
            {
                if (i != index)
                {
                    curIndex = i;
                    break;
                }
            }

            Select(curIndex);
        }

        public void RemoveTab(int index)
        {
            if (tabs.Count == 0)
            {
                return;
            }

            tabs[index].SetActiveVirtual(false);
            contents[index].SetActiveVirtual(false);
            tabs.RemoveAt(index);
            contents.RemoveAt(index);
        }
    }
}