using UnityEngine;

public class DelayGetComponent<T> where T : Component
{
    public T Get(GameObject target)
    {
        if (_instance == null)
        {
            _instance = target.GetComponent<T>();
        }

        if (_instance == null)
        {
            Debug.LogError(target.name + " not get component " + typeof(T));
        }

        return _instance;
    }


    private T _instance;
}