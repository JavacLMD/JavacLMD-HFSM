using System;
using UnityEngine;

namespace JavacLMD.EventSystem
{
    
    public interface IGameEventStorage<TEventID>
    {
        /// <summary>
        /// Add a 'Listener' to the Storage that does not take arguments.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="listener">Action that will be executed when the event is triggered.</param>
        void AddListener(TEventID id, Action listener);
        /// <summary>
        /// Remove a 'Listener' from the Storage that does not take arguments.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="listener"></param>
        void RemoveListener(TEventID id, Action listener);
        /// <summary>
        /// Add a 'Listener' to the Storage that takes an <see cref="IGameEvent"/> data argument.
        /// </summary>
        /// <typeparam name="TGameEvent">Data argument deriving from <see cref="IGameEvent"/></typeparam>
        /// <param name="id"></param>
        /// <param name="listener"></param>
        void AddListener<TGameEvent>(TEventID id, Action<TGameEvent> listener) where TGameEvent : IGameEvent;
        /// <summary>
        /// Remove a 'Listener' from the Storage that takes an <see cref="IGameEvent"/> data argument.
        /// </summary>
        /// <typeparam name="TGameEvent">Data argument deriving from <see cref="IGameEvent"/></typeparam>
        /// <param name="id"></param>
        /// <param name="listener"></param>
        void RemoveListener<TGameEvent>(TEventID id, Action<TGameEvent> listener) where TGameEvent : IGameEvent;

        /// <summary>
        /// Execute the event that doesn't take arguments
        /// </summary>
        /// <param name="eventID"></param>
        void TriggerEvent(TEventID eventID);
        /// <summary>
        /// Execute the event passing in the <see cref="IGameEvent"/> data argument.
        /// </summary>
        /// <typeparam name="TGameEvent"></typeparam>
        /// <param name="eventID"></param>
        /// <param name="eventData">Data object deriving from <see cref="IGameEvent"/></param>
        void TriggerEvent<TGameEvent>(TEventID eventID, TGameEvent eventData) where TGameEvent : IGameEvent;
    }

    public interface IGameEvent { }

    /// <summary>
    /// IGameEvent that does not hold any data
    /// </summary>
    public sealed class NoArgGameEvent : IGameEvent
    {
        public static readonly NoArgGameEvent INSTANCE = new NoArgGameEvent();
    }

    /// <summary>
    /// IGameEvent that takes a generic type parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

    /// <summary>
    /// IGameEvent with single String data
    /// </summary>
    public class StringGameEvent : GenericTypeGameEvent<string> { }
    /// <summary>
    /// IGameEvent with single Float data
    /// </summary>
    public class FloatGameEvent : GenericTypeGameEvent<float> { }

    /// <summary>
    /// IGameEvent with single Integer data
    /// </summary>
    public class IntGameEvent : GenericTypeGameEvent<int> { }
    

    /// <summary>
    /// Helper class to create new GameEvents
    /// </summary>
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