using Lei31.SElement;
using UnityEngine;

namespace Lei31.SElement
{
    public class SimpleElementComponent<T> : MonoBehaviour, ISimpleComponent where T : SimpleElement
    {
        public T Context { get; private set; }
        public bool IsInited { get; }

        public void Init(SimpleElement context)
        {
            Context = context as T;
            OnInit();
            //            Debug.Log("Init " + GetType());
        }

        protected virtual void OnInit()
        {
        }
    }
}