
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    [SerializeField] private Image Icon;
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
        //Icon.sprite=
    }
    public void ShowItem()
    {
        DialogManager.Instance.GetDialog<CollectionItemDialog>().InitDialog(collectiondataBase);

    }
}
