
using MoreMountains.NiceVibrations;

public class VibrationManager : MonoSingleton<VibrationManager>
{
    public bool IsOpen { get; private set; }

    protected override void HandleAwake()
    {
        IsOpen = PlayerDataStorage.GetVibrationOpen();
        LDebug.Log($"VibrationManager is open {IsOpen}");
    }

    public void SetIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        PlayerDataStorage.SetVibrationOpen(isOpen);
        if (!isOpen)
        {
            StaticModule.SwitchVibrateOff();
        }
    }
    
    public void SelectedBlockImpact()
    {
        if (!IsOpen)
            return;
        MMVibrationManager.Haptic(HapticTypes.Selection);
    }

    public void LightImpact()
    {
        if (!IsOpen)
            return;

        MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }

    public void MediumImpact()
    {
        if (!IsOpen)
            return;
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }

    public void HeavyImpact()
    {
        if (!IsOpen)
            return;
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }
}