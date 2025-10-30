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
        Debug.Log("路径为" + $"Kinds/{collectiondata.name}");
        if (sprite == null)
        {
            Debug.Log("没有找到图片");
        }
        Icon.sprite = sprite;

    }



}
