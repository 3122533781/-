using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
public class TaskDialog : Dialog
{
    private QuestConfig quest;
    [SerializeField] private Text descriptionLabel;
    [SerializeField] private Text BtnDes;
    [SerializeField] private Button PlayBtn;
    [SerializeField] private Image BtnImage;
    public List<Sprite> sprites;
    // Start is called before the first frame update


    private void OnEnable()
    {
        PlayBtn.onClick.AddListener(GotoPlay);
      
    }
    public void InitDialog(QuestConfig questconfig)
    {
        quest = questconfig;
        base.ShowDialog();
        RefreshDes();
        RefreshBtn();
        Debug.Log("任务状态为"+QuestManager.Instance.GetQuestStatus(quest.questType));

    }
    private void GotoPlay()
    {
        base.CloseDialog();
        TransitionManager.Instance.Transition(0.5f, () => { SceneManager.LoadScene("InGameScenario"); },
                  0.5f);
    }

    private void RefreshBtn()
    {
        if(QuestManager.Instance.GetQuestStatus(quest.questType))
        {
            PlayBtn.interactable = false;
            BtnDes.text = "已完成";
            BtnImage.sprite = sprites[1];
        }
        else
        {
            PlayBtn.interactable = true;
            BtnDes.text = "去完成";
            BtnImage.sprite = sprites[0];
        }



    }

    private void RefreshDes()
    {
        switch (quest.questType)
        {
            case QuestType.Completebowlplating:
            {
                    descriptionLabel.text = quest.description;
                    break;
                }
            case QuestType.Completeplateplating:
            {
                    descriptionLabel.text = quest.description;
                    break;
                }
            case QuestType.Completecupplating:
            {
                    descriptionLabel.text = quest.description;
                    break;
                }
            case QuestType.Completechopsticksplating:
                {
                    descriptionLabel.text = quest.description;
                    break;
                }
            case QuestType.Completeknifeplating:
                {
                    descriptionLabel.text = quest.description;
                    break;
                }
        }
    }


}
