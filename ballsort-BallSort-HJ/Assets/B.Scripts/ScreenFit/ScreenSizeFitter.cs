using UnityEngine;

public static class ScreenSizeFitter
{
    public static float IPhone10DownDistance = 132;

    public static bool IsIPhone10
    {
        get
        {
            return DeviceType == IOSDeviceType.IPhoneX;
        }
    }

    public static bool IsIPad
    {
        get
        {
            return DeviceType == IOSDeviceType.IPad;
        }
    }

    //??????
    public static bool IsAndroidPad
    {
        get
        {
#if UNITY_ANDROID &&  !UNITY_EDITOR
            float physicscreen = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / Screen.dpi;
            return physicscreen >= 7f;
#endif
            return false;
        }
    }

    static ScreenSizeFitter()
    { 
        Debug.Log("Current device name :" + SystemInfo.deviceModel + " DeviceType :" + DeviceType);
    }

    public enum IOSDeviceType
    {
        IPhone6,
        IPhoneX,
        IPad,
    }

    public static IOSDeviceType DeviceType
    {
        get
        {
            if (!_hasInit)
            {
                _type = GetDeviceType();
                _hasInit = true;
            }

            return _type;
        }
    }

    private static IOSDeviceType GetDeviceType()
    {
#if UNITY_EDITOR
        float rate = (float) Screen.height / (float) Screen.width;

        if (rate > 2f)
        {
            return IOSDeviceType.IPhoneX;
        }
        else if (rate > 1.7f)
        {
            return IOSDeviceType.IPhone6;
        }
        else
        {
            return IOSDeviceType.IPad;
        }
#else
        if (SystemInfo.deviceModel.ToLower().Contains("iphone10"))
        {
            return IOSDeviceType.IPhoneX;
        }
        else if (SystemInfo.deviceModel.ToLower().Contains("ipad"))
        {
            return IOSDeviceType.IPad;
        }
        else
        {
            return IOSDeviceType.IPhone6;
        }
#endif
    }

    private static IOSDeviceType _type;
    private static bool _hasInit = false;
}