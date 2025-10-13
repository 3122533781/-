using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;
using UnityEngine.UI;

namespace _02.Scripts.InGame.UI
{
    public class InGamePauseUI : ElementUI<global::InGame>
    {
        [SerializeField] private ToggleButton _toggleSounds;
        [SerializeField] private ToggleButton _toggleVibration;
        [SerializeField] private Button _btnRestart;
        [SerializeField] private Button _btnHome;
        [SerializeField] private Button _btnClose;

        private void OnEnable()
        {
            _toggleSounds.IsOn = AudioManager.Instance.Model.IsSoundOpen;
            _toggleSounds.OnChange += ToggleSoundChange;
            _toggleVibration.IsOn = VibrationManager.Instance.IsOpen;
            _toggleVibration.OnChange += ToggleVibrationChange;

            _btnHome.onClick.AddListener(ClickHome);
            _btnRestart.onClick.AddListener(ClickRestart);
            _btnClose.onClick.AddListener(ClickClose);
        }

        private void OnDisable()
        {
            _toggleSounds.OnChange -= ToggleSoundChange;
            _toggleVibration.OnChange -= ToggleVibrationChange;

            _btnHome.onClick.RemoveListener(ClickHome);
            _btnRestart.onClick.RemoveListener(ClickRestart);
            _btnClose.onClick.RemoveListener(ClickClose);
        }

        private void ClickClose()
        {
            Deactivate();
            Context.Continue();
        }

        private void ClickRestart()
        {
            // App.Instance.RestartGame();
            // Context.Restart();
        }

        private void ClickHome()
        {
            Game.Instance.BackHome();
        }

        private void ToggleVibrationChange(bool isOn, ToggleButton arg2)
        {
            VibrationManager.Instance.SetIsOpen(isOn);
        } 

        private void ToggleSoundChange(bool isOn, ToggleButton arg2)
        {
            AudioManager.Instance.Model.IsSoundOpen = isOn;
            AudioManager.Instance.Model.IsBgmOpen = isOn; 
        }
    }
}