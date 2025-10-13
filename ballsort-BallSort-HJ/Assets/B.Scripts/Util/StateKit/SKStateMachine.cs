using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Prime31.StateKit
{
    public sealed class SKStateMachine<T>
    {
#pragma warning disable
        public event Action onStateChanged;
#pragma warning restore

        public SKState<T> currentState { get { return _currentState; } }
        public SKState<T> previousState { get; private set; }
        public SKState<T> nextState { get; private set; }

        public SKStateMachine(T context)
        {
            _states = new Dictionary<System.Type, SKState<T>>();
            _context = context;
            _stateTransitions = new Dictionary<Type, List<Type>>();
        }

        public SKStateMachine(T context, SKState<T> initialState)
            : this(context)
        {
            addState(initialState);
            _currentState = initialState;
            _currentState.begin();
            _stateTransitions = new Dictionary<Type, List<Type>>();
        }

        public void AddStateTransition<S,D>() 
            where S : SKState<T> 
            where D : SKState<T>
        {
            Type srcState = typeof(S);
            Type desState = typeof(D);

            if (!_stateTransitions.ContainsKey(srcState))
            {
                _stateTransitions.Add(srcState, new List<Type>());
            }

            _stateTransitions[srcState].Add(desState);
        }

        public bool isInState<R>() where R : SKState<T>
        {
            return _currentState != null && _currentState.GetType() == typeof(R);
        }

        public bool isPreviouslyInState<R>() where R : SKState<T>
        {
            return previousState != null && previousState.GetType() == typeof(R);
        }

        public bool isNextInState<R>() where R : SKState<T>
        {
            return nextState != null && nextState.GetType() == typeof(R);
        }

        public void addState(SKState<T> state)
        {
            state.setMachineAndContext(this, _context);
            _states[state.GetType()] = state;
        }

        public R getState<R>() where R : SKState<T>
        {
            System.Type stateType = typeof(R);

#if UNITY_EDITOR
            if (!_states.ContainsKey(stateType))
            {
                var error = GetType() + ": state " +
                    stateType + " does not exist. Did you forget to add it by calling addState?";
                Debug.LogError(error);
                throw new System.Exception(error);
            }
#endif

            return _states[stateType] as R;
        }

        public void update(float deltaTime)
        {
            if (_currentState != null)
            {
                _currentState.reason();
                _currentState.update(deltaTime);
            }
        }

        public SKState<T> revertToPreviousState()
        {
            if (previousState != null)
            {
                nextState = previousState;

                if (_currentState != null)
                {
                    _currentState.end();
                }

                nextState = null;

                SKState<T> oldState = previousState;
                previousState = _currentState;
                _currentState = oldState;
                _currentState.begin();

                if (onStateChanged != null)
                {
                    onStateChanged();
                }
            }

            return _currentState;
        }

        public void changeNullState()
        {
            if (_currentState == null)
            {
                return;
            }

            nextState = null;

            _currentState.end();

            previousState = _currentState;
            _currentState = null; 

            if (onStateChanged != null)
            {
                onStateChanged();
            }
        }

        public void changeState<R>() where R : SKState<T>
        {
            var newType = typeof(R);
            changeState(newType);
        }

        public void changeState(Type newType)
        {
            if (_currentState != null &&
                _currentState.GetType() == newType)
            {
                return;
            }
            
            if (!HaveTransitionToDestination(newType))
            {
                Debug.LogError("StateMachine not`t have transition from " + _currentState.GetType() + " to " + newType);
                return;
            }

#if UNITY_EDITOR
            nextState = !_states.ContainsKey(newType) ? null : _states[newType]; 
#else
            nextState = _states[newType];
#endif

            if (_currentState != null)
            {
                _currentState.end();
            }

#if UNITY_EDITOR
            if (!_states.ContainsKey(newType))
            {
                var error = GetType() + ": state " + newType + " does not exist. Did you forget to add it by calling addState?";
                Debug.LogError(error);
                throw new Exception(error);
            }
#endif

            nextState = null;
            previousState = _currentState;
            _currentState = _states[newType];
            _currentState.begin();

            if (onStateChanged != null)
            {
                onStateChanged();
            }
        }

        private bool HaveTransitionToDestination(Type newType)
        {
            if (_currentState == null)
            {
                return true;
            }

            Type currentStateType = _currentState.GetType();
            if (!_stateTransitions.ContainsKey(currentStateType))
            {
                return false;
            }

            return _stateTransitions[currentStateType].Contains(newType);
        }

        private T _context;
        private Dictionary<System.Type, SKState<T>> _states;
        private SKState<T> _currentState;
        private Dictionary<System.Type, List<System.Type>> _stateTransitions;
    }
}