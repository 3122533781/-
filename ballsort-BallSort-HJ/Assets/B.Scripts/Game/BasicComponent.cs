using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicComponent : MonoBehaviour
{
    public UnityEngine.UI.RawImage rawImage;
    public Canvas canvas;
    public UnityEngine.UI.Button CloseBtn;
    public UnityEngine.UI.Button CloseBtnShare;
    public UnityEngine.UI.Button WxBtnShare;
    public GameObject rankMask;
    public GameObject shareMask;
    private void Start()
    {
        Game.Instance.InitCreateInstance();
        DontDestroyOnLoad(gameObject);

        Game.Instance.IsBasicComponentLoaded = true;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;


        //CloseBtn.onClick.AddListener(WXSDKManager.Instance.HideFriendRand);
        //CloseBtnShare.onClick.AddListener(WXSDKManager.Instance.HideShareDialog);
        //WxBtnShare.onClick.AddListener(WXSDKManager.Instance.ShareFriendRand);
        //WXSDKManager.Instance.InitRankCanvas(rawImage, canvas, rankMask);
        //WXSDKManager.Instance.InitShareCanvas(shareMask);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            PlayerPrefs.Save();
        }
    }
}