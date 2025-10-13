using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using System.Collections;
using System.Collections.Generic;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;
using UnityEngine.UI;
using _02.Scripts.InGame;

public class OptionDialog : Dialog
{
    [SerializeField] private SwitchToggle toggleSounds;
    [SerializeField] private ToggleButton toggleShake;

    [SerializeField] protected Button closeBtn;
    [SerializeField] protected Button closeBtn2;
    [SerializeField] private Button GiveButton;
    [SerializeField] protected Button levelBtn;
    [SerializeField] protected Button languageBtn;
    [SerializeField] protected Button termOfUseBtn;
    [SerializeField] protected Button privacyPolicyBtn;
    [SerializeField] public Text showText;

    [SerializeField] private Image maskBG;
    [SerializeField] private List<GameObject> butttons;
    private bool isInit;

    public void Init()
    {
        showText.text = LocalizationManager.Instance.Model.CurrentLanguage.LanguageName;
        toggleSounds.IsOn = AudioManager.Instance.Model.IsSoundOpen;
        toggleShake.IsOn = VibrationManager.Instance.IsOpen;
        isInit = true;
     

    }

    public override void ShowDialog()
    {
        base.ShowDialog();
        bool IsShow = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "InGame";
        foreach (var temp in butttons)
        {
            temp.gameObject.SetActive(!IsShow);
        }
        if (!isInit)
            Init();
    }

    private void OnEnable()
    {
        //maskBG.sprite = InGameManager.Instance.bg.sprite;
        toggleSounds.OnValueChange += ToggleSoundChange;
        toggleShake.OnChange += ToggleShakeChange;
        termOfUseBtn.onClick.AddListener(ClickTermsButton);
        privacyPolicyBtn.onClick.AddListener(ClickPrivacyButton);
        closeBtn.onClick.AddListener(CloseMenuBtn);
        closeBtn2.onClick.AddListener(CloseMenuBtn);
        GiveButton.onClick.AddListener(GiveUp);
        levelBtn.onClick.AddListener(ClickLevelBtn);
        languageBtn.onClick.AddListener(ClicklanguageBtn);
    }

    private void OnDisable()
    {
        toggleSounds.OnValueChange -= ToggleSoundChange;
        toggleShake.OnChange -= ToggleShakeChange;
        GiveButton.onClick.RemoveListener(GiveUp);
        closeBtn.onClick.RemoveListener(CloseMenuBtn);
        closeBtn2.onClick.RemoveListener(CloseMenuBtn);
        levelBtn.onClick.RemoveListener(ClickLevelBtn);
        languageBtn.onClick.RemoveListener(ClicklanguageBtn);
        termOfUseBtn.onClick.RemoveListener(ClickTermsButton);
        privacyPolicyBtn.onClick.RemoveListener(ClickPrivacyButton);
    }

    /// <summary>
    /// 按下关闭选项面板按钮
    /// </summary>
    private void CloseMenuBtn()
    {
        DialogManager.Instance.GetDialog<OptionDialog>().CloseDialog();

        AudioClipHelper.Instance.PlayButtonTap();
        VibrationManager.Instance.SelectedBlockImpact();
        //OptionObj.SetActive(false);
    }
    private void GiveUp()
    {
        DialogManager.Instance.GetDialog<OptionDialog>().CloseDialog();

        TransitionManager.Instance.Transition(0.5f, () => { SceneManager.LoadScene("InGame"); },
            0.5f);
    }
    public void ClickLevelBtn()
    {
        DialogManager.Instance.GetDialog<LevelUIDialog>().ShowDialog();
        DialogManager.Instance.GetDialog<OptionDialog>().CloseDialog();

        AudioClipHelper.Instance.PlayButtonTap();
        VibrationManager.Instance.SelectedBlockImpact();
    }

    /// <summary>
    /// 按下语言按钮
    /// </summary>
    private void ClicklanguageBtn()
    {
        DialogManager.Instance.GetDialog<LanguageOptionsDialog>().ShowDialog();
        DialogManager.Instance.GetDialog<OptionDialog>().CloseDialog();

        AudioClipHelper.Instance.PlayButtonTap();
        VibrationManager.Instance.SelectedBlockImpact();
    }

    private void ClickTermsButton()
    {
        var dialog = DialogManager.Instance.GetDialog<TermPrivacyDialog>();
        dialog.aboutToShowType = ShowType.TermOfUse;
        dialog.ShowDialog();
        DialogManager.Instance.GetDialog<OptionDialog>().CloseDialog();
        AudioClipHelper.Instance.PlayButtonTap();
    }

    private void ClickPrivacyButton()
    {
        var dialog = DialogManager.Instance.GetDialog<TermPrivacyDialog>();
        dialog.aboutToShowType = ShowType.PrivacyPolicy;
        dialog.ShowDialog();
        DialogManager.Instance.GetDialog<OptionDialog>().CloseDialog();
        AudioClipHelper.Instance.PlayButtonTap();
    }

    private void ToggleSoundChange(bool isOn)
    {
        AudioManager.Instance.Model.IsSoundOpen = isOn;
        AudioManager.Instance.Model.IsBgmOpen = isOn;

        AudioClipHelper.Instance.PlayButtonTap();
        VibrationManager.Instance.SelectedBlockImpact();
    }

    private void ToggleShakeChange(bool isOn, ToggleButton arg2)
    {
        AudioClipHelper.Instance.PlayButtonTap();
        VibrationManager.Instance.SetIsOpen(isOn);
        VibrationManager.Instance.SelectedBlockImpact();
    }

    private int numClicks = 0;

    public void SettingBtn()
    {
        if (LogSwitcher.Instance.Open)
        {
            numClicks++;
            if (numClicks >= 5)
            {
                SoyProfile.DelaySet(SoyProfileConst.NormalLevel, SoyProfileConst.NormalLevel_Default, 1);
                SoyProfile.DelaySet(SoyProfileConst.HaveCoin, SoyProfileConst.HaveCoinDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.ForeverCard, SoyProfileConst.ForeverCardDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.MonthCard, SoyProfileConst.MonthCardDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.ADRemoved, SoyProfileConst.ADRemoved_Default, 1);
                SoyProfile.DelaySet(SoyProfileConst.PlateProgress, SoyProfileConst.PlateProgressDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.SigninProgress, SoyProfileConst.SigninProgressDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.FirstChargeProgress, SoyProfileConst.FirstChargeProgressDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.LimitTimeProgress, SoyProfileConst.LimitTimeProgressDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.CoinShopProgress, SoyProfileConst.CoinShopProgressDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.PurchaseTime, SoyProfileConst.PurchaseTimeDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.OnlineTimeProgress, SoyProfileConst.OnlineTimeProgressDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.GameToolProgress, SoyProfileConst.GameToolProgressDefault, 1);
                SoyProfile.DelaySet(SoyProfileConst.LevelChallengeProgress, SoyProfileConst.LevelChallengeProgressDefault, 1);
                Game.Instance.LevelModel.MaxUnlockLevel.Value = 1;
                Game.Instance.Model.IsAdRemoved.Value = false;
                numClicks = 0;
                FloatingWindow.Instance.Show("清除成功");
            }
        }
    }
}