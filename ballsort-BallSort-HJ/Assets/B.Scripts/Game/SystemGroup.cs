using System;
using System.Collections.Generic;
using UnityEngine;

public class SystemGroup
{
    public SystemGroup()
    {
        _typeToSystem = new Dictionary<Type, GameSystem>();
    }

    public void RegisterSystem<T>(T system) where T : GameSystem
    {
        Type t = typeof(T);
        if (_typeToSystem.ContainsKey(t))
        {
            Debug.LogError("System already registered: " + t.ToString());
        }
        else
        {
            _typeToSystem.Add(t, system);
            system.Init();
        }
    }

    public void UnregisterSystem<T>() where T : GameSystem
    {
        Type t = typeof(T);
        if (_typeToSystem.ContainsKey(t))
        {
            _typeToSystem[t].Destroy();
            _typeToSystem.Remove(t);
        }
    }
    
    public T GetSystem<T>() where T : GameSystem
    {
        return _typeToSystem[typeof(T)] as T;
    }
    
    private Dictionary<Type, GameSystem> _typeToSystem;
}