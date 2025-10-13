
using System.Collections.Generic;


public class Configs
{
    //host-1
    public static string apiHost = "api.twinetgame.com/";
    
    //获取单词解释地址
    public static string wordExplainHost = "https://word-des.s3.us-east-2.amazonaws.com/";
    public static string wordExplainHost2 = "https://api.dictionaryapi.dev/api/v2/entries/en/";

    // Facebook 主页地址
    public static string facebookHomeUrl = "https://www.facebook.com/Solitaire-Collection-102292551859689";

    // 主页
    public static string homePage = "http://www.soybeangame.com";

    // google 登录client id
    public static string googleClientId = "1074464606919-9nkjbi8ukcf8ibpv26jppmalb693ndv8.apps.googleusercontent.com";

    //反馈邮箱
    public static string supportEmail = "solitairereal@outlook.com";

    public static string appsFlyerKey = "2ZJJtEZ7Xif8UiPQen8kE9";

    public static int gameId = 142;

    public static Dictionary<string, string> admob_android = new Dictionary<string, string>()
    {
        { "appId", "" },
        { "banner_unit_id", "ca-app-pub-3940256099942544/6300978111" },
        { "insertitial_unit_id", "ca-app-pub-3940256099942544/1033173712" },
        { "rewarded_unit_id", "ca-app-pub-3940256099942544/5224354917" },
    };

    public static Dictionary<string, string> admob_ios = new Dictionary<string, string>()
    {
        { "appId", "" },
        { "banner_unit_id", "ca-app-pub-3940256099942544/6300978111" },
        { "insertitial_unit_id", "ca-app-pub-3940256099942544/1033173712" },
        { "rewarded_unit_id", "ca-app-pub-3940256099942544/5224354917" },
    };

    public static Dictionary<string, string> FBAN_ios = new Dictionary<string, string>()
    {
        { "appId", "" },
        { "banner_unit_id", "ca-app-pub-3940256099942544/6300978111" },
        { "insertitial_unit_id", "ca-app-pub-3940256099942544/1033173712" },
        { "rewarded_unit_id", "ca-app-pub-3940256099942544/5224354917" },
    };
    public static Dictionary<string, string> FBAN_android = new Dictionary<string, string>()
    {
        { "appId", "" },
        { "banner_unit_id", "572017636955794_572032440287647" },
        { "insertitial_unit_id", "572017636955794_572032736954284" },
        { "rewarded_unit_id", "572017636955794_572019736955584" },
    };

    public static Dictionary<string, string> MoPub_ios = new Dictionary<string, string>
    {
        { "banner_unit_id", "fac08820c05f4fc78957617eb728412e" },
        { "insertitial_unit_id", "f019f049cb76420ca9db6b8a5ddd861f" },
        { "rewarded_unit_id", "e55e9d483f824bbcbaf7d34db05a0171" },
    };

    public static Dictionary<string, string> MoPub_android = new Dictionary<string, string>
    {
        { "banner_unit_id", "cf3ce41c6e744430b95d4b4fb97d20e7" },
        { "insertitial_unit_id", "522c942ae2d94da69ee60089fc34b026" },
        { "rewarded_unit_id", "bd5677bf7ad54ac39897a95012a2ffdf" },
    };
    // 发型的账号
    //public static Dictionary<string, string> MoPub_android = new Dictionary<string, string>
    //{
    //    { "banner_unit_id", "fac08820c05f4fc78957617eb728412e" },
    //    { "insertitial_unit_id", "f019f049cb76420ca9db6b8a5ddd861f" },
    //    { "rewarded_unit_id", "e55e9d483f824bbcbaf7d34db05a0171" },
    //};
}
