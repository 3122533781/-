using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ProjectSpace.BubbleMatch.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using Spine;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void GotoBallSort()
    {
        //if (Game.Instance.CurrencyModel.DiamondNum < 1)
        //{
        //    DialogManager.Instance.GetDialog<DiamondGetDialog>().ShowDialog();
        //    return;
        //}
        //Game.Instance.CurrencyModel.ConsumeDiamond(1);
        TransitionManager.Instance.Transition(0.5f, () => { SceneManager.LoadScene("InGameScenario"); },
                     0.5f);
    }

    private void ClickSetting()
    {
        DialogManager.Instance.GetDialog<OptionDialog>().ShowDialog();
    }

    public void OpenSkinMainUI()
    {
     
    }

    //[SerializeField] public CanvasGroup TopGroup;

    //public async Task PlayEnterAnim()
    //{
    //    float animTime = 0.3f;
    //    TopGroup.DOFade(1f, animTime);
    //    _btnGetCoin.GetComponent<RectTransform>().DOScale(1f, animTime);
    //    _btnUseHint.GetComponent<RectTransform>().DOScale(1f, animTime);
    //    _btnSkipLevel.GetComponent<RectTransform>().DOScale(1f, animTime);
    //    _btnSetting.GetComponent<RectTransform>().DOScale(1f, animTime);
    //    await TaskExtension.DelaySecond(animTime);
    //}

    //private void AnimReady()
    //{
    //    TopGroup.alpha = 0;
    //    _btnGetCoin.GetComponent<RectTransform>().localScale = Vector3.zero;
    //    _btnUseHint.GetComponent<RectTransform>().localScale = Vector3.zero;
    //    _btnSkipLevel.GetComponent<RectTransform>().localScale = Vector3.zero;
    //    _btnSetting.GetComponent<RectTransform>().localScale = Vector3.zero;
    //}

    private void OnEnable()
    {

        InitQuests();
        _gotoBallSort.onClick.AddListener(GotoBallSort);
        skinEntryBtn.onClick.AddListener(OpenSkinMainUI);
        btnSetting.onClick.AddListener(ClickSetting);
        btncheckIn.onClick.AddListener(OpenCheck);
        btnFriendRank.onClick.AddListener(OpenRanking);
        ShareBtn.onClick.AddListener(ShareDialog);
        btnCollection.onClick.AddListener(OpenCollection);
        btnTask.onClick.AddListener(OpenTask);
        //_isSettingPanelShow = false;
        //_settingPanelRect.SetAnchoredPositionY(_hideSettingPanelY);
        Refresh();
        //_btnSkipLevel.onClick.AddListener(ClickSkipLevel);
        _btnUseHint.onClick.AddListener(ClickHint);
        _btnGetCoin.onClick.AddListener(ClickGetCoin);
        _btnGetPower.onClick.AddListener(GetPower);
        Playbtn.onClick.AddListener(GotoBallSort);
        //_btnExit.onClick.AddListener(ClickExit);
        //_btnSetting.onClick.AddListener(ClickSetting);
        //_btnRestart.onClick.AddListener(ClickRestart);
        UIEvents.OnDressUpDialogOpened += OnDressUpDialogOpened;
        UIEvents.OnDressUpDialogClosed += OnDressUpDialogClosed;
    }

    private void OnDisable()
    {
        //    _btnSkipLevel.onClick.RemoveListener(ClickSkipLevel);
        _btnUseHint.onClick.RemoveListener(ClickHint);
        _btnGetCoin.onClick.RemoveListener(ClickGetCoin);
        _btnGetPower.onClick.RemoveListener(GetPower);
        //    _btnExit.onClick.RemoveListener(ClickExit);
        //    _btnSetting.onClick.RemoveListener(ClickSetting);
        //    _btnRestart.onClick.RemoveListener(ClickRestart);
        UIEvents.OnDressUpDialogOpened -= OnDressUpDialogOpened;
        UIEvents.OnDressUpDialogClosed -= OnDressUpDialogClosed;
    }

    //private void ClickRestart()
    //{
    //    App.Instance.EnterGame();
    //}
    private void OnDressUpDialogOpened()
    {
        // 皮肤UI打开时，隐藏_balls
        if (_balls != null)
        {
            _balls.SetActive(false);
            Debug.Log("_balls 已隐藏");
        }
    }
    private void ShareDialog()
    {
       // WXSDKManager.Instance.ShowShare();
    }
    private void OnDressUpDialogClosed()
    {
        
    }

    private void ClickHint()
    {
       
        //}
        //else
        //{
        //    if (App.Instance.CurrencyModel.CoinNum >= GameConfig.Instance.HintConsumeCoin)
        //    {
        //        App.Instance.CurrencyModel.ConsumeCoin(GameConfig.Instance.HintConsumeCoin);
        //        RewardClaimHandle.ConsumeCoin(GameConfig.Instance.HintConsumeCoin, "System", "Hint");
        //        Context.LevelModel.SetHasHint();
        //        _hintCoinPanel.SetActive(false);
        //        Context.GetController<InGameHintControl>().ShowHint();
        //    }
        //    else
        //    {
        //        FloatingWindow.Instance.Show("You not have enough coin.");
        //        DialogManager.Instance.GetDialog<CoinGetDialog>().Activate();
        //    }
        //}
    }

    //private void ClickSetting()
    //{
    //    if (_isSettingPanelShow)
    //    {
    //        HideSettingPanel();
    //    }
    //    else
    //    {
    //        ShowSettingPanel();
    //    }
    //}

    //private void ClickExit()
    //{
    //    App.Instance.BackHome();
    //}
    private void OpenTask()
    {
        QuestConfigs configs = Resources.Load<QuestConfigs>("Configs/QuestConfig");
        var temp = configs.quests[0];
        DialogManager.Instance.GetDialog<TaskDialog>().InitDialog(temp);
    }
    private void OpenCheck()
    {
        DialogManager.Instance.GetDialog<CheckinDialog>().ShowDialog();
    }
    private void OpenCollection()
    {
        DialogManager.Instance.GetDialog<CollectionDialog>().Show();
    }
    private void OpenRanking()
    {
       // WXSDKManager.Instance.ShowFriendRand();
    }

    public void GetPower()
    {
       
    }

    private void InitQuests()
    {
        QuestConfigs configs = null;

        configs = Resources.Load<QuestConfigs>("Configs/QuestConfig");
        List<Quest> quests = GameDataManager.Instance.GetQuest();

        List<Quest> data;
        if (quests != null)
            data = new List<Quest>(quests);
        else
            data = new List<Quest>();
        List<Quest> questList = new List<Quest>();
        foreach (QuestConfig config in configs.quests)
        {
            var exist = data.Find(i => i.ID == config.ID);
            questList.Add(new Quest()
            {
                ID = config.ID,
                questType = config.questType,

                rewardCount = config.rewardCount,
                status = (exist != null) ? exist.status : QuestStatus.Pendding,
                finishedCount = (exist != null) ? exist.finishedCount : 0,
                targetCount = config.targetCount,
                description = config.description,
                date = (exist != null) ? exist.date : GetDateString(System.DateTime.Now)
            });
        }
        foreach (Quest quest in questList)
        {
            if (quest.date != GetDateString(System.DateTime.Now))
            {
                quest.finishedCount = 0;
                quest.status = QuestStatus.Pendding;
                quest.date = GetDateString(System.DateTime.Now);
            }
        }

        GameDataManager.Instance.SetQuest(questList);
    }




    private void EnterGame()
    {
   
    
        foreach (var stage in gameStages)
        {
            if (stage != null) // 额外检查物体是否存在
            {
                stage.SetActive(true);
                var sequence = DOTween.Sequence();

                sequence.Append(stage.transform.DOScale(1.15f, 0.295f));
                sequence.Append(stage.transform.DOScale(0.85f, 0.17f));
                sequence.Append(stage.transform.DOScale(1.00f, 0.17f));

                sequence.SetEase(Ease.OutQuart);
                sequence.SetAutoKill(true);
            }
            Playbtn.SetActive(false);
            _circle.SetActive(false);
        }
    }

    private void ClickGetCoin()
    {
        ADMudule.ShowRewardedAd(ADPosConst.GetCoinDialog, (isSuccess) =>
        {
            if (isSuccess)
            {
                //IStaticDelegate.SourceCurrency("Coin", GameConfig.Instance.AdRewardCoin, "AD",
                //    ADPosConst.GetCoinInGame);
                Game.Instance.CurrencyModel.RewardCoin(20);
            }
        });
    }

    //private void ClickSkipLevel()
    //{
    //    ADMudule.ShowRewardedAd(ADPosConst.SkipLevel, (isSuccess) =>
    //    {
    //        if (isSuccess)
    //        {
    //            App.Instance.LevelModel.PassCurrentLevel();
    //            App.Instance.EnterGame();
    //        }
    //    });
    //}
    private string GetDateString(System.DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }
    private void Refresh()
    {
       
        
        //_textGetCoinCount.text = $"+{GameConfig.Instance.AdRewardCoin}";
        //_textHintCoin.text = GameConfig.Instance.HintConsumeCoin.ToString();
    }

 
    [SerializeField] private Button _gotoBallSort;

    [SerializeField] private Button skinEntryBtn;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btncheckIn;
    [SerializeField] private Button btnFriendRank;
    [SerializeField] private Button ShareBtn;
    [SerializeField] private Button btnCollection;
    [SerializeField] private Button btnTask;
    [SerializeField] private Button Playbtn;
    [SerializeField] private Button _btnGetCoin;
    [SerializeField] private Button _btnUseHint;
    [SerializeField] private Button _btnGetPower;
    [SerializeField] private Text _textLevel;
    [SerializeField] private List<GameObject> gameStages = new List<GameObject>();
    [SerializeField] private GameObject _playbtn;
    [SerializeField] private GameObject _balls;
    [SerializeField] private GameObject _line;
    [SerializeField] private GameObject _circle;
    //[SerializeField] private Button _btnExit;
    //[SerializeField] private Button _btnSetting;
    //[SerializeField] private Button _btnRestart;

    //[SerializeField] private RectTransform _settingPanelRect;
    //[SerializeField] private float _displaySettingPanelY;
    //[SerializeField] private float _hideSettingPanelY;
    //[SerializeField] private GameObject _hintCoinPanel;

    //private bool _isSettingPanelShow;
}