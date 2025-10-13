using UnityEngine;

public class NetworkSetting : ScriptableSingleton<NetworkSetting>
{
    [SerializeField] public bool OpenUserServiceStub = false;
    [SerializeField] public int NetConnectTimeout = 30;
}