using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionDialog : Dialog
{
    public Transform collectionParent;
    public GameObject collectionPrefab;
    [SerializeField] private RectTransform trans;
    public Animator moreAnimator;
    public ScrollRect scrollRect;
    private bool isExtended = false;
    [SerializeField] private List<Button> buttons;
    private List<CollectionGoodsData> collectionGoods = new List<CollectionGoodsData>();
    private Dictionary<CollectionGoodsData, GameObject> dataToUI = new Dictionary<CollectionGoodsData, GameObject>();

    public void Show()
    {
        base.ShowDialog();
    }

    private void OnEnable()
    {
        // Re-initialize to generate new UI every time the dialog is opened
        InitCollections();
        StartCoroutine(InitCollectionsAfterFrame());
    }

    private void OnDisable()
    {
        moreAnimator.enabled = false;
        // Keep original function: Disable scroll rect when dialog is closed
        scrollRect.enabled = false;

        // Clear all generated UI and data to avoid duplication next time
        ClearAllCollectionUI();
        collectionGoods.Clear();
        dataToUI.Clear();
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
    }

    // New: Helper method to clear all generated collection UI instances
    private void ClearAllCollectionUI()
    {
        if (collectionParent == null) return;

        // Traverse children in reverse to avoid index confusion when deleting
        for (int i = collectionParent.childCount - 1; i >= 0; i--)
        {
            Transform child = collectionParent.GetChild(i);
            // Skip prefab template to avoid accidental deletion
            if (child.gameObject != collectionPrefab)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Keep original function: Animation logic for "More" button
    public void OnMoreButtonsClicked()
    {
        if (moreAnimator.enabled == false)
        {
            moreAnimator.enabled = true;
        }

        moreAnimator.Rebind();
        if (isExtended == false)
        {
            moreAnimator.Play("MorePanel");
        }
        else
        {
            moreAnimator.StartPlayback();
        }
        isExtended = !isExtended;
    }

    // Keep original function: Enable scroll rect after UI is ready
    private IEnumerator InitCollectionsAfterFrame()
    {
        yield return null; // Wait 1 frame to ensure UI components are ready
        scrollRect.enabled = true;
    }

    // Keep original function: Button click logic for collection filtering
    private void BtnChoose(int temp)
    {
        // 1. Control button selected state (keep original logic)
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].TryGetComponent<Image>(out Image btnImage))
            {
                btnImage.enabled = (i == temp);
            }
            else
            {
                Debug.LogWarning($"Button {i} has no Image component, skipping");
            }
        }

        // 2. Collection filtering logic based on button index
        switch (temp)
        {
            case 0: // Show all collections
                foreach (var item in dataToUI.Values)
                {
                    item.SetActive(true);
                }
                break;

            case 1: // Filter: Kitchenware
                foreach (var item in dataToUI)
                {
                    if (item.Key.kind == Kings.Kitchenware)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 2: // Filter: Condiments
                foreach (var item in dataToUI)
                {
                    if (item.Key.kind == Kings.Condiments)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 3: // Filter: Ingredients
                foreach (var item in dataToUI)
                {
                    if (item.Key.kind == Kings.Ingredients)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 4: // Filter: Dishes
                foreach (var item in dataToUI)
                {
                    if (item.Key.kind == Kings.Dishes)
                        item.Value.SetActive(true);
                    else
                        item.Value.SetActive(false);
                }
                break;

            case 5: // Filter: Beverages
                foreach (var item in dataToUI)
                {
                    if (item.Key.kind == Kings.Beverages)
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