namespace Fangtang
{
    public class ElementComponent<T> : IElementComponent where T : SceneElement
    {
        public bool IsInited { get; private set; }

        public T Context
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

        public bool IsActive { get; private set; }

        public ElementComponent()
        {
            IsInited = false;
        }

        public void Init(SceneElement context, IElementComponents container)
        {
            _components = container;
            Context = context as T;

            OnInit();
            IsInited = true;
        }

        public R Get<R>() where R : class, IElementComponent 
        {
            return _components.Get<R>() as R;
        }

        public void Activate()
        {
            if (IsActive == false)
            {
                OnActivate();
                IsActive = true;
            }
        }

        public void Deactivate()
        {
            if (IsActive == true)
            {
                IsActive = false;
                OnDeactivate();
            }
        }

        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }
        protected virtual void OnInit() { }

        protected T _context;
        private IElementComponents _components;
    }
}
