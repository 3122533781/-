using UnityEngine;
using System.Collections;

public class GUIMatrixProvider
{
    public static Matrix4x4 Get()
    {
        float ratio = Screen.width / WIDTH < Screen.height / HEIGHT ? Screen.width / WIDTH : Screen.height / HEIGHT;
        if (ratio != _ratio)
        {
            _ratio = ratio;
            _guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(ratio, ratio, 1));
        }
        return _guiMatrix;
    }
    
    private static Matrix4x4 _guiMatrix;
    private static float _ratio;

    private const float WIDTH = 960f;
    private const float HEIGHT = 540f;
}
