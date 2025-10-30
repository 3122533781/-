using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
public class CollectionDialog : Dialog
{
    public Transform collectionParent;
    public Transform collectionParent2;
    public GameObject collectionPrefab;
    [SerializeField] private RectTransform trans;
    [SerializeField] private List<GameObject> Objs;
    //  public Animator moreAnimator;
    public ScrollRect scrollRect;
    private bool isExtended = false;
    [SerializeField] private List<Button> buttons;
    private List<CollectionGoodsData> collectionGoods = new List<CollectionGoodsData>();
    private Dictionary<CollectionGoodsData, GameObject> dataToUI = new Dictionary<CollectionGoodsData, GameObject>();
    private Dictionary<CollectionGoodsData, GameObject> dataToUI2 = new Dictionary<CollectionGoodsData, GameObject>();
    public AutoFlip autoFlip;
    [SerializeField] private SkeletonAnimation OpenAnim;
    private int tempBtn=0;


    public void Show()
    {
        base.ShowDialog();
    }

    private void OnEnable()
    {
        OpenAnim.AnimationState.SetAnimation(0, "animation", false);
        InitCollections();
        StartCoroutine(InitCollectionsAfterFrame());
    }

    private void OnDisable()
    {
        //moreAnimator.enabled = false;
        // Keep original function: Disable scroll rect when dialog is closed
        scrollRect.enabled = false;

        // Clear all generated UI and data to avoid duplication next time
        ClearAllCollectionUI();
        collectionGoods.Clear();
        dataToUI.Clear();
        dataToUI2.Clear();
    }
    private void ShowBook()
    {
        foreach (var item in Objs)
        {
            item.SetActive(true);
        }
    }
    private void Start()
    {
        // Keep original function: Register button click events (only once)
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => BtnChoose(index));
        }

        // Keep original function: Generate UI on first start
        InitCollections();
    }

    private void InitCollections()
    {
        // Clear old UI and data first to prevent duplication
        ClearAllCollectionUI();
        collectionGoods.Clear();
        dataToUI.Clear();
        dataToUI2.Clear();

        // Keep original function: Get all collection configs and filter owned ones
        var collections = CollectionConfig.Instance.All;
        foreach (var item in collections)
        {
            if (item.IsHave.Value)
                collectionGoods.Add(item);
        }

        // Keep original function: Instantiate UI for owned collections
        foreach (var item in collectionGoods)
        {
            var obj = Instantiate(collectionPrefab, collectionParent);
            var CollectionPrefab = obj.GetComponent<CollectionItem>();
            CollectionPrefab.Initialize(item);
            dataToUI[item] = obj;
        }
        foreach (var item in collectionGoods)
        {
            var obj = Instantiate(collectionPrefab, collectionParent2);
            var CollectionPrefab = obj.GetComponent<CollectionItem>();
            CollectionPrefab.Initialize(item);
            dataToUI2[item] = obj;
        }
    }

    // New: Helper method to clear all generated collection UI instances
    private void ClearAllCollectionUI()
    {
        // 清理 collectionParent 下的UI
        if (collectionParent != null)
        {
            for (int i = collectionParent.childCount - 1; i >= 0; i--)
            {
                Transform child = collectionParent.GetChild(i);
                if (child.gameObject != collectionPrefab) // 跳过预制体模板
                    Destroy(child.gameObject);
            }
        }

        // 新增：清理 collectionParent2 下的UI（关键！）
        if (collectionParent2 != null)
        {
            for (int i = collectionParent2.childCount - 1; i >= 0; i--)
            {
                Transform child = collectionParent2.GetChild(i);
                if (child.gameObject != collectionPrefab) // 跳过预制体模板
                    Destroy(child.gameObject);
            }
        }
    }

    // Keep original function: Animation logic for "More" button
    public void OnMoreButtonsClicked()
    {
        //if (moreAnimator.enabled == false)
        //{
        //    moreAnimator.enabled = true;
        //}

        //moreAnimator.Rebind();
        //if (isExtended == false)
        //{
        //    moreAnimator.Play("MorePanel");
        //}
        //else
        //{
        //    moreAnimator.StartPlayback();
        //}
        //isExtended = !isExtended;
    }

    // Keep original function: Enable scroll rect after UI is ready
    private IEnumerator InitCollectionsAfterFrame()
    {
        yield return null; // Wait 1 frame to ensure UI components are ready
        scrollRect.enabled = true;
    }

    // Keep original function: Button click logic for collection filtering


    private void BtnChoose2(int temp)
    {
        switch (temp)
        {


            case 0: // Filter: Quality _N
                foreach (var item in dataToUI2)
                {
                    if (item.Key.quality == Quality._N)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 1: // Filter: Quality _R
                foreach (var item in dataToUI2)
                {
                    if (item.Key.quality == Quality._R)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 2: // Filter: Quality _SR
                foreach (var item in dataToUI2)
                {
                    if (item.Key.quality == Quality._SR)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 3: // Filter: Quality _SSR
                foreach (var item in dataToUI2)
                {
                    if (item.Key.quality == Quality._SSR)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 4: // Filter: Quality _UR
                foreach (var item in dataToUI2)
                {
                    if (item.Key.quality == Quality._UR)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            default: // Fallback: Show all if index is unknown
                foreach (var item in dataToUI2.Values)
                {
                    item.SetActive(true);
                }
                Debug.LogWarning($"Unknown button index: {temp}, showing all collections by default");
                break;
        }
    }
    private void BtnChoose(int temp)
    {
        autoFlip.FlipRightPage(() =>
        {
            Debug.Log("翻页动画完成");
            BtnChoose2(tempBtn); // 翻页动画完成后执行
        });
        switch (temp)
        {
           

            case 0: // Filter: Quality _N
                tempBtn = temp;
                foreach (var item in dataToUI)
                {
                    if (item.Key.quality == Quality._N)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 1: // Filter: Quality _R
                tempBtn = temp;
                foreach (var item in dataToUI)
                {
                    if (item.Key.quality == Quality._R)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 2: // Filter: Quality _SR
                tempBtn = temp;
                foreach (var item in dataToUI)
                {
                    if (item.Key.quality == Quality._SR)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 3: // Filter: Quality _SSR
                tempBtn = temp;
                foreach (var item in dataToUI)
                {
                    if (item.Key.quality == Quality._SSR)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 4: // Filter: Quality _UR
                tempBtn = temp;
                foreach (var item in dataToUI)
                {
                    if (item.Key.quality == Quality._UR)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            default: // Fallback: Show all if index is unknown
                foreach (var item in dataToUI.Values)
                {
                    item.SetActive(true);
                }
                Debug.LogWarning($"Unknown button index: {temp}, showing all collections by default");
                break;
        }
    }
}