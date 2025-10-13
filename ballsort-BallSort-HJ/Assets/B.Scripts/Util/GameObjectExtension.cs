
using UnityEngine;

public static class GameObjectExtension
{
    public static void SetActiveAvoidNullError(this GameObject target, bool active)
    {
        if (target != null)
        {
            target.SetActive(active);
        }
    }

    public static bool IsInScene(this GameObject target)
    {
        return target.scene.name != null;
    }

    
}