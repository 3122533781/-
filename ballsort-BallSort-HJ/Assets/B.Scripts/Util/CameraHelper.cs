using UnityEngine;

public static class CameraHelper
{
    //return world position
    public static Vector3 TransitionToCameraSpace(Vector3 targetWorldPos, Camera form, Camera to)
    {
        Vector2 screenPoint = form.WorldToScreenPoint(targetWorldPos);
        float z = Vector3.ProjectOnPlane(targetWorldPos - form.transform.position,
            form.transform.up).magnitude;
        Vector3 result = to.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, z));
        return result;
    }
}