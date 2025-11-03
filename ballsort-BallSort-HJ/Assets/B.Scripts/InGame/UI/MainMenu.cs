using DG.Tweening;
using ProjectSpace.BubbleMatch.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ProjectSpace.Lei31Utils.Scripts.Framework.ElementKit;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private float collectFadeDuration = 0.5f;
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
    [SerializeField] private Button _btnLevel;
    [SerializeField] private Text _textLevel;
    [SerializeField] private Text _textCoin;
    [SerializeField] private List<GameObject> gameStages = new List<GameObject>();
    [SerializeField] private GameObject _playbtn;
    [SerializeField] private GameObject _balls;
    [SerializeField] private GameObject _line;
    [SerializeField] private GameObject _circle;
    [SerializeField] private Image CollectImage;

    private List<CollectionGoodsData> _ownedCollects = new List<CollectionGoodsData>();
    private int _currentCollectIndex = 0;
    private Coroutine _collectLoopCoroutine;
    // 新增：记录当前显示的藏品ID（用于判断是否重复）
    private int _currentDisplayedCollectId = -1;

    public void GotoBallSort()
    {
        TransitionManager.Instance.Transition(0.5f, () => { SceneManager.LoadScene("InGameScenario"); }, 0.5f);
    }

    private void ClickSetting()
    {
        DialogManager.Instance.GetDialog<OptionDialog>().ShowDialog();
    }

    public void OpenSkinMainUI()
    {
    }

    private void Start()
    {
        SpriteManager.Instance.InitSkin();
        InitOwnedCollects();

        if (_ownedCollects.Count > 0)
        {
            _collectLoopCoroutine = StartCoroutine(CollectLoopCoroutine());
        }
        else
        {
            // 无藏品时隐藏图片
            CollectImage.gameObject.SetActive(false);
        }

        foreach (var collect in CollectionConfig.Instance.All)
        {
            // collect.IsHave.OnValueChange += OnCollectUnlocked;
        }
    }

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
        _btnLevel.onClick.AddListener(ClickLevelBtn);
        Refresh();
        _btnUseHint.onClick.AddListener(ClickHint);
        _btnGetCoin.onClick.AddListener(ClickGetCoin);
        _btnGetPower.onClick.AddListener(GetPower);
        Playbtn.onClick.AddListener(GotoBallSort);
        UIEvents.OnDressUpDialogOpened += OnDressUpDialogOpened;
        UIEvents.OnDressUpDialogClosed += OnDressUpDialogClosed;
    }

    private void OnDisable()
    {
        if (_collectLoopCoroutine != null)
        {
            StopCoroutine(_collectLoopCoroutine);
            _collectLoopCoroutine = null;
        }

        _btnUseHint.onClick.RemoveListener(ClickHint);
        _btnGetCoin.onClick.RemoveListener(ClickGetCoin);
        _btnGetPower.onClick.RemoveListener(GetPower);
        _btnLevel.onClick.RemoveListener(ClickLevelBtn);
        UIEvents.OnDressUpDialogOpened -= OnDressUpDialogOpened;
        UIEvents.OnDressUpDialogClosed -= OnDressUpDialogClosed;
    }

    private void OnCollectUnlocked(bool temp, bool isNowOwned)
    {
        if (isNowOwned)
        {
            InitOwnedCollects();

            if (_collectLoopCoroutine == null && _ownedCollects.Count > 0)
            {
                var inGame = SceneElementManager.Instance.Resolve<InGame>();
                if (inGame != null)
                    _collectLoopCoroutine = StartCoroutine(CollectLoopCoroutine());
            }
        }
    }

    private void InitOwnedCollects()
    {
        _ownedCollects.Clear();
        _currentDisplayedCollectId = -1; // 重置当前显示ID

        var allCollects = CollectionConfig.Instance.All;
        if (allCollects != null && allCollects.Count() > 0)
        {
            foreach (var collect in allCollects)
            {
                if (collect.IsHave.Value)
                {
                    _ownedCollects.Add(collect);
                }
            }
        }

        _currentCollectIndex = 0;
        Debug.Log($"已拥有藏品数量：{_ownedCollects.Count}");

        // 处理显示状态：有藏品则显示，无则隐藏
        if (_ownedCollects.Count > 0 && CollectImage != null)
        {
            CollectImage.gameObject.SetActive(true);
            Color tempColor = CollectImage.color;
            tempColor.a = 0f;
            CollectImage.color = tempColor;
            TurnCollections(_currentCollectIndex);
        }
        else if (CollectImage != null)
        {
            CollectImage.gameObject.SetActive(false);
        }
    }

    private void OnDressUpDialogOpened()
    {
        if (_balls != null)
        {
            _balls.SetActive(false);
            Debug.Log("_balls 已隐藏");
        }
    }

    private void ShareDialog()
    {
    }

    private void OnDressUpDialogClosed()
    {
    }

    private void ClickHint()
    {
    }

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
    }

    public void GetPower()
    {
    }

    public void GetCoin()
    {
        _textCoin.text = $"{Game.Instance.CurrencyModel.CoinNum}";
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
            if (stage != null)
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
                Game.Instance.CurrencyModel.RewardCoin(20);
            }
        });
    }

    private string GetDateString(System.DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }

    public void ClickLevelBtn()
    {
        DialogManager.Instance.GetDialog<LevelUIDialog>().ShowDialog();
        AudioClipHelper.Instance.PlayButtonTap();
        VibrationManager.Instance.SelectedBlockImpact();
    }

    private void TurnCollections(int index)
    {
        if (_ownedCollects == null || _ownedCollects.Count == 0 || index < 0 || index >= _ownedCollects.Count || CollectImage == null)
        {
            CollectImage.gameObject.SetActive(false);
            Debug.LogWarning("无已拥有的藏品可显示或CollectImage未赋值！");
            return;
        }

        // 获取目标藏品并判断是否与当前显示重复
        CollectionGoodsData targetCollect = _ownedCollects[index];
        if (targetCollect.id == _currentDisplayedCollectId)
        {
            // 藏品重复，不执行渐变，直接返回
            return;
        }

        CollectImage.DOKill();
        CollectImage.gameObject.SetActive(true);

        // 藏品不同，执行渐变切换
        CollectImage.DOFade(0f, collectFadeDuration).OnComplete(() =>
        {
            Sprite sprite = Resources.Load<Sprite>($"Kinds/{targetCollect.name}");
            if (sprite != null)
            {
                CollectImage.sprite = sprite;
                Debug.Log($"当前显示藏品：{targetCollect.name}（ID：{targetCollect.id}）");
            }
            else
            {
                Debug.LogError($"未找到藏品图片：Kinds/{targetCollect.name}");
            }

            if (targetCollect != null)
            {
                CollectionPositionData posData = CollectionPositionConfig.Instance.GetPositionData(targetCollect.id);
                if (posData != null)
                {
                    CollectImage.rectTransform.anchoredPosition = new Vector2(posData.x, posData.y);
                }
                else
                {
                    CollectImage.rectTransform.anchoredPosition = Vector2.zero;
                }
            }

            // 更新当前显示藏品ID
            _currentDisplayedCollectId = targetCollect.id;
            CollectImage.DOFade(1f, collectFadeDuration);
        });
    }

    private IEnumerator CollectLoopCoroutine()
    {
        while (true)
        {
            // 循环中检查：若藏品列表为空，隐藏图片并停止协程
            if (_ownedCollects.Count == 0)
            {
                CollectImage.gameObject.SetActive(false);
                _currentDisplayedCollectId = -1;
                yield break;
            }

            TurnCollections(_currentCollectIndex);

            yield return new WaitForSeconds(5f);

            _currentCollectIndex++;
            if (_currentCollectIndex >= _ownedCollects.Count)
            {
                _currentCollectIndex = 0;
            }
        }
    }

    private void Refresh()
    {
        GetCoin();
        Game.Instance.LevelModel.PassLevelTemp = Game.Instance.LevelModel.PassLevelNumber.Value;
        Game.Instance.LevelModel.EnterLevelSecond = false;
        Game.Instance.LevelModel.TheSmallLevelID = 0;
    }
}