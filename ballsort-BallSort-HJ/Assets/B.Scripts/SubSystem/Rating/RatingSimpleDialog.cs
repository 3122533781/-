using ProjectSpace.BubbleMatch.Scripts.SubSystem.ActivitySystem;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;

public class RatingSimpleDialog : Dialog
{
    private void OnEnable()
    {
        //        _lastStar.ShowBlink();
        Game.Instance.GetSystem<RatingSystem>().Model.CurrentRatingStar = 0;
        _rateButton.onClick.AddListener(OnRateClick);
    }

    private void OnDisable()
    {
        _rateButton.onClick.RemoveListener(OnRateClick);
    }

    private void OnRateClick()
    {
        Game.Instance.GetSystem<RatingSystem>().CompletedRating();
#if UNITY_IOS
        HandleIOSRating();
#else
        HandleAndroidRating();
#endif
    }

    private void HandleAndroidRating()
    {
        Application.OpenURL(GameConfig.Instance.GooglePlayURL);
        Deactivate();
        //        DialogManager.Instance.GetDialog<YesNoDialog>()
        //            .Show(LocalizationManager.Instance.GetTextByTag(LocalizationConst.THANK_RATING),
        //                () => {  /*发邮件*/ Debug.Log("Send feedback email!"); EmailHelpler.SendFeedbackEmail(); },
        //                () => { });
    }

#if UNITY_IOS

    private void HandleIOSRating()
    {
        UnityEngine.iOS.Device.RequestStoreReview();
        Deactivate();
    }

#endif

    [SerializeField] private Button _rateButton = null;
}