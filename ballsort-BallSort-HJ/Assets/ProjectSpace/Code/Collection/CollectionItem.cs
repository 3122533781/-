
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
        Debug.Log("·��Ϊ" + $"Kinds/{collectiondata.name}");
        if(sprite==null)
        {
            Debug.Log("û���ҵ�ͼƬ");
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
