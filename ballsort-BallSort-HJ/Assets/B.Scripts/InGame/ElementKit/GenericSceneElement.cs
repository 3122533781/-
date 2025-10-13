using Prime31.StateKit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fangtang
{
    public abstract class GenericSceneElement<T, TEnum> : SceneElement
        where T : SceneElement
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        public ElementStateModel<TEnum> StateModel { get { return _stateModel; } }
        public ElementComponents<T> Models { get { return _models; } }
        public ElementComponents<T> Views { get { return _views; } }
        public ElementComponents<T> Controllers { get { return _controllers; } }

        public bool PrintStateLog { get; set; }

        public override void Init(object data)
        {
            _stateModel = CreateStateModel();
            _stateModel.SetOnChangeStateInternal(OnChangeStateInternal);

            _enumToStates = new Dictionary<TEnum, SKState<T>>();
            _stateMachine = new SKStateMachine<T>(this as T);

            _models = new ElementComponents<T>(this as T);
            _views = new ElementComponents<T>(this as T);
            _controllers = new ElementComponents<T>(this as T);

            _models.Add(_stateModel);

            OnInit(data);
        }

        public void ResetAllModels()
        {
            IEnumerable<IElementComponent> models = _models.Components;
            foreach (var model in models)
            {
                (model as ElementModel).Reset();
            }
        }

        public R GetModel<R>() where R : class, IElementComponent
        {
            return _models.Get<R>();
        }

        public R GetView<R>() where R : class, IElementComponent
        {
            return _views.Get<R>();
        }

        public R GetController<R>() where R : class, IElementComponent
        {
            return _controllers.Get<R>();
        }

        public void UpdateStateMachine()
        {
            if (_stateMachine != null)
            {
                _stateMachine.update(Time.deltaTime);
            }
        }

        protected virtual ElementStateModel<TEnum> CreateStateModel() { return new ElementStateModel<TEnum>(); }

        protected abstract void OnInit(object data);

        protected void AddState(SKState<T> state, TEnum relatedEnum)
        {
            _stateMachine.addState(state);
            _enumToStates.Add(relatedEnum, state);
        }

        protected void AddStateTransition<S,D>()
            where S : SKState<T> 
            where D : SKState<T>
        {
            _stateMachine.AddStateTransition<S,D>();
        }

        private void OnChangeStateInternal(TEnum newEnumState)
        {
            if (_enumToStates.ContainsKey(newEnumState))
            {
                SKState<T> state = _enumToStates[newEnumState];
                _stateMachine.changeState(state.GetType());

                if (PrintStateLog)
                {
                    Debug.Log(string.Format(
                        "[{0}] changes from {1} to {2}",
                            ID, (_stateMachine.previousState != null ?
                                 _stateMachine.previousState.ToString() : "None"),
                            _stateMachine.currentState.ToString()));
                }
            }
            else
            {
                Debug.LogError("Failed to change state to [" +
                    newEnumState + "] because it's not added in StateMachine.");
            }
        }

        protected ElementComponents<T> _models;
        protected ElementComponents<T> _views;
        protected ElementComponents<T> _controllers;

        protected ElementStateModel<TEnum> _stateModel;
        private SKStateMachine<T> _stateMachine;
        private Dictionary<TEnum, SKState<T>> _enumToStates;
    }
}
