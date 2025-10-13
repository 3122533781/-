using _02.Scripts.LevelEdit;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Image effImage;
    public Sprite passbtnIcon;
    public Sprite unPassbtnIcon;

    GameObject passIcon => transform.Find("Pass").gameObject;
    GameObject lockIcon => transform.Find("Lock").gameObject;
    Button levelBtn => GetComponent<Button>();
    public Text nubtext => transform.Find("Id").GetComponent<Text>();

    public int level;

    ScrollRect parentRect => transform.parent.parent.parent.GetComponent<ScrollRect>();
    //设置滑动条位置
    public float GetVerticalValue(int levelValue)
    {
        if (levelValue / 40 > 0)
        {
            float tempValue = 1 - (levelValue / 40 * (5 / LevelConfig.Instance.All.Count) - (20.0f / LevelConfig.Instance.All.Count));
            return tempValue;
        }
        else
        {
            return 1;
        }
    }


    private void OnEnable()
    {
        levelBtn.onClick.AddListener(ClickLoadLevel);
    }

    private void OnDisable()
    {
        levelBtn.onClick.RemoveListener(ClickLoadLevel);
    }

    //处理末尾关卡
    public void Init(int levelValue)
    {
        level = levelValue;
        transform.name = string.Format("LEVEL{0}", levelValue);
        nubtext.text = string.Format("{0}", levelValue);
        passIcon.SetActive(false);
        lockIcon.SetActive(false);
        effImage.SetActiveVirtual(true);
        transform.GetComponent<Image>().sprite = unPassbtnIcon;
    }

    //处理通关后及上锁关卡
    public void Init(int levelValue, bool pass)
    {
        level = levelValue;
        transform.name = string.Format("LEVEL{0}", levelValue);
        nubtext.text = string.Format("{0}", levelValue);
        if (pass)
        {
            PassLevel();
        }
        else
        {
            NotCleared();
        }
    }

    //通关后的处理
    public void PassLevel()
    {
        transform.GetComponent<Image>().sprite = passbtnIcon;
        passIcon.SetActive(true);
        lockIcon.SetActive(false);
    }

    //未通关的处理(上锁)
    public void NotCleared()
    {
        lockIcon.SetActive(true);
    }

    //未通关的处理(解锁)
    public void UnCleared()
    {
        lockIcon.SetActive(false);
    }

    public void ClickLoadLevel()
    {
        if (!lockIcon.gameObject.activeInHierarchy)
        {
            Game.Instance.RestartGame("LevelSelect",int.Parse(nubtext.text), forceShowAd: true);

            DialogManager.Instance.GetDialog<LevelUIDialog>().CloseDialog();
        }
    }
}