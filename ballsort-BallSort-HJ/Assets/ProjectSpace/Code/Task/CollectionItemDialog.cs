using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
public class CollectionItemDialog : Dialog
{
    [SerializeField] private Image Icon;
    public void InitDialog(CollectionGoodsData collectiondata)
    {
        base.ShowDialog();
        Sprite sprite = Resources.Load<Sprite>($"Kinds/{collectiondata.name}");
        Debug.Log("·��Ϊ" + $"Kinds/{collectiondata.name}");
        if (sprite == null)
        {
            Debug.Log("û���ҵ�ͼƬ");
        }
        Icon.sprite = sprite;

    }



}
