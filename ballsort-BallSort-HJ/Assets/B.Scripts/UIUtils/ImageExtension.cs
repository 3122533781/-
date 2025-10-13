using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension
{
    //设置Image 为原图比例
    public static void SetNativeProportion(this Image origin, bool fixedWidth)
    {
        RectTransform originRect = origin.GetComponent<RectTransform>();
        Vector2 originSize = originRect.sizeDelta;
        Vector2 spriteSize = origin.sprite.rect.size;

        if (fixedWidth)
        {
            float setHeight = spriteSize.y / spriteSize.x * originSize.x;
            originRect.sizeDelta = new Vector2(originSize.x, setHeight);
        }
        else
        {
            float setWidth = spriteSize.x / spriteSize.y * originSize.y;
            originRect.sizeDelta = new Vector2(setWidth, originSize.y);
        }
    }
}