using ProjectSpace.BubbleMatch.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SetBallBtn : MonoBehaviour
{
    public Image icon;

    public GameObject selectedObj;

    public GameObject lockObj;

    private bool isLock;

    public Toggle toggle;

    public int id;

    public bool IsLock
    {
        get => isLock;
        set
        {
            isLock = value;
          
        }
    }

    //初始化按钮，设置按钮图片
    public void Init(Sprite icon, int id)
    {
        this.id = id;
        this.icon.sprite = icon;
        if (PlayerPrefs.HasKey("BallSkin" + id) || id == 0)
            IsLock = false;
        else
            IsLock = true;
        toggle.group = transform.parent.GetComponent<UnityEngine.UI.ToggleGroup>();
    }

    // Start is called before the first frame update
    public void SetBallSkin(bool isOn)
    {
        if (isOn)
        {
            selectedObj.SetActive(true);
            PlayerPrefs.SetInt("ClickBallSkin", id);
        }
        else
        {
            selectedObj.SetActive(false);
        }
    }

    // Start is called before the first frame update
    public void UnlockBallSkin()
    {
        lockObj.SetActive(false);
        toggle.enabled = true;
    }

    // Start is called before the first frame update
    public void lockBallSkin()
    {
        lockObj.SetActive(true);
        toggle.enabled = false;
    }
}