using _02.Scripts.LevelEdit;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoopList : MonoBehaviour
{

    public RectTransform rect;
    public GameObject go;
    public TestData[] datas;
    public LevelUIDialog levelUIDialog;
    public List<LevelSelectUI> levelSelectUIs = new List<LevelSelectUI>();
    MyLoopList<TestData, TestView> myLoopList;
    // Start is called before the first frame update

public float GetVerticalValue(int levelValue)
{
    float totalRow = Mathf.Ceil((float)LevelConfig.Instance.All.Count / 5); // 总行数（5列/行）
    float currentRow = Mathf.Ceil((float)levelValue / 5); // 当前关卡所在行数（浮点除法）
    
    if (currentRow > totalRow) currentRow = totalRow;
    if (currentRow < 1) currentRow = 1;
    
    // 计算垂直归一化位置（1为最顶部，0为最底部）
    float tempValue = 1 - (currentRow - 1) / (totalRow - 1); 
    return Mathf.Clamp(tempValue, 0, 1);
}

    public void Refresh()
    {
        myLoopList.SetPos();
    }

    public void Init()
    {
        //go = transform.Find("Item").gameObject;
        //go.SetActive(false);
        //rect = transform.Find("Scroll View").GetComponent<RectTransform>();
        //datas = levelUIDialog

        for (int i = 0; i <50; i++)
        {
            datas[i] = new TestData((i + 1).ToString(), "Item" + i);
        }

        /* MyLoopList<TestData, TestView>*/
        myLoopList = new MyLoopList<TestData, TestView>();
        myLoopList.Init(datas, 80f, 70f, new Vector2(40, 35), 168, 168, 5, datas.Length, go, rect, this);

    }


}