using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRewardDialog : Dialog
{
    public Image icon;
    private GoodSubType2 type;
    [SerializeField] private Button NorBtn;
    [SerializeField] private Button ADBtn;
    [SerializeField] private Button CollectionBtn;
    [SerializeField] private List<GameObject> games;
    private static readonly HashSet<GoodSubType2> _targetRewardTypes = new()
{
    GoodSubType2.AddPipe,
    GoodSubType2.Remove,
    GoodSubType2.RevocationTool
};

    public static bool hasWatchedAD = false;
    private GoodsData currentGoodsData;
    private System.Action downCallback = null;
    private float rateAmount = 1f;

    private void OnEnable()
    {
        NorBtn.onClick.AddListener(ReceiveFree);
        ADBtn.onClick.AddListener(ReceiveAD);
        CollectionBtn.onClick.AddListener(Collection);
    }

    private void OnDisable()
    {
         NorBtn.onClick.RemoveListener(ReceiveFree);
         ADBtn.onClick.RemoveListener(ReceiveAD);
        CollectionBtn.onClick.RemoveListener(Collection);
    }

    private void ReceiveFree()
    {
        Game.Instance.CurrencyModel.AddNewGoodCount(type);
        this.Deactivate();

    }
    private void ReceiveAD()
    {
        ADMudule.ShowRewardedAd("WatchAd_GetReWard", (isSuccess) =>
        {
            if (isSuccess)
            {
                Game.Instance.CurrencyModel.AddNewGoodCount2(type);
                this.Deactivate();
            }
        });


      
    }
    private void Collection()
    {
        //Game.Instance.CurrencyModel.OpenCollectionPackage(type);
        icon.sprite = Resources.Load<Sprite>($"Kinds/{Game.Instance.CurrencyModel.OpenCollectionPackage(type)}");
       // Debug.Log("Â·¾¶Îª" + Game.Instance.CurrencyModel.OpenCollectionPackage(type));
        games[0].SetActive(false);
        games[1].SetActive(false);
        games[2].SetActive(true);
    }

    public void Init(GoodSubType2 rewardType)
    {
        bool isTargetType = _targetRewardTypes.Contains(rewardType);
        games[0].SetActive(isTargetType);
        games[1].SetActive(!isTargetType);
        Debug.Log("³ö");
        base.ShowDialog();
        icon.sprite = Resources.Load<Sprite>($"Chest/{rewardType}");
        icon.SetNativeSize();
        this.type = rewardType;
    }

   
}