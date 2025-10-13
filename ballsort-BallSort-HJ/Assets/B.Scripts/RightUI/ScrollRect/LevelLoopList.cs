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
        if (levelValue / 5 > 0)
        {
            float tempValue = 1 - (levelValue / 5 * (5.0f / LevelConfig.Instance.All.Count) - (30 / 5 * (5.0f / LevelConfig.Instance.All.Count)));
            //parentRect.verticalNormalizedPosition = Mathf.Clamp(tempValue, 0, 1);
            return Mathf.Clamp(tempValue, 0, 1);
        }
        else
        {
            //parentRect.verticalNormalizedPosition = 1;
            return 1;
        }
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

        for (int i = 0; i < LevelConfig.Instance.All.Count; i++)
        {
            datas[i] = new TestData((i + 1).ToString(), "Item" + i);
        }

        /* MyLoopList<TestData, TestView>*/
        myLoopList = new MyLoopList<TestData, TestView>();
        myLoopList.Init(datas, 80f, 70f, new Vector2(15, 35), 168, 168, 5, datas.Length, go, rect, this);

    }


}