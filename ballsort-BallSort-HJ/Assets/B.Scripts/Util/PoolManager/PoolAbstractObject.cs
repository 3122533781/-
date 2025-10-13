using UnityEngine;

public abstract class PoolAbstractObject : MonoBehaviour
{
    public RectTransform RectTransform
    {
        get { return _delayGetRectTransform.Get(gameObject); }
    }

    public static T Create<T>() where T : PoolAbstractObject
    {
        GameObject result = PoolManager.Instance.CreateObject(typeof(T).ToString()); 
        return result.GetComponent<T>();
    }

    public static T Create<T>(float duration) where T : PoolAbstractObject
    {
        GameObject result = PoolManager.Instance.CreateObject(typeof(T).ToString(), duration);
        return result.GetComponent<T>();
    }

    public void Free<T>()
    {
        if (!PoolManager.IsQuiting && PoolManager.IsInited)
        {
            Transform root = PoolManager.Instance.GetRootTransform(typeof(T).ToString());
            transform.SetParent(root);
            PoolManager.Instance.FreeObject(typeof(T).ToString(), gameObject);
        }
    }

    private DelayGetComponent<RectTransform> _delayGetRectTransform = new DelayGetComponent<RectTransform>();
}