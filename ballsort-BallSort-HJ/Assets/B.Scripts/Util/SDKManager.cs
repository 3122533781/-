using System;

//using Redeem;

////using UnityEngine.Purchasing;

public class SDKManager
{
    //    //是否是自然流量安装，默认true
    //    public static bool IsOrganicInstall
    //    {
    //        get { return GetAppsFlyerOrganicInstallStatus(); }
    //    }

    //    public static Action<bool> OnOrganicInstallValueChange = null;

    //    private static GameObject SDKManagerObj = null;

    //    private static Dictionary<string, object> firebaseRemoteConfigDefaults = new Dictionary<string, object>()
    //    {
    //        //默认间隔 0 关
    //        {SDKConst.AD_PASSLEVEL_INTERSTITAL, 1},
    //        {SDKConst.ENABLE_BANNER, false},
    //        {SDKConst.ENABLE_INTERSTITAL, false},
    //        {SDKConst.REDEEM_COUNTRY_INDEX, 0},
    //        {SDKConst.REMOTE_IN_GAME_AUTO_SHOW_INSERT_TIME, 180},
    //    };

    //    private static bool setReferUserFlag = false;

    //    private const string AppsFlyerOrganicInstallKey = "AppsFlyerOrganicInstall";

    //    private static int moreRewardADTryCount = 0;

    //    private static bool hasTriggerLogin = false;

    //    //需要初始化的SDK放在这里，在游戏的合适位置调用，一般在进入游戏的第一个脚本Awake处调用
    //    public static void InitSDK()
    //    {
    //        if (SDKManagerObj == null)
    //        {
    //            SDKManagerObj = new GameObject("SDKManagerObj");
    //            SDKManagerObj.AddComponent<MAXSDKManager>();
    //            SDKManagerObj.AddComponent<LocalNotificationManager>();
    //            SDKManagerObj.AddComponent<AppsFlyerManager>();
    //            GameObject.DontDestroyOnLoad(SDKManagerObj);
    //        }

    //        FirebaseManager.Init(firebaseRemoteConfigDefaults);
    //        MAXSDKManager.Instance?.Init();

    //        //IAPStoreManger.Init();
    //        //TDGAManager.Init();
    //        FacebookManager.InitFaceBook();
    //        InitAppsFlyerCallBack();

    //        LDebug.Log($"InitSDK...");
    //    }

    //    #region AD

    //    public static bool NoAutoAD()
    //    {
    //        bool ret = false;
    //        //if (SubscriptionSDKInfo.SubscriptionValid())
    //        //{
    //        //    ret = true;
    //        //}
    //        return ret;
    //    }

    //    public static bool IsInterstitialReady()
    //    {
    //        bool ret = false;
    //        if (MAXSDKManager.Instance != null)
    //        {
    //            ret = MAXSDKManager.Instance.IsInterstitialReady();
    //        }

    //        //LDebug.Log($"IsInterstitialReady ret:{ret}");
    //        return ret;
    //    }

    //    //广告准备好时，才能调用Show
    //    public static void ShowInterstitialAds(string pos, Action<bool> callBack = null)
    //    {
    //        StaticModule.ADInsertitialTriger(pos);
    //        if (IsInterstitialReady())
    //        {
    //            StaticModule.WatchInsertitialAds(pos);
    //            MAXSDKManager.Instance?.ShowInterstitialAds(callBack);
    //        }
    //        else
    //        {
    //            StaticModule.ADInsertitialNo();
    //            callBack?.Invoke(false);
    //        }
    //    }

    //    public static bool IsRewardedAdReady()
    //    {
    //        bool ret = false;
    //        if (MAXSDKManager.Instance != null)
    //        {
    //            ret = MAXSDKManager.Instance.IsRewardedAdReady();
    //        }

    //        //LDebug.Log($"IsRewardedAdReady {ret}");
    //        return ret;
    //    }

    //    //广告准备好时，才能调用Show
    //    private static void ShowRewardedAd(Action<bool> callBack = null)
    //    {
    //        MAXSDKManager.Instance?.ShowRewardedAd(ret =>
    //        {
    //            callBack?.Invoke(ret);
    //            if (ret)
    //            {
    //                StaticModule.RewardCompleted();
    //            }
    //            else
    //            {
    //                StaticModule.RewardCancel();
    //            }
    //        });
    //    }

    //    public static void ShowBanner()
    //    {
    //        //#if !UNITY_EDITOR
    //        MAXSDKManager.Instance?.ShowBanner();
    //        //#endif
    //        LDebug.Log($"ShowBanner...");
    //    }

    //    public static async void ShowBannerAsync()
    //    {
    //        await Task.Delay(500);
    //        ShowBanner();
    //    }

    //    public static void HideBanner()
    //    {
    //        MAXSDKManager.Instance?.HideBanner();
    //    }

    //    public static void ShowMRec()
    //    {
    //        //#if !UNITY_EDITOR
    //        MAXSDKManager.Instance?.UpdateMRecPosition(MaxSdkBase.AdViewPosition.BottomCenter);
    //        MAXSDKManager.Instance?.ShowMRec();
    //        StaticModule.ADMRecsTriger();
    //    }

    //    public static void HideMRec()
    //    {
    //        //#if !UNITY_EDITOR
    //        MAXSDKManager.Instance?.HideMRec();
    //        //#endif
    //        LDebug.Log($"HideMRec...");
    //    }

    //    public static Rect GetMRecLayout()
    //    {
    //        return MAXSDKManager.Instance.GetMRecLayout();
    //    }

    //    public static float GetScreenDensity()
    //    {
    //        return MAXSDKManager.Instance.GetScreenDensity();
    //    }

    //    public static float GetShowMRecDisBottom()
    //    {
    //        Rect rect = GetMRecLayout();
    //        float screenDensity = GetScreenDensity();
    //        float mercHeight = rect.height;
    //        LDebug.Log($"GetShowMRecDisBottom rect:{rect},screenDensity:{screenDensity}");

    //        // MRECs are sized to 300x250 on phones and tablets
    //        if (mercHeight < 100f)
    //        {
    //            CheckMRecLayoutAsync();
    //        }

    //        mercHeight = mercHeight < 100f ? 250f : mercHeight;
    //        return mercHeight * screenDensity;
    //    }

    //    private static async void CheckMRecLayoutAsync()
    //    {
    //        await Task.Delay(500);
    //        Rect rect = GetMRecLayout();
    //        if (rect.height < 100f)
    //        {
    //            StaticModule.ADMRecsNo();
    //        }
    //    }

    //    public static void ShowRewardAndRetry(string pos, Action<bool> callBack = null)
    //    {
    //        StaticModule.RewardTriger(pos);
    //        if (IsRewardedAdReady())
    //        {
    //            StaticModule.WatchRewardAds(pos);
    //            ShowRewardedAd(callBack);
    //        }
    //        else
    //        {
    //            StaticModule.RewardLoading();
    //            //DialogManager.Instance.GetDialog<WaitingDialog>().Activate();
    //            DialogManager.Instance.GetDialog<ADLoadingDialog>().Activate();
    //            moreRewardADTryCount = 0;
    //            CheckMoreRewardADStateAsync(pos, callBack);
    //        }
    //    }

    //    private static async void CheckMoreRewardADStateAsync(string pos, Action<bool> callBack = null, int waitTime = 1000)
    //    {
    //        await Task.Delay(waitTime);
    //        moreRewardADTryCount++;
    //        if (IsRewardedAdReady())
    //        {
    //            StaticModule.RewardLoadingYes();
    //            StaticModule.WatchRewardAds(pos);
    //            ShowRewardedAd(ret =>
    //            {
    //                //DialogManager.Instance.GetDialog<WaitingDialog>().HideWaiting();
    //                DialogManager.Instance.GetDialog<ADLoadingDialog>().Deactivate();
    //                callBack?.Invoke(ret);
    //            });
    //        }
    //        else
    //        {
    //            if (moreRewardADTryCount >= 30)
    //            {
    //                //DialogManager.Instance.GetDialog<WaitingDialog>().HideWaiting();
    //                DialogManager.Instance.GetDialog<ADLoadingDialog>().Deactivate();
    //                StaticModule.ADInsertitialTriger(InsertPos.Replace.ToString());

    //                if (IsInterstitialReady())
    //                {
    //                    StaticModule.RewardLoadingReplace();
    //                    MAXSDKManager.Instance?.ShowInterstitialAds(callBack);
    //                }
    //                else
    //                {
    //                    StaticModule.ADInsertitialNo();
    //                    StaticModule.RewardLoadingNo();
    //                    FloatingWindow.Instance.Show(
    //                        LocalizationManager.Instance.GetTextByTag(LocalizationConst.VIDEO_NOT_READY));
    //                    callBack?.Invoke(false);
    //                }
    //            }
    //            else
    //            {
    //                CheckMoreRewardADStateAsync(pos, callBack, waitTime);
    //            }
    //        }
    //    }

    //    #endregion

    //    #region AppsFlyer

    public static void AppsFlyerGenerateInviteLink(string userName, string userID)
    {
        //AppsFlyerManager.Instance.GenerateInviteLink(userName, userID);
    }

    public static void AppsFlyerGenerateRedeemInviteLink(string userID, string redeemID, Action<string> inviteLinkCallBack)
    {
        //AppsFlyerManager.Instance.GenerateRedeemInviteLink(userID, redeemID, inviteLinkCallBack);
    }

    //    public static void AppsFlyerSetCallBack(Action<bool, string, string> receiveConversionDataCallBack,
    //        Action<string> inviteLinkCallBack)
    //    {
    //        //Debug.Log($"AppsFlyerSetCallBack");
    //        AppsFlyerManager.Instance.SetCallBack(receiveConversionDataCallBack, inviteLinkCallBack);
    //    }

    //    private static void InitAppsFlyerCallBack()
    //    {
    //        //Debug.Log($"InitAppsFlyerCallBack");
    //        AppsFlyerSetCallBack(AppsFlyerConversionDataCallBack, AppsFlyerLinkCallBack);
    //    }

    //    private static void AppsFlyerConversionDataCallBack(bool ret, string referUser, string redeemIDStr)
    //    {
    //        Debug.Log($"AppsFlyerConversionDataCallBack ret:{ret},referUser:{referUser},redeemIDStr:{redeemIDStr},IsOrganicInstall:{IsOrganicInstall}");

    //        if (IsOrganicInstall && !ret)
    //        {
    //            SetAppsFlyerOrganicInstallStatus(ret);
    //            OnOrganicInstallValueChange?.Invoke(ret);
    //        }
    //        //else if (ShowCash())
    //        //{
    //        //    ret = false;
    //        //    SetAppsFlyerOrganicInstallStatus(ret);
    //        //    OnOrganicInstallValueChange?.Invoke(ret);
    //        //}

    //        if (!hasTriggerLogin)
    //        {
    //            hasTriggerLogin = true;
    //            LoadingScene.Instance.HandleLoginAfterAFCallBack(() =>
    //            {
    //                HandleAppsFlyerRefer(referUser, redeemIDStr);
    //            });
    //        }
    //        else
    //        {
    //            HandleAppsFlyerRefer(referUser, redeemIDStr);
    //        }
    //    }

    //    private static void HandleAppsFlyerRefer(string referUser, string redeemIDStr)
    //    {
    //        if (int.TryParse(referUser, out int userID) && int.TryParse(redeemIDStr, out int redeemID))
    //        {
    //            App.Instance.GetSystem<RedeemSystem>().HandleRecordReferUser(userID, redeemID);
    //        }
    //    }

    //    private static void AppsFlyerLinkCallBack(string info)
    //    {
    //        App.Instance.LocalUser.AppsFlyShareLink = info;
    //        LDebug.Log($"AppsFlyerLinkCallBack AppsFlyShareLink:{info}");
    //    }

    //    public static void SetAppsFlyerNonOrganicInstall()
    //    {
    //        SetAppsFlyerOrganicInstallStatus(false);
    //        OnOrganicInstallValueChange?.Invoke(false);
    //        LDebug.Log($"SetAppsFlyerNonOrganicInstall value:{GetAppsFlyerOrganicInstallStatus()}");
    //    }

    //    private static void SetAppsFlyerOrganicInstallStatus(bool isOrganic)
    //    {
    //        PlayerPrefs.SetInt(AppsFlyerOrganicInstallKey, isOrganic ? 1 : 0);
    //        App.Instance.Model.RefreshCanRedeem();
    //    }

    //    public static bool GetAppsFlyerOrganicInstallStatus()
    //    {
    //        return PlayerPrefs.GetInt(AppsFlyerOrganicInstallKey, 1) == 1 ? true : false;
    //    }

    //    #endregion

    //    #region NativeShare

    public static void NativeShare(Action<bool> callBack = null)
    {
        //StaticModule.ShareClick();
        //NativeShare(App.Instance.LocalUser.AppsFlyShareLink, callBack);
    }

    public static void NativeShare(string shareLink, Action<bool> callBack = null)
    {
        //string shareText = $"Download and Play Solitaire Scape";
        //string titleText = "Classic Solitaire Scape Game";
        //List<string> shareTexts = new List<string>()
        //    {
        //        "This Solitaire Trip game really allows you to cash out.",
        //        "This game really allows you to cash out. I downloaded it, played for a bit, and cashed out 10 USD already. ",
        //        "You gotta download this game quick, it really lets you cash out.",
        //        "I can't believe how fast I earned real money just by playing Solitaire Trip. This game is a must download!",
        //        "Solitaire Trip lets you make real cash while enjoying solitaire in beautiful destinations. I cashed out so easily after playing for only a short time!",
        //        "With Solitaire Trip you can turn your solitaire skills into real rewards. I was able to withdraw my earnings instantly!",
        //        "Don't just play solitaire for free - earn real money while you play! Solitaire Trip makes it possible. I already have cash in hand from playing.",
        //        "This is crazy - I'm actually making money just by playing solitaire with Solitaire Trip. Download now and see for yourself how you can cash out too! ",
        //    };

        //shareText = shareTexts[UnityEngine.Random.Range(0, shareTexts.Count)];

        //NativeShare(shareText, shareLink, titleText, callBack);
        ////StaticModule.ShareClick();
    }

    //    private static void NativeShare(string shareText, string shareLink, string titleText, Action<bool> callBack = null)
    //    {
    //        LDebug.Log($"NativeShare shareText:{shareText},shareLink:{shareLink},titleText:{titleText}");
    //        NativeShare ns = new NativeShare();
    //        ns.SetCallback((result, shareTarget) =>
    //        {
    //            LDebug.Log(
    //                $"NativeShare callBack shareText:{shareText},titleText:{titleText},result:{result},shareTarget:{shareTarget}");
    //            if (result == global::NativeShare.ShareResult.Shared)
    //            {
    //                StaticModule.ShareSuccess();
    //                callBack?.Invoke(true);
    //            }
    //            else
    //            {
    //                callBack?.Invoke(false);
    //            }
    //        });
    //        ns.SetText(shareText + shareLink);
    //        ns.SetTitle(titleText);
    //        ns.Share();
    //    }

    //    #endregion

    //    #region IAP

    //    //public static void Purchase(string productId, Action<UnityEngine.Purchasing.Product, bool, string> callBack)
    //    //{
    //    //    LDebug.Log($"Purchase productId:{productId}");
    //    //    IAPStoreManger.Purchase(productId, callBack);
    //    //}

    //    //public static string GetLocalPriceStr(string productId)
    //    //{
    //    //    return IAPStoreManger.GetLocalPriceStr(productId);
    //    //}

    //    //public static void CheckSubscriptionInfo()
    //    //{
    //    //    IAPStoreManger.CheckSubscriptionInfo();
    //    //}

    //    //public static bool GetSubscriptionValidByID(string productId)
    //    //{
    //    //    return IAPStoreManger.GetSubscriptionValidByID(productId);
    //    //}
    //    //#endregion

    //    //#region Facebook
    //    //public static void FacebookLogin(Action<bool> callBack)
    //    //{
    //    //    FacebookManager.FacebookLogin(callBack);
    //    //}

    //    //public static void FacebookLogout()
    //    //{
    //    //    FacebookManager.FacebookLogout();
    //    //}

    //    //public static void GetFBProfile(Action<Dictionary<string, object>> callBack)
    //    //{
    //    //    FacebookManager.GetFBProfile(callBack);
    //    //}

    //    //public static void GetFBPicture(Action<Sprite> callBack)
    //    //{
    //    //    FacebookManager.GetFBPicture(callBack);
    //    //}

    //    #endregion

    //    #region FireBaseRemoteConfig

    //    //AB test 
    //    //0 正常
    //    //1 无banner
    //    //2 20 关才出插屏
    //    public static long GetBannerRetentionLong()
    //    {
    //        return FirebaseRemoteConfigManager.GetValue<long>(SDKConst.AD_PASSLEVEL_INTERSTITAL);
    //    }

    //    public static bool HaveBanner()
    //    {
    //        return GetBannerRetentionLong() != 1;
    //    }

    //    public static bool Intersistal20Level()
    //    {
    //        return GetBannerRetentionLong() == 2;
    //    }

    //    public static bool AD3RewardWithInsert()
    //    {
    //        return true;
    //    }

    //    #endregion

    //    #region FaceBook

    //    public static void FacebookLogin(Action<bool> callBack)
    //    {
    //        FacebookManager.FacebookLogin(callBack);
    //    }

    //    public static void FacebookLogout()
    //    {
    //        FacebookManager.FacebookLogout();
    //    }

    //    public static void GetFBProfile(Action<Dictionary<string, object>> callBack)
    //    {
    //        FacebookManager.GetFBProfile(callBack);
    //    }

    //    public static void GetFBPicture(Action<Sprite> callBack)
    //    {
    //        FacebookManager.GetFBPicture(callBack);
    //    }

    //    #endregion
}