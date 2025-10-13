using UnityEngine;

public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
{
    public static T Instance
    {
        get
        {
            if (cachedInstance == null)
            {
                cachedInstance = Resources.Load(ResourcePath) as T;
                if (cachedInstance != null) cachedInstance.HandleOnEnable();
            }
            if (cachedInstance == null)
            {
                Debug.LogWarning("No instance of " + FileName + " found, using default values");
                cachedInstance = ScriptableObject.CreateInstance<T>();
                cachedInstance.HandleOnEnable();
            }
            return cachedInstance;
        }
    }

    private static string FileName
    {
        get
        {
            return typeof(T).Name;
        }
    }

    private static string ResourcePath
    {
        get
        {
            return FileName;
        }
    }

    private static T cachedInstance;

    protected virtual void HandleOnEnable() { }
}