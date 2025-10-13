using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    //菜单按钮
    public Button menuBtn; 
     

    // Start is called before the first frame update
    void Start()
    {
        menuBtn.onClick.AddListener(() => { ClickMenuBtn(); }); 
    }
 

  

    /// <summary>
    /// 按下菜单按钮
    /// </summary>
    private void ClickMenuBtn()
    {
        DialogManager.Instance.GetDialog<OptionDialog>().ShowDialog();
        //OptionObj.SetActive(true);
    } 

    /// <summary>
    /// 按下关闭选项面板按钮
    /// </summary>
    private void CloseMenuBtn()
    {
        DialogManager.Instance.GetDialog<OptionDialog>().CloseDialog();
        //OptionObj.SetActive(false);
    }  

}
