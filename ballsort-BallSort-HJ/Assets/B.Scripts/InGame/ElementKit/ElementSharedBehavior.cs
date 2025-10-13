using UnityEngine;

namespace Fangtang
{
    public class ElementSharedBehavior : MonoBehaviour, IElementComponent
    {
        public bool IsInited { get; private set; }

        public SceneElement Context
        {
            get
            {
                return _context;
            }
            private set
            {
                _context = value;
            }
        }

        public T CastContext<T>() where T : SceneElement
        {
            return (T)System.Convert.ChangeType(_context, typeof(T));
        }

        public void Init(SceneElement context, IElementComponents container)
        {
            _components = container;
            Context = context;

            OnInit();
            IsInited = true;
        }

        public R Get<R>() where R : class, IElementComponent
        {
            return _components.Get<R>() as R;
        }

        public virtual void Activate()
        {
            enabled = true;
        }

        public virtual void Deactivate()
        {
            enabled = false;
        }

        protected virtual void OnInit() { }

        [SerializeField]
        [HideInInspector]
        private SceneElement _context;

        private IElementComponents _components;
    }
}
