using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ICustomGridItem<T> where T : class
{

    protected T info;
    protected GameObject go;


    public virtual void Init(GameObject go)
    {
        this.go = go;
    }

    public virtual void UpdateView()
    {

    }
    public void SetInfo(T info)
    {
        this.info = info;
        UpdateView();
    }

}


public class MyLoopList<DataType, ItemType> where ItemType : ICustomGridItem<DataType>, new() where DataType : class
{
    public LevelLoopList levelLoopList;
    RectTransform scrollRect;

    RectTransform contentTR;
    public GameObject ItemGo;

    public int GridHeight;          //
    public int GridWidth;
    public int wholeItemCount;     //格子总数

    public int lineCount;           //每一行的格子数
    public float top;//边距
    public float left;

    public Vector2 space;
    public int firstLineIndex;//当前面板中的第一个Item的index
    int line;                       //面板里的行数
    float contentHeight;            //content根据grid数量初始化后的高度

    float lastContentRTy;

    int itemCountInView;            //面板里可容纳的最大Item数量
    float scrollViewHeight;         //面板的初始高度



    LinkedList<RectTransform> itemList = new LinkedList<RectTransform>();
    Dictionary<GameObject, ICustomGridItem<DataType>> gridList = new Dictionary<GameObject, ICustomGridItem<DataType>>();

    DataType[] datas;

    public void Init(DataType[] datas, float left, float top, Vector2 space, int gridHeight,
    int gridWidth, int lineCount, int wholeItemCount, GameObject itemGo, RectTransform scrollRect, LevelLoopList levelLoopList)
    {
        this.scrollRect = scrollRect;
        this.levelLoopList = levelLoopList;
        this.datas = datas;
        GridHeight = gridHeight;
        GridWidth = gridWidth;
        this.lineCount = lineCount;
        this.wholeItemCount = wholeItemCount;
        this.space = space;
        this.left = left;
        this.top = top;
        ItemGo = itemGo;

        contentTR = scrollRect.Find("Viewport/Content").GetComponent<RectTransform>();
        if (contentTR == null)
        {
            Debug.LogError("应该有叫'Content'的子物体");
            return;
        }

        var rect = scrollRect.GetComponent<RectTransform>().rect;
        scrollViewHeight = rect.height;
        Vector2 size = Vector2.zero;
        size.x = rect.width;
        contentHeight = (space.y + GridHeight) * wholeItemCount / lineCount + top; //设置content的高度
        size.y = contentHeight;

        contentTR.sizeDelta = size; //content的总大小被设置
        //上一帧的 content y轴位置
        lastContentRTy = GetContentPositionY();


        //将预设的宽高设置
        (ItemGo.transform as RectTransform).sizeDelta = new Vector2(GridWidth, GridHeight);

        //计算面板里能显示多少行  +2 防止穿帮
        line = (int)(scrollViewHeight / (GridHeight + space.x)) + 2;

        itemCountInView = line * lineCount; // view窗口里显示的item数量
        for (int i = 1; i < itemCountInView + 1; i++)
        {
            GameObject go = GameObject.Instantiate(ItemGo, contentTR);
            levelLoopList.levelSelectUIs.Add(go.GetComponent<LevelSelectUI>());
            var item = new ItemType();
            item.Init(go);
            gridList.Add(go, item);

            Vector3 pos = Vector3.zero;

            int count = (i % lineCount);
            if (count == 0) count = lineCount;

            pos.x = (i - 1) % lineCount * GridWidth + count * space.x;
            pos.x += left;
            int hang = (int)(Mathf.Ceil(i * 1.0f / lineCount));

            pos.y = -((hang - 1) * GridHeight + hang * space.y);
            pos.y -= top;
            pos.z = 0;
            var rectt = (go.transform as RectTransform);
            rectt.anchoredPosition = pos;
            itemList.AddLast(rectt);

        }
        //第一个go的索引  最后一个go的索引（对应的数据索引）
        firstLineIndex = 0;

        scrollRect.GetComponent<ScrollRect>().onValueChanged.AddListener(OnValueChanged);

        ItemGo.SetActive(false);

        Refresh();
    }

    public void OnValueChanged(Vector2 pos)
    {
        float nowContentRTy = GetContentPositionY();
        //往上移
        bool isUp = false;
        if (pos.x >= 1 || pos.y >= 1) return;
        if (nowContentRTy - lastContentRTy > 0)
        {
            if (scrollViewHeight + GetItemPositionY(false) + nowContentRTy > GridHeight
            && contentHeight - nowContentRTy > scrollViewHeight)
            {
                isUp = true;
                MoveItemGo(isUp);
                //firstLineIndex += lineCount;
            }

        }
        else
        {
            if (-GetItemPositionY(true) - nowContentRTy > space.y
            && nowContentRTy > 0)
            {
                isUp = false;
                MoveItemGo(isUp);
                //firstLineIndex -= lineCount;
            }
        }

        lastContentRTy = nowContentRTy;
    }
    //true 表示是处理第一个
    float GetItemPositionY(bool first)
    {
        float y;
        if (first)
            y = itemList.First.Value.anchoredPosition.y;
        else
            y = itemList.Last.Value.anchoredPosition.y;

        return y;
    }
    float GetContentPositionY()
    {
        return contentTR.anchoredPosition.y;
    }

    void SetInfo(ICustomGridItem<DataType> item, int index)
    {
        if (index >= 0 && index < datas.Length)
        {
            item.SetInfo(datas[index]);
        }
        else
        {
            item.SetInfo(null);
        }
    }

    public void SetButtonState()
    {

        int passLevel = Game.Instance.LevelModel.MaxUnlockLevel.Value;
        for (int i = 0; i < levelLoopList.levelSelectUIs.Count; i++)
        {
            LevelSelectUI temp = levelLoopList.levelSelectUIs[i];
            //通关的打开勾选，通关后的一关图片默认未通关，再之后的上锁
            if ((int.Parse)(temp.nubtext.text) < passLevel)
                temp.PassLevel();
            else if ((int.Parse)(temp.nubtext.text) > passLevel)
                temp.NotCleared();
            else
                temp.Init(passLevel);
        }
    }

    void MoveItemGo(bool up)
    {
        if (up)
        {//tou
            for (int i = 0; i < lineCount; i++)
            {
                var item = itemList.First.Value;

                Vector3 pos = item.anchoredPosition;

                pos.y -= line * (GridHeight + space.y);

                item.anchoredPosition = pos;

                itemList.RemoveFirst();

                itemList.AddLast(item);

                //注入数据
                int index = firstLineIndex + (line - 1) * lineCount + lineCount + i;


                SetInfo(gridList[item.gameObject], index);


            }
            firstLineIndex += lineCount;
        }
        else
        {
            for (int i = 0; i < lineCount; i++)
            {
                var item = itemList.Last.Value;

                Vector3 pos = item.anchoredPosition;

                pos.y += line * (GridHeight + space.y);

                item.anchoredPosition = pos;

                itemList.RemoveLast();
                itemList.AddFirst(item);
                int index = firstLineIndex - i - 1;

                SetInfo(gridList[item.gameObject], index);

            }
            firstLineIndex -= lineCount;
        }

        SetButtonState();
    }

    public void SetPos()
    { 
        int passLevel = Game.Instance.LevelModel.MaxUnlockLevel.Value;

        if (firstLineIndex < Mathf.Clamp(passLevel - 40, 0, Game.Instance.LevelModel.MaxUnlockLevel.Value))
        {
            while (firstLineIndex < Mathf.Clamp(passLevel - 40, 0, Game.Instance.LevelModel.MaxUnlockLevel.Value))
            {
                MoveItemGo(true);
            }
           
        }
        else
        {
            while (firstLineIndex > Mathf.Clamp(passLevel - 40, 0, Game.Instance.LevelModel.MaxUnlockLevel.Value))
            {
                MoveItemGo(false);
            }
        }


        scrollRect.GetComponent<ScrollRect>().verticalNormalizedPosition = levelLoopList.GetVerticalValue(passLevel + 1);

        SetButtonState();
    }

    public void Refresh()
    {
        var value = itemList.First;
        for (int i = firstLineIndex; i < firstLineIndex + itemCountInView; i++)
        {
            if (i >= datas.Length) return;
            if (value == null) return;
            gridList[value.Value.gameObject].SetInfo(datas[i]);
            gridList[value.Value.gameObject].UpdateView();
            value = value.Next;
        }

        SetButtonState();
    }
}