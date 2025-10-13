using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Yielders
{
    public static WaitForEndOfFrame EndOfFrame
    {
        get { return _endOfFrame; }
    }

    public static WaitForFixedUpdate FixedUpdate
    {
        get { return _fixedUpdate; }
    }

    public static WaitForSeconds Get(float seconds)
    {
        if (!_timeInterval.ContainsKey(seconds))
        {
            _timeInterval.Add(seconds, new WaitForSeconds(seconds));
        }
        return _timeInterval[seconds];
    }

    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    private static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100);
    private static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    private static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
}
