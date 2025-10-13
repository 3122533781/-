#pragma warning disable CS0162

public class SDKDelegateControl
{
    public static void Init()
    {
#if !UNITY_EDITOR && ENBALE_WX
        PlayableInit();
#endif
    }

    private static void PlayableInit()
    {
        //if (!SoyConfigToolRes.Instance.UseWXADMode)
        //{
        //    return;//开关关闭
        //}

       // IADDelegate.SetDelegate(new WXADDelegate());

        //PublicModuleInstaller.SetDelegateReady();
        LDebug.Log($"Init SDKDelegateControl success .");
    }
}