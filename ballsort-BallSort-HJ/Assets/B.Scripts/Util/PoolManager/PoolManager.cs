using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR

using System.Linq;

#endif

[System.Serializable]
public class ObjectPoolInfo
{
    public string ID;
    public GameObject Prefab;
    public int InitialCapacity;
}

public class PoolManager : MonoSingleton<PoolManager>
{
#if UNITY_EDITOR
    public List<ObjectRecycler> Pools
    { get { return _pools.Values.ToList(); } }
#endif

    public List<ObjectPoolInfo> PoolInfos
    { get { return _poolInfos; } }

    public Transform GetRootTransform(string id)
    {
        if (_idToParentTransform.ContainsKey(id))
        {
            return _idToParentTransform[id];
        }
        return null;
    }

    public GameObject CreateObject(string id, float duration)
    {
        GameObject gameObject = CreateObject(id);
        if (gameObject != null)
        {
            StartCoroutine(FreeObjectWaitForTime(id, gameObject, duration));
            return gameObject;
        }
        return null;
    }

    public GameObject CreateObject(string id)
    {
        if (_pools.ContainsKey(id))
        {
            return _pools[id].GetNextFree();
        }
        return null;
    }

    public void FreeObject(string id, GameObject gameObject)
    {
        if (_pools.ContainsKey(id))
        {
            _pools[id].FreeObject(gameObject);
        }
    }

    public void FreeAllOfID(string id)
    {
        if (_pools.ContainsKey(id))
        {
            _pools[id].FreeAllObjects();
        }
    }

    public void FreeAll()
    {
        foreach (var pool in _pools.Values)
        {
            pool.FreeAllObjects();
        }
    }

    public void InitPool()
    {
        _pools = new Dictionary<string, ObjectRecycler>();
        _idToParentTransform = new Dictionary<string, Transform>();

        for (int i = 0; i < _poolInfos.Count; i++)
        {
            ObjectPoolInfo info = _poolInfos[i];
            if (info.Prefab != null)
            {
                GameObject container = new GameObject(info.ID);
                //container.transform.parent = transform;
                container.transform.SetParent(transform);
                ObjectRecycler pool = new ObjectRecycler(info.Prefab,
                     info.InitialCapacity, container);
                _pools.Add(info.ID, pool);
                _idToParentTransform.Add(info.ID, container.transform);
            }
            else
            {
                //    Debug.LogWarning("pool info prefab :{0} is null!" + info.ID);
            }
        }
    }

    protected override void HandleAwake()
    {
        InitPool();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator FreeObjectWaitForTime(string id, GameObject gameObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        FreeObject(id, gameObject);
    }

    [SerializeField]
    private List<ObjectPoolInfo> _poolInfos = null;

    private Dictionary<string, ObjectRecycler> _pools;
    private Dictionary<string, Transform> _idToParentTransform;
}