using Spine.Unity;
using UnityEngine;

public class CoinFlyBase : MonoBehaviour
{
    [SerializeField] protected Canvas canvas;

    //    [SerializeField] protected SkeletonGraphic skeleton;
    [SerializeField] protected Transform flash;

    [SerializeField] protected string anim = "coin";

    protected void Awake()
    {
        canvas.sortingLayerName = "UI";
        Stop();
    }

    public virtual RectTransform RectT
    {
        get { return GetComponent<RectTransform>(); }
    }

    public virtual void Play(float quicken = 1)
    {
        if (flash) flash.SetActive(true);
    }

    public virtual void Stop()
    {
        if (flash) flash.SetActive(false);
    }
}