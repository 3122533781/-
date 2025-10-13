using _02.Scripts.InGame;
using _02.Scripts.LevelEdit;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIDialog : Dialog
{
    [SerializeField] protected Button closeBtn;

    //?????????
    public Transform levelContent;

    //????UI???
    public LevelSelectUI levelSelectUI;

    public ScrollRect levelScrollRect;

    [SerializeField] private Image maskBG;

    private int passLevel;

    public LevelLoopList levelLoopList;

    public override void ShowDialog()
    {
        base.ShowDialog();
        StartCoroutine(PopCloseSlide());
    }

    public IEnumerator PopCloseSlide()
    {
        levelScrollRect.vertical = false;
        yield return new WaitForSeconds(0.15f);
        levelScrollRect.vertical = true;

        levelLoopList.Refresh();
    }

    public void Init()
    {
        base.Activate(false);
        levelLoopList.datas = new TestData[LevelConfig.Instance.All.Count];
        levelLoopList.Init();
        //CheckLevelData();
        base.Deactivate(false);
    }

    private void OnEnable()
    {
        //   maskBG.sprite = InGameManager.Instance.bg.sprite;
        closeBtn.onClick.AddListener(CloseLevelBtn);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(CloseLevelBtn);
    }

    public void CloseLevelBtn()
    {
        DialogManager.Instance.GetDialog<LevelUIDialog>().CloseDialog();
    }

    public List<LevelSelectUI> levelSelectUIs = new List<LevelSelectUI>();

    // Start is called before the first frame update
    public void CheckLevelData()
    {
        passLevel = Game.Instance.LevelModel.MaxUnlockLevel.Value;
        if (levelContent.childCount <= 0)
        {
            for (int i = 0; i < LevelConfig.Instance.All.Count; i++)
            {
                LevelSelectUI temp = Instantiate(levelSelectUI, levelContent);
                //??????????????????????¦Ä??????????????
                if (i < passLevel - 1)
                    temp.Init(i + 1, true);
                else if (i >= passLevel)
                    temp.Init(i + 1, false);

                levelSelectUIs.Add(temp);
            }
            //?????¦Â???
            levelSelectUIs[passLevel - 1].Init(passLevel);
        }
        //?????????¦Ë??
        levelSelectUIs[passLevel].GetVerticalValue(passLevel + 1);
    }

    public void PassLastLevel()
    {
        passLevel = Game.Instance.LevelModel.MaxUnlockLevel.Value;
    }

    //????????UI
    public void CloseLevelUI()
    {
        gameObject.SetActive(false);
    }
}