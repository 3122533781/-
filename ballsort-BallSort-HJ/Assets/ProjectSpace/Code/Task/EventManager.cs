using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

    public class MahjongEvent : UnityEvent<Object>
    {

    }
    public class IntParamWrap : Object
    {
        public int value;
    }
    public class EventManager : MonoSingleton<EventManager>
    {

        private Dictionary<string, MahjongEvent> eventDictionary;

        private static EventManager eventManager;

        protected override void HandleAwake()
        {
            Init();
            DontDestroyOnLoad(this);
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, MahjongEvent>();
            }
        }

        public static void On(string eventName, UnityAction<Object> listener)
        {
            MahjongEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new MahjongEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void Off(string eventName, UnityAction<Object> listener)
        {
            if (eventManager == null) return;
            MahjongEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void Emit(string eventName, Object param = null)
        {
            MahjongEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(param);
            }
        }
    }
