using NPOI.SS.Formula.Functions;
using UnityEngine;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class SelectToggle : MonoBehaviour
{
    //??????
    private Image icon => transform.Find("Icon").GetComponent<Image>();

    //?????
    private Image bg => GetComponent<Image>();
    //???
    private Text text => transform.Find("Text").GetComponent<Text>();

    //?????????§Ý?????
    public Sprite[] selectOnBGSprites;
    //???????????§Ý?????
    public Sprite[] selectOnIconSprites;



    public void ListenInFunction(bool isOn)
    {
        if (isOn)
        {
            SelectOn();
        }
        else
        {
            SelectOff();
        }
    }

    void SelectOn()
    {
        if (icon)
            icon.sprite = selectOnIconSprites[0];
        if (bg)
            bg.sprite = selectOnBGSprites[0];
        text.color = Color.white;
    }

    void SelectOff()
    {
        if (icon)
            icon.sprite = selectOnIconSprites[1];
        if (bg)
            bg.sprite = selectOnBGSprites[1];
        text.color = Color.black;
    }
}
