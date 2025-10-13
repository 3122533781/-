using System;
using Fangtang;
using UnityEngine;

public class ElementSubUI<T> : MonoBehaviour where T : SceneElement
{
    public void Show(Action onClose)
    {
        _onClose = onClose;
        gameObject.SetActive(true);
    }
    
    public T Context
    {
        get { return _context; }
        private set { _context = value; }
    }

    public void Init(T context)
    {
        _context = context;
        Init();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        if (_onClose != null)
        {
            _onClose.Invoke();
        }
    }

    protected virtual void Init()
    {
    }

    private T _context;
    private Action _onClose;
}