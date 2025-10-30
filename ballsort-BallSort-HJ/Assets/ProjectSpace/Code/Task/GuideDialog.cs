using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using Spine;
using Spine.Unity;
public class GuideDialog : Dialog
{
    [SerializeField] private List<SkeletonDataAsset> skeletonDataAsset;
    [SerializeField]private SkeletonAnimation guideAnim;
    public void InitDialog(int Temp)
    {
        base.ShowDialog();
        InitAnim(Temp);
        Debug.Log("引导数为" +Temp);
        Game.Instance.LevelModel.GuideGroup.Value += 1;
    }

    private void InitAnim(int temp)
    {
        guideAnim.skeletonDataAsset = skeletonDataAsset[temp-1];
        guideAnim.Initialize(true);
        guideAnim.AnimationState.SetAnimation(0, "animation", true);
    }

}
