using System;
using System.Diagnostics;
using ProjectSpace.Lei31Utils.Scripts.Framework.App;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;

//弹出逻辑：
//1、返回主界面尝试弹出，与商店推销和存钱罐排序
//2、20关之后才弹出
//3、每次弹出之间间隔 300 秒
//4、从评价界面去过应用商店则不弹出
//5、当天弹出3次，被取消后则当天不再弹出
//
public class RatingSystem : GameSystem
{
    public const int MaxTimeGapToShow = 300;

    public RatingModel Model { get; private set; }

    public void CheckAndShowRating()
    {
        if (CanShowRatingNow())
        {
            StaticModule.Rating_Popup();
#if UNITY_IOS
            DialogManager.Instance.GetDialog<RatingSimpleDialog>().Activate();
#else
               DialogManager.Instance.GetDialog<RatingSimpleDialog>().Activate();
#endif
            PlusShowRatingDialogTime();
            Model.LastShowTimestamp.Value = DataFormater.ConvertDateTimeToTimeStamp(DateTime.Now);
        }
    }

    public void CompletedRating()
    {
        SetHasGotoTheMarket();
    }

    public override void Destroy()
    {
    }

    public override void Init()
    {
        Model = new RatingModel();
    }

    public bool CanShowRatingNow()
    {
        if (Game.Instance.LevelModel.MaxUnlockLevel.Value < 6)
        {
            //20关以前不弹出
            return false;
        }

        if (HasGotoTheMarket())
        {
            //去过评价市场，不再弹出
            return false;
        }

        if (Model.GetToLastShowTime() < MaxTimeGapToShow)
        {
            //间隔时间过短
            return false;
        }

        if (GetTodayShowDialogTime() >= 3)
        {
            return false;
        }

        return true;
    }

    private int GetTodayShowDialogTime()
    {
        try
        {
            string dayKey = DataFormater.GetDayTimeStamp(DateTime.Now).ToString();
            return Storage.Instance.GetInt($"{ShowRatingDialogTimeKey}_{dayKey}", 0);
        }
        catch
        {
            return 3;
        }
    }

    private void PlusShowRatingDialogTime()
    {
        try
        {
            string dayKey = DataFormater.GetDayTimeStamp(DateTime.Now).ToString();
            int current = Storage.Instance.GetInt($"{ShowRatingDialogTimeKey}_{dayKey}", 0);
            Storage.Instance.SetInt($"{ShowRatingDialogTimeKey}_{dayKey}", current + 1);
        }
        catch
        {
        }
    }

    private bool HasGotoTheMarket()
    {
        return Storage.Instance.GetBool(HasGotoTheMarketKey, false);
    }

    private void SetHasGotoTheMarket()
    {
        Storage.Instance.SetBool(HasGotoTheMarketKey, true);
    }

    private const string RatingStarKey = "RatingStarKey";
    private const string HasGotoTheMarketKey = "HasGotoTheMarketKey";
    private const string DailyPassLevelKey = "DailyPassLevelKey";
    private const string ShowRatingDialogTimeKey = "DailyPassLevelKey";
}