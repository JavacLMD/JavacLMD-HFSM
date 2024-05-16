using System;
using UnityEngine;

namespace JavacLMD.EventSystem
{
    
    public interface IGameEventStorage<TEventID>
    {
        void AddListener(TEventID id, Action listener);
        void RemoveListener(TEventID id, Action listener);
        void AddListener<TGameEvent>(TEventID id, Action<TGameEvent> listener) where TGameEvent : IGameEvent;
        void RemoveListener<TGameEvent>(TEventID id, Action<TGameEvent> listener) where TGameEvent : IGameEvent;

        void TriggerEvent(TEventID eventID);
        void TriggerEvent<TGameEvent>(TEventID eventID, TGameEvent eventData) where TGameEvent : IGameEvent;
    }

    public interface IGameEvent { }

    public class NoArgGameEvent : IGameEvent
    {
        public static readonly NoArgGameEvent INSTANCE = new NoArgGameEvent();
    }

    [Serializable]
    public class GenericTypeGameEvent<T> : IGameEvent
    {
        [SerializeField]
        public T Value;

        public GenericTypeGameEvent(T value)
        {
            this.Value = value;
        }

        public GenericTypeGameEvent()
        {
            this.Value = default;
        }
    }

    public class StringGameEvent : GenericTypeGameEvent<string> { }
    public class FloatGameEvent : GenericTypeGameEvent<float> { }
    public class IntGameEvent : GenericTypeGameEvent<int> { }
    

    public static class GameEvents
    {
        public static readonly NoArgGameEvent NO_ARG_GAME_EVENT = NoArgGameEvent.INSTANCE;

        public static GenericTypeGameEvent<T> CreateGenericTypeGameEvent<T>(T value)
        {
            return new GenericTypeGameEvent<T>(value);
        }

        public static StringGameEvent CreateStringEvent() => new StringGameEvent();
        public static FloatGameEvent CreateFloatEvent() => new FloatGameEvent();
        public static IntGameEvent CreateIntEvent() => new IntGameEvent();





    }
    
    


    
    
}