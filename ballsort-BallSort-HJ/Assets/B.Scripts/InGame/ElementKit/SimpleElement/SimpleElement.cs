using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lei31.SElement
{
    public class SimpleElement : MonoBehaviour
    {
        public T GetBehaviour<T>() where T : class, ISimpleComponent
        {
            System.Type viewType = typeof(T);

            return _typeToElement[viewType] as T;
        }

        public void InitSimpleElement<T>() where T : SimpleElement
        {
            _typeToElement = new Dictionary<Type, ISimpleComponent>();
            SimpleElementComponent<T>[] allEle = GetComponentsInChildren<SimpleElementComponent<T>>(true);
            for (int i = 0; i < allEle.Length; i++)
            {
                allEle[i].Init(this);
                _typeToElement.Add(allEle[i].GetType(), allEle[i]);
            }
        }

        private Dictionary<System.Type, ISimpleComponent> _typeToElement;
    }
}