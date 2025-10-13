using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableABConfigGroup<T, R> :
    ScriptableObject
    where T : IConfig
    where R : ScriptableABConfigGroup<T, R>
{
    public List<T> All
    {
        get { return _configs; }
    }

    public bool TryGetConfigByID(int id, out T config)
    {
#if UNITY_EDITOR
        RefreshIdToConfigMap();
#endif
        return _idToConfig.TryGetValue(id, out config);
    }

    public T GetConfigByID(int id)
    {
#if UNITY_EDITOR
        RefreshIdToConfigMap();
#endif
        if (_idToConfig.ContainsKey(id))
        {
            return _idToConfig[id];
        }

        Debug.LogError($"Get not exist goods in CollectionSystemModel id:{id}");
        return default(T);
    }

    private void OnEnable()
    {
        //RefreshIdToConfigMap();
    }

    private void RefreshIdToConfigMap()
    {
        if (_idToConfig == null)
        {
            _idToConfig = new Dictionary<int, T>();
        }

        _idToConfig.Clear();

        for (int i = 0; i < _configs.Count; i++)
        {
            if (_configs[i] == null)
            {
                Debug.LogError("ScriptableConfigGroup [" + name + "]'s " + i + " item is null.");
            }
            else
            {
                if (_idToConfig.ContainsKey(_configs[i].ID))
                {
                    Debug.LogError(typeof(T) + " config with same ID: " + _configs[i].ID);
                }
                else
                {
                    _idToConfig.Add(_configs[i].ID, _configs[i]);
                }
            }
        }
    }

    [SerializeField] private List<T> _configs = new List<T>();

    private Dictionary<int, T> _idToConfig = null;

    #region ScriptableSingleton

    /// <summary>
    /// 为了释放其占用的资源（图片等）,移除 instance 引用，使得GC 工作时候可以释放内存
    /// </summary>
    public void FreeInstance()
    {
        Resources.UnloadAsset(cachedInstanceB);
        cachedInstanceB = null;
        Resources.UnloadAsset(cachedInstanceA);
        cachedInstanceA = null;
    }

    //获取配置文件时候 更具需要的目标获取
    public static R GetInstance(bool useB)
    {
        R cachedInstance = useB ? cachedInstanceB : cachedInstanceA;
        
        if (cachedInstance == null)
        {
            cachedInstance = Resources.Load(GetResourcePath(useB)) as R;
        }
        
        if (cachedInstance == null)
        {
            Debug.LogWarning("No instance of " + FileName + " found, using default values");
            cachedInstance = ScriptableObject.CreateInstance<R>();
            cachedInstance.OnCreate();
        }

        cachedInstance.RefreshIdToConfigMap();

        return cachedInstance;
    }

    private static string FileName
    {
        get { return typeof(R).Name; }
    }

#if UNITY_EDITOR
    private static string AssetPath
    {
        get { return "Assets/Resources/" + FileName + ".asset"; }
    }
#endif

    private static string GetResourcePath(bool useB)
    {
        return useB ? $"{FileName}_B" : FileName;
    }

    private static R cachedInstanceA;
    private static R cachedInstanceB;

    protected virtual void OnCreate()
    {
        // Do setup particular to your class here
    }

    protected virtual void OnInit()
    {
    }

    #endregion
}