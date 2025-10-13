using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Models;
using System.Text.RegularExpressions;

public class UnityNetworkManager : MonoSingleton<UnityNetworkManager>
{
    public string user_token;

    private void Start()
    {
        user_token = PlayerPrefs.GetString("user_token");
    }

    /**
    UnityWebRequest uwr = new UnityWebRequest();
    uwr.url = "http://www.mysite.com";
    uwr.method = UnityWebRequest.kHttpVerbGET;   // can be set to any custom method, common constants privided
 
    uwr.useHttpContinue = false;
    uwr.chunkedTransfer = false;
    uwr.redirectLimit = 0;  // disable redirects
    uwr.timeout = 60;       // don't make this small, web requests do take some time 
    **/
    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="action"></param>
    public void Get(string url, Action<bool, string> actionResult, bool isRealUrl = false, bool hasErrorCode = false)
    {
        StartCoroutine(_Get(url, actionResult, isRealUrl, hasErrorCode));
    }

    /// <summary>
    /// 请求byte数据
    /// </summary>
    /// <param name="url"></param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的byte数组</param>
    /// <returns></returns>
    public void GetBytes(string url, Action<bool, string, byte[]> actionResult)
    {
        StartCoroutine(_GetBytes(url, actionResult));
    }

    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    public void GetTexture(string url, Action<bool, string, Texture2D> actionResult)
    {
        StartCoroutine(_GetTexture(url, actionResult));
    }

    /// <summary>
    /// 请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    public void GetAssetBundle(string url, Action<bool, string, AssetBundle> actionResult)
    {
        StartCoroutine(_GetAssetBundle(url, actionResult));
    }

    /// <summary>
    /// 请求服务器地址上的音效
    /// </summary>
    /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    public void GetAudioClip(string url, Action<bool, string, AudioClip> actionResult,
        AudioType audioType = AudioType.MPEG)
    {
        StartCoroutine(FetchAudioClipLei31(url, actionResult, audioType));
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="lstformData">form表单参数</param>
    /// <param name="lstformData">处理返回结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void Post(string serverURL, string data, Action<bool, string> actionResult, bool hasErrorCode = false)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

         StartCoroutine(_Post(serverURL, data, actionResult, hasErrorCode));
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <returns></returns>
    public void UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult)
    {
        StartCoroutine(_UploadByPut(url, contentBytes, actionResult, ""));
    }

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url">请求地址,like 'http://www.my-server.com/ '</param>
    /// <param name="action">请求发起后处理回调结果的委托</param>
    /// <returns></returns>
    IEnumerator _Get(string url, Action<bool, string> actionResult, bool isRealUrl, bool hasErrorCode)
    {
        string realUrl = isRealUrl ? url : Configs.apiHost + url;
        if (realUrl.Contains("?"))
        {
            realUrl = realUrl + "&gameId=" + Configs.gameId;
        }
        else
        {
            realUrl = realUrl + "?gameId=" + Configs.gameId;
        }

        Debug.Log(realUrl);
        using (UnityWebRequest uwr = UnityWebRequest.Get(realUrl))
        {
            //yield return uwr.SendWebRequest();
            //UnityWebRequest.ClearCookieCache();
            //Debug.Log("user token is:" + this.user_token);
            if (this.user_token != null)
                uwr.SetRequestHeader("authorization", this.user_token);

            uwr.timeout = NetworkSetting.Instance.NetConnectTimeout;
            yield return uwr.SendWebRequest();
            string text = "";
            bool isError = false;
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                text = uwr.downloadHandler.text;
                //Debug.Log(text);
                if (!isRealUrl)
                {
                    Models.Data data = JsonUtility.FromJson<Models.Data>(text);
                    isError = !data.success;
                    text = AES.Decode(data.data);
                }
            }
            else
            {
                isError = true;
                text = uwr.error;
                if (hasErrorCode)
                {
                    text = Regex.Replace(uwr.downloadHandler.text, @"\s", "");
                    ErrorData error = JsonUtility.FromJson<ErrorData>(text);
                    if (error != null && error.error != null)
                    {
                        text = error.error.Split(':')[0];
                    }
                }

                LDebug.Log("NetWork error", "Make sure INTERNET available or you may lose data");
            }

            if (actionResult != null)
            {
                actionResult.Invoke(isError, text);
            }
        }
    }

    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的byte数组</param>
    /// <returns></returns>
    IEnumerator _GetBytes(string url, Action<bool, string, byte[]> actionResult)
    {
        using (UnityWebRequest uwr = new UnityWebRequest(url))
        {
            DownloadHandlerFile downloadFile = new DownloadHandlerFile(url);
            uwr.downloadHandler = downloadFile;
            yield return uwr.SendWebRequest();
            byte[] bytes = null;
            string text = null;
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                bytes = downloadFile.data;
            }
            else
            {
                text = uwr.error;
            }

            if (actionResult != null)
            {
                actionResult.Invoke(((uwr.result == UnityWebRequest.Result.ConnectionError ||
                                      uwr.result == UnityWebRequest.Result.ProtocolError)), text, bytes);
            }
        }
    }

    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    IEnumerator _GetTexture(string url, Action<bool, string, Texture2D> actionResult)
    {
        using (UnityWebRequest uwr = new UnityWebRequest(url))
        {
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.downloadHandler = downloadTexture;
            yield return uwr.SendWebRequest();
            Texture2D texture = null;
            string text = null;
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                texture = downloadTexture.texture;
            }
            else
            {
                text = uwr.error;
            }

            if (actionResult != null)
            {
                actionResult.Invoke(((uwr.result == UnityWebRequest.Result.ConnectionError ||
                                      uwr.result == UnityWebRequest.Result.ProtocolError)), text, texture);
            }
        }
    }

    /// <summary>
    /// 请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    IEnumerator _GetAssetBundle(string url, Action<bool, string, AssetBundle> actionResult)
    {
        using (UnityWebRequest uwr = new UnityWebRequest(url))
        {
            DownloadHandlerAssetBundle downloadAssetBundle = new DownloadHandlerAssetBundle(uwr.url, uint.MaxValue);
            uwr.downloadHandler = downloadAssetBundle;
            yield return uwr.SendWebRequest();
            AssetBundle assetBundle = null;
            string text = null;
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                assetBundle = downloadAssetBundle.assetBundle;
            }
            else
            {
                text = uwr.error;
            }

            if (actionResult != null)
            {
                actionResult.Invoke(((uwr.result == UnityWebRequest.Result.ConnectionError ||
                                      uwr.result == UnityWebRequest.Result.ProtocolError)), text, assetBundle);
            }
        }
    }

    public IEnumerator FetchAudioClipLei31(string url, Action<bool, string, AudioClip> actionResult,
        AudioType audioType = AudioType.MPEG)
    {
//        LDebug.Log(url);
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(
            url, audioType))
        {
            yield return uwr.SendWebRequest();
            AudioClip audioClip = null;
            string errorText = null;
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                audioClip = DownloadHandlerAudioClip.GetContent(uwr);
//                LDebug.Log("Fetch clip completed");
            }
            else
            {
                errorText = uwr.error;
            }

            if (actionResult != null)
            {
                actionResult.Invoke(((uwr.result == UnityWebRequest.Result.ConnectionError ||
                                      uwr.result == UnityWebRequest.Result.ProtocolError)), errorText, audioClip);
            }
        }
    }

    /// <summary>
    /// 请求服务器地址上的音效
    /// </summary>
    /// <param name="url">没有音频地址,like 'http://myserver.com/mymovie.mp3'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的MovieTexture</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    IEnumerator _GetAudioClip(string url, Action<bool, string, AudioClip> actionResult,
        AudioType audioType = AudioType.WAV)
    {
        using (UnityWebRequest uwr = new UnityWebRequest(url))
        {
            DownloadHandlerAudioClip downloadAudioClip = new DownloadHandlerAudioClip(url, AudioType.WAV);
            uwr.downloadHandler = downloadAudioClip;
            yield return uwr.SendWebRequest();
            AudioClip audioClip = null;
            string text = null;
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                audioClip = downloadAudioClip.audioClip;
            }
            else
            {
                text = uwr.error;
            }

            if (actionResult != null)
            {
                actionResult.Invoke(((uwr.result == UnityWebRequest.Result.ConnectionError ||
                                      uwr.result == UnityWebRequest.Result.ProtocolError)), text, audioClip);
            }
        }
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="url">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="lstformData">form表单参数</param>
    /// <param name="lstformData">处理返回结果的委托</param>
    /// <returns></returns>
    IEnumerator _Post(string url, string data, Action<bool, string> actionResult, bool hasErrorCode)
    {
        string realUrl = Configs.apiHost + url;
        if (realUrl.Contains("?"))
        {
            realUrl = realUrl + "&gameId=" + Configs.gameId;
        }
        else
        {
            realUrl = realUrl + "?gameId=" + Configs.gameId;
        }

        LDebug.Log("try to post:" + realUrl);
        // 加密数据
        Models.Data eData = new Models.Data();
        eData.data = AES.Encode(data);
        string json = JsonUtility.ToJson(eData);

        using (UnityWebRequest uwr = new UnityWebRequest(realUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            uwr.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            if (this.user_token != null)
            {
                uwr.SetRequestHeader("authorization", this.user_token);
            }

            uwr.timeout = NetworkSetting.Instance.NetConnectTimeout;
            yield return uwr.SendWebRequest();
            string text = "";
            bool isError = false;
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                text = uwr.downloadHandler.text;
                Models.Data dData = JsonUtility.FromJson<Models.Data>(text);
                if (dData != null)
                {
                    isError = !dData.success;
                    text = AES.Decode(dData.data);
                }
            }
            else
            {
                isError = true;
                text = uwr.error;
                if (hasErrorCode)
                {
                    text = Regex.Replace(uwr.downloadHandler.text, @"\s", "");
                    ErrorData error = JsonUtility.FromJson<ErrorData>(text);
                    if (error != null && error.error != null)
                    {
                        text = error.error.Split(':')[0];
                    }
                }

                LDebug.LogError("NetWork error",
                    "Opps, There are some problems. uwr.downloadHandler.text:" + uwr.downloadHandler.text
                                                                               + " error: " + text);
            }

            if (actionResult != null)
            {
                actionResult.Invoke(isError, text);
            }
        }
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <param name="resultAction">设置header文件中的Content-Type属性</param>
    /// <returns></returns>
    IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult,
        string contentType = "application/octet-stream")
    {
        using (UnityWebRequest uwr = new UnityWebRequest())
        {
            UploadHandler uploader = new UploadHandlerRaw(contentBytes);

            // Sends header: "Content-Type: custom/content-type";
            uploader.contentType = contentType;

            uwr.uploadHandler = uploader;

            yield return uwr.SendWebRequest();

            bool res = true;
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                res = false;
            }

            if (actionResult != null)
            {
                actionResult.Invoke(res);
            }
        }
    }

    public bool isLogin()
    {
        if (this.user_token == null)
            user_token = PlayerPrefs.GetString("user_token");

        return this.user_token != null && this.user_token != "";
    }

    public IEnumerator GetWordExplain(string word, Action<bool, string> actionResult, bool isRealUrl = true)
    {
        string realUrl = $"{Configs.wordExplainHost2}{word}";
        LDebug.Log(realUrl);
        using (UnityWebRequest uwr = UnityWebRequest.Get(realUrl))
        {
//            if (this.user_token != null)
//                uwr.SetRequestHeader("authorization", this.user_token);

            uwr.timeout = NetworkSetting.Instance.NetConnectTimeout;
            yield return uwr.SendWebRequest();
            string text = "";
            if (!((uwr.result == UnityWebRequest.Result.ConnectionError ||
                   uwr.result == UnityWebRequest.Result.ProtocolError)))
            {
                text = uwr.downloadHandler.text;
                Debug.Log(text);
                if (!isRealUrl)
                {
                    Models.Data data = JsonUtility.FromJson<Models.Data>(text);
                    text = AES.Decode(data.data);
                }
            }
            else
            {
                text = uwr.error;
                LDebug.Log("NetWork error", text);
            }

            if (actionResult != null)
            {
                actionResult.Invoke(((uwr.result == UnityWebRequest.Result.ConnectionError ||
                                      uwr.result == UnityWebRequest.Result.ProtocolError)), text);
            }
        }
    }
}