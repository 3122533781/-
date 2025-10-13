using ProjectSpace.BubbleMatch.Scripts.SubSystem.ActivitySystem;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.UI;

public class RatingStarDialog : Dialog
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
        if (Game.Instance.GetSystem<RatingSystem>().Model.CurrentRatingStar == 0)
        {
            _lastStar.ShowBlink();
        }
        else
        {
            RateHandle();
        }
    }
    

    private void RateHandle()
    {
        Game.Instance.GetSystem<RatingSystem>().CompletedRating();

        if (Game.Instance.GetSystem<RatingSystem>().Model.CurrentRatingStar < 4)
        {
//            DialogManager.Instance.GetDialog<ContactUsDialog>().Activate();
        }
        else
        {
#if UNITY_IOS
            HandleIOSRating();
#else
            HandleAndroidRating();
#endif
        }

        Deactivate();
    }

    private void HandleAndroidRating()
    {
//        int ratingStar = App.Instance.GetSystem<RatingSystem>().Model.CurrentRatingStar;

        Application.OpenURL(GameConfig.Instance.GooglePlayURL);

//        DialogManager.Instance.GetDialog<YesNoDialog>()
//            .Show(LocalizationManager.Instance.GetTextByTag(LocalizationConst.THANK_RATING),
//                () => {  /*发邮件*/ Debug.Log("Send feedback email!"); EmailHelpler.SendFeedbackEmail(); },
//                () => { });
    }
#if UNITY_IOS
    private void HandleIOSRating()
    {
        UnityEngine.iOS.Device.RequestStoreReview();
    }
#endif


    [SerializeField] private RatingClickableStar _lastStar = null;
    [SerializeField] private Button _rateButton = null;

}