using Prime31;
using UnityEngine;

public class SoyProfileTest : MonoBehaviour, Prime31.IObjectInspectable
{
    [MakeButton]
    public void LogInit()
    {
        Debug.Log(SoyProfile.IsInit());
    }

    [MakeButton]
    public void Set()
    {
        Debug.Log(SoyProfile.IsInit());

        SoyProfile.Set("TestKey2", true);
    }

    [SerializeField] public string Key;

    [MakeButton]
    public void LogValue()
    {
        Debug.Log(SoyProfile.Get<bool>(Key));
    }
}