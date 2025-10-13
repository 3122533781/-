using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SwapImage : MonoBehaviour
{
    public Sprite enbaleImg;
    public Sprite enbaleAdImg;
    public Sprite disableImg;

    Image target = null;

    IapStatus state = IapStatus.SoldAll;

    private void Start()
    {
        switch (state)
        {
            case IapStatus.Free:
                GetTarget().sprite = enbaleImg;
                break;
            case IapStatus.CanWatch:
                GetTarget().sprite = enbaleAdImg;
                break;
            case IapStatus.SoldAll:
                GetTarget().sprite = disableImg;
                break;
        }
    }

    Image GetTarget()
    {
        if (target == null)
        {
            target = GetComponent<Image>();
        }
        return target;
    }

    /// <summary>
    /// …Ë÷√
    /// </summary>
    /// <param name="s"></param>
    public void Set(IapStatus s)
    {
        state = s;
        if (state == IapStatus.Free)
        {
            GetTarget().sprite = enbaleImg;
        }
        else if (state == IapStatus.CanWatch)
        {
            GetTarget().sprite = enbaleAdImg;
        }
        else
        {
            GetTarget().sprite = disableImg;
        }
    }

    /// <summary>
    /// «–ªª
    /// </summary>
    public void Swap()
    {
        // state = !state;
        // if (state)
        // {
        //     GetTarget().sprite = enbaleImg;
        // }
        // else
        // {
        //     GetTarget().sprite = disableImg;
        // } 
    }

    // public bool GetState()
    // {
    //     // return state;
    // }

}
