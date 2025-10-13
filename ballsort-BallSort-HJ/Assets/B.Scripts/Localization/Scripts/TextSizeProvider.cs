using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextSizeProvider
{
    public static Vector2 ChineseSize = new Vector2(1f, 1.3f);

    //failed
    public static Vector2Int GetUnitTextSizeDy(string context, Font font)
    {
        char[] array = context.ToCharArray();
        font.RequestCharactersInTexture(context, 1, FontStyle.Normal);
        int sumWidth = 0;
        int maxHeight = int.MinValue;
        for (int i = 0; i < array.Length; i++)
        {
            CharacterInfo info = new CharacterInfo();
            bool isSuccess = font.GetCharacterInfo(array[i], out info);
            if (!isSuccess)
                Debug.LogError("Get CharacterInfo failed");
            sumWidth += info.glyphWidth;
            if (maxHeight < info.glyphHeight)
            {
                maxHeight = info.glyphHeight;
            }
        }

        return new Vector2Int(sumWidth, maxHeight);
    }

    //failed
    public static Vector2 GetUnitTextSize(string context, LanguageFontData data)
    {
        //chinese 
        if (ContainChinese(context))
        {
            Debug.Log("is chinese ");
            return ChineseSize;
        }

        char[] array = context.ToCharArray();

        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] textBuff = encoding.GetBytes(array);

        for (int i = 0; i < textBuff.Length; i++)
        {
            Debug.Log(textBuff[i]);
        }

        float sumWidth = 0;
        float maxHeight = int.MinValue;
        for (int i = 0; i < array.Length; i++)
        {
            Vector2 charSize = GetCharSize(array[0], data);
            sumWidth += charSize.x;
            if (maxHeight < charSize.y)
            {
                maxHeight = charSize.y;
            }
        }

        return new Vector2(sumWidth, maxHeight);
    }

    private static bool ContainChinese(string input)
    {
        string pattern = "[\u4e00-\u9fbb]";
        return Regex.IsMatch(input, pattern);
    }

    private static Vector2 GetCharSize(char c, LanguageFontData data)
    {
        switch (c)
        {
            case ' ':
                return new Vector2();
            default:
                return new Vector2(data.FontTextUnitWidth, data.FontTextUnitHeight);
        }
    }
}