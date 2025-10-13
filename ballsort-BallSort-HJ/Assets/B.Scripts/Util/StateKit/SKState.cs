namespace Prime31.StateKit
{
    public abstract class SKState<T>
    {
        protected SKStateMachine<T> _machine;
        protected T _context;

        public SKState() { }

        internal void setMachineAndContext(SKStateMachine<T> machine, T context)
        {
            _machine = machine;
            _context = context;
            onInitialized();
        }

        public virtual void onInitialized() { }
        public virtual void begin() { }
        public virtual void reason() { }
        public virtual void update(float deltaTime) { }
        public virtual void end() { }
    }
}