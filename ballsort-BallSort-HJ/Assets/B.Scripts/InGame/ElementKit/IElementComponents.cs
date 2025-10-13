using UnityEngine;
using System.Collections;

namespace Fangtang
{
    public interface IElementComponents
    {
        void Add(IElementComponent component);
        void Remove(IElementComponent component);
        T Get<T>() where T : class, IElementComponent;
    }
}
