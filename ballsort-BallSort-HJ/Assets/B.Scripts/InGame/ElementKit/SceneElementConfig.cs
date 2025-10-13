using UnityEngine;
using System.Collections.Generic;
using Fangtang;
using ProjectSpace.Lei31Utils.Scripts.Framework.ElementKit;

public class SceneElementConfig : MonoBehaviour
{
    public List<SceneElement> Elements { get { return _elements; } }

    public void RemoveAllElements()
    {
        for (int i = 0; i < _elements.Count; i++)
        {
            SceneElementManager.Instance.UnregisterElement(_elements[i]);
        }
    }

    private void AddElements()
    {
        for (int i = 0; i < _elements.Count; i++)
        {
            SceneElementManager.Instance.TryAddElementToDictioinary(_elements[i]);
        }

        for (int i = 0; i < _elements.Count; i++)
        {
            SceneElementManager.Instance.InjectDependencies(_elements[i]);
        }
    }

    private void Awake()
    {
        AddElements();

        for (int i = 0; i < _elements.Count; i++)
        {
            _elements[i].Init(null);
        }
    }

    private void OnDestroy()
    {
        RemoveAllElements();
    }

    [SerializeField]
    private List<SceneElement> _elements = null;
}
