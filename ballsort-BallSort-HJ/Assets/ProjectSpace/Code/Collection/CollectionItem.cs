
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour
{
    [SerializeField] private Image Icon;

    public void Initialize(CollectionGoodsData collectiondata)
    {
        Sprite sprite = Resources.Load<Sprite>($"Kinds/{collectiondata.name}");
        Debug.Log("·��Ϊ" + $"Kinds/{collectiondata.name}");
        if(sprite==null)
        {
            Debug.Log("û���ҵ�ͼƬ");
        }
        Icon.sprite = sprite;
        //Icon.sprite=
    }
}
