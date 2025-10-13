using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;

public class DebugAD : IDebugPage
{
    public string Title
    {
        get { return "AD"; }
    }

    public void Draw()
    {
        if (GUILayout.Button("Set ad remove false"))
        {
            SoyProfile.Set(SoyProfileConst.ADRemoved, false);
        }

        if (GUILayout.Button("ShowBanner"))
        {
            // ADMudule.ShowBanner();
        }

        if (GUILayout.Button("HideBanner"))
        {
            // ADMudule.HideBanner();
        }

        if (GUILayout.Button("ShowInterstitial"))
        {
            // ADMudule.ShowInterstitialAds("test", null);
        }

        if (GUILayout.Button("ShowRewarded"))
        {
            // ADMudule.ShowRewardedAd("test", null);
        }

        if (GUILayout.Button("Show waiting ad dialog"))
        {
            DialogManager.Instance.ShowDialog(DialogName.ADLoadingDialog);
        }

        
    }
}