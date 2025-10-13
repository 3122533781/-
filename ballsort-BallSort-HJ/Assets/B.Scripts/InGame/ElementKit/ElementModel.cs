using System;

namespace Fangtang
{
    public class ElementModel : IElementComponent
    {
        public bool IsInited { get; private set; }

        public void Init(SceneElement sceneElement, IElementComponents container)
        {
            _components = container;
            IsInited = true;
        }

        public R GetModel<R>() where R : ElementModel
        {
            return _components.Get<R>() as R;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public virtual void Reset()
        {
        }

        private IElementComponents _components;
    }

    public class ElementStateModel<TEnum> : ElementModel where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        public Action<TEnum, TEnum> OnStateChange = delegate { };

        public TEnum CurrentState
        {
            get { return _currentState; }
            set
            {
                if (!_currentState.Equals(value))
                {
                    PreviousState = _currentState;
                    _currentState = value;
                    if (_onStateChangeInternal != null)
                    {
                        _onStateChangeInternal(_currentState);
                    }

                    OnStateChange(PreviousState, _currentState);
                }
            }
        }


        public TEnum PreviousState { get; private set; }

        internal void SetOnChangeStateInternal(Action<TEnum> onChangeStateInternal)
        {
            _onStateChangeInternal = onChangeStateInternal;
        }

        private Action<TEnum> _onStateChangeInternal;
        private Action<TEnum> _onHomeStateChangeInternal;


        private TEnum _currentState;
        private TEnum _homeCurrentState;
    }
}