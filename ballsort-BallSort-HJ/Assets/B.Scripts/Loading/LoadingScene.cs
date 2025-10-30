using System;
using Lei31.Localizetion;
using Prime31;
using ProjectSpace.BubbleMatch.Scripts.Util;
using ProjectSpace.Lei31Utils.Scripts.Common;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils;
using SoyBean.Procedure;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour, IObjectInspectable
{
    private void Start()
    {
        _loadingText.text = $"loading";
        _timer = -1;
        isLoginCompleted = false;
        _bar.Reset(0);
        CheckThirdLogin();
        BgImage.sprite = SpriteManager.Instance.GetBgIcon();
    }

    private void CheckThirdLogin()
    {
        //WXSDKManager.Instance.Init(res =>
        //{
        //    Debug.Log("WXSDK inited:" + res);
        //    StartGuestLogin();
        //});
        StartGuestLogin();
    }

    private void StartGuestLogin()
    {
        if (Game.Instance.GameStatic.IsFirstLoginTimeUnset())
        {
            Game.Instance.GameStatic.SetFirstLoginTime(
                DataFormater.ConvertDateTimeToTimeStamp(DateTime.Now));
            LDebug.Log("First login in " + DateTime.Now.ToShortDateString());
        }

        if (Game.Instance.GameStatic.IsSecondDay() && !Game.Instance.GameStatic.HaveSecondDayLogin.Value)
        {
            Game.Instance.GameStatic.HaveSecondDayLogin.Value = true;
        }

        NextStep();
    }

    private void NextStep()
    {
        _timer = _waitDuration;

        RedDotSystem.Instance.InitRedPointTreeNode();
        Game.Instance.GetSystem<LocalUserSystem>().ThirdLogin(BackLoginCompleted);
    }

    private void BackLoginCompleted()
    {
        LDebug.Log("GuestLogin completed");
        Game.Instance.GetSystem<LocalUserSystem>().FetchIpInfo(() => { LDebug.Log(" Fetch ip completed"); });
        Game.Instance.GetSystem<LocalUserSystem>().FetchGameConfig(() => { LDebug.Log("FetchGameConfig  completed"); });

        if (Game.Instance.GameStatic.PlayTime.Value == 0)
        {
            // PlayerPrefs.SetInt("ClickTubeSkin", 1);
        }
        InitRedeem();
        isLoginCompleted = true;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            _bar.UpdateProgress((_waitDuration - _timer) / _waitDuration);
        }
        if (Time.frameCount % 30 == 0)
        {
            string baseText = "loading";
            string currentDots = _loadingText.text.Replace(baseText, "");
            if (currentDots.Length < 3)
            {
                _loadingText.text = baseText + currentDots + ".";
            }
            else
            {
                _loadingText.text = baseText;
            }
        }

        if (_timer <= 0 )
        {
            enabled = false;
            LoadingCompleted();
        }
    }

    private void LoadingCompleted()
    {
        StaticModule.GameFlow_LoadingCompleted();

        GoToNextScene();
    }

    private void GoToNextScene()
    {
       
            TransitionManager.Instance.Transition(0.5f, () => { SceneManager.LoadScene("InGame"); },
                0.5f);
       
    }

    #region redeem

    private ProcedureWorkerStation _workerStation;

    public void InitRedeem()
    {
        Debug.Log("Test开始请求网赚接口");
        _workerStation = new ProcedureWorkerStation();

        Procedure p1 = new Procedure(1);

        p1.Work = () =>
        {
            Game.Instance.GetSystem<LocalUserSystem>().FetchIpInfo(() =>
            {
                LDebug.Log("p1", " Fetch ip completed");
                p1.Completed();
                if (Game.Instance.LocalUser.IpGroup.IPInfo_1 != null)
                {
                    string country = Game.Instance.LocalUser.IpGroup.IPInfo_1.country;
                    Debug.Log("CountryLog:" + country);
                    if (!string.IsNullOrEmpty(country))
                    {
                        //只有LocalUserSystem SetCountry失败  才会出现这种情况
                      
                    }
                }
            });
        };

        _workerStation.Add(p1, AddProcedureType.Parallel);

        Procedure p2 = new Procedure(1);
        p2.Work = () =>
        {
            Game.Instance.GetSystem<LocalUserSystem>().FetchGameConfig(() =>
            {
                LDebug.Log("p2", " FetchGameConfig  completed");
                p2.Completed();
            });
        };
        _workerStation.Add(p2, AddProcedureType.Serial);

        _workerStation.StartWork();

    }

    #endregion redeem

    [SerializeField] private Text _loadingText = null;
    [SerializeField] private float _waitDuration = 1f;
    [SerializeField] private string _nextSceneName = "Home";
    [SerializeField] private ProgressBar _bar = null;
    [SerializeField] private Image BgImage;
    private float _timer;
    private bool isLoginCompleted;
}