using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class GameCameraSizeFit : MonoBehaviour
{
    public int fitHeight = 1920;
    public int fitWidth = 1080;

    private void Awake()
    {
        //FixScreen();
    }

    [Button]
    public void FixScreen()
    {
        if (UICamera.Instance.MainCamera.orthographic)
        {
            OrthographicFix();
        }
        else
        {
            PerspectiveFix();
        }
    }

    private void OrthographicFix()
    {
        float orthographicSize = UICamera.Instance.MainCamera.orthographicSize;
        float fitRatio, aspectRatio, devHeight, devWidth;
        fitRatio = fitWidth * 1.0f / fitHeight;
        aspectRatio = Screen.width * 1.0f / Screen.height;
        devHeight = fitHeight * 1.0f / 100 / 2;
        devWidth = devHeight * fitRatio;
        if (fitRatio >= aspectRatio)//“‘øÌ  ≈‰
        {
            float cameraHeight = devWidth / aspectRatio;
            if (cameraHeight > devHeight)
            {
                orthographicSize = cameraHeight;
                UICamera.Instance.MainCamera.orthographicSize = orthographicSize;
            }
        }
        else//“‘∏ﬂ  ≈‰
        {
            float cameraWidth = orthographicSize * 2 * aspectRatio;
            if (cameraWidth < devWidth)
            {
                orthographicSize = devWidth / (2 * aspectRatio);
                UICamera.Instance.MainCamera.orthographicSize = orthographicSize;
            }
        }
    }

    private void PerspectiveFix()
    {
        int manualHeight;
        if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(fitHeight) / fitWidth)
        {
            manualHeight = Mathf.RoundToInt(Convert.ToSingle(fitWidth) / Screen.width * Screen.height);
        }
        else
        {
            manualHeight = fitHeight;
        }
        float scale = Convert.ToSingle(manualHeight * 1.0f / fitHeight);
        UICamera.Instance.MainCamera.fieldOfView *= scale;
    }
}
