using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class GameConfig : ScriptableSingleton<GameConfig>
{
    [SerializeField] public string GooglePlayURL = "https://play.google.com/store/apps/details?id=com.orderofball.fun";
    [SerializeField] public string SupportEmail = "zhiyiwang7878@outlook.com";


    [SerializeField] public string BundleVersionCode;

    [SerializeField] public string DebugUserString = "+ab1";

    public static string GetVersionInfo()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(Application.productName);
        builder.Append("\t\n");
//        if (Application.platform == RuntimePlatform.Android)
//        {
//            builder.Append("Android");
//        }

        builder.Append($"{Application.version}.{Instance.BundleVersionCode}");

        return builder.ToString();
    }
}