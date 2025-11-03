
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private Button itemButton;
    [SerializeField] private RectTransform _itemRect;
    private CollectionGoodsData collectiondataBase;
    public void Initialize(CollectionGoodsData collectiondata)
    {
        Sprite sprite = Resources.Load<Sprite>($"Kinds/{collectiondata.name}");
        Debug.Log("路径为" + $"Kinds/{collectiondata.name}");
        if(sprite==null)
        {
            Debug.Log("没有找到图片");
        }
        Icon.sprite = sprite;
        collectiondataBase= collectiondata;
        itemButton.onClick.AddListener(ShowItem);
        //Icon.sprite=
    }
    public void Initialize2(CollectionGoodsData collectiondata)
    {
        Sprite sprite = Resources.Load<Sprite>($"Kinds/WeiHuoDe");
        Debug.Log("路径为" + $"Kinds/{collectiondata.name}");
        if (sprite == null)
        {
            Debug.Log("没有找到图片");
        }
        Icon.sprite = sprite;
        collectiondataBase = collectiondata;


        if (_itemRect != null)
        {
            // 设置锚点位置（x=65，y=35）
            _itemRect.anchoredPosition = new Vector2(65f, 35f);
            // 设置缩放（x=0.5，y=0.5，z=0.5）
            _itemRect.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            Debug.LogWarning("当前物体没有 RectTransform 组件！");
        }
        //Icon.sprite=
    }
    public void ShowItem()
    {
        DialogManager.Instance.GetDialog<CollectionItemDialog>().InitDialog(collectiondataBase);

    }
}
