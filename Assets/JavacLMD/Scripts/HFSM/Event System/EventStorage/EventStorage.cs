using System;
using System.Collections.Generic;

namespace JavacLMD.EventSystem
{

    /// <summary>
    /// EventManager that uses <see cref="IGameEvent"/> for all listeners and actions
    /// Event Ids can be shared for different game events and expected data. The listeners will trigger if parameters match accordingly.
    /// </summary>
    /// <typeparam name="TEventId"></typeparam>
    [Serializable]
    public class EventStorage<TEventId> : IGameEventStorage<TEventId>
    {
        
        /// <summary>
        /// private GameEvent<typeparamref name="T"/> class used to hold like listeners with data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class GameEvent<T> : IGameEvent where T : IGameEvent
        {
            private event Action<T> listeners;

            public void AddListener(Action<T> listener)
            {
                listeners += listener;
            }

            public void RemoveListener(Action<T> listener)
            {
                listeners -= listener;
            }

            public void Trigger(T gameEvent)
            {
                listeners?.Invoke(gameEvent);
            }
        }
       /// <summary>
       /// Private GameEvent class used to hold like listeners that don't have data
       /// </summary>
        private class GameEvent : IGameEvent
        {
            private event Action listeners;

            public void AddListener(Action listener)
            {
                listeners += listener;
            }

            public void RemoveListener(Action listener)
            {
                listeners -= listener;
            }

            public void Trigger()
            {
                listeners?.Invoke();
            }
        }
        
        /// <summary>
        /// Private class that can store and run actions.
        /// Separates control over each dictionary to make it simpler to implement.
        /// </summary>
        private class Storage
        {

            private Dictionary<Type, IGameEvent> eventDictionary;
            
            /// <summary>
            /// Adds a listener that can be called with <see cref="TriggerEvent{T}"/>
            /// </summary>
            /// <param name="listener">Function that should be called when the event is triggered</param>
            /// <typeparam name="T"><see cref="IGameEvent"/> class that contains the data needed for the event</typeparam>
            public void AddListener<T>(Action<T> listener) where T : IGameEvent
            {
                var eventType = typeof(T);
                eventDictionary.TryAdd(eventType, default);
                
                ((GameEvent<T>)eventDictionary[eventType]).AddListener(listener);
            }
            
            /// <summary>
            /// Adds a listener that can be called with <see cref="TriggerEvent"/>
            /// </summary>
            /// <param name="listener">Function that should be called when the event is triggered</param>
            public void AddListener(Action listener)
            {
                var eventType = typeof(GameEvent);
                eventDictionary.TryAdd(eventType, default);
                
                ((GameEvent)eventDictionary[eventType]).AddListener(listener);
            }

            /// <summary>
            /// Removes a listener with args
            /// </summary>
            /// <param name="listener">Function that should be removed</param>
            /// <typeparam name="T"><see cref="IGameEvent"/> class that contains the data needed for the event</typeparam>
            public void RemoveListener<T>(Action<T> listener) where T : IGameEvent
            {
                var eventType = typeof(T);
                if (!eventDictionary.TryGetValue(eventType, out var value)) return;
                
                ((GameEvent<T>)value).RemoveListener(listener);
            }
            
            /// <summary>
            /// Removes a listener with noArgs 
            /// </summary>
            /// <param name="listener">Function that should be removed</param>
            public void RemoveListener(Action listener)
            {
                var eventType = typeof(GameEvent);
                if (!eventDictionary.TryGetValue(eventType, out var value)) return;
                
                ((GameEvent)value).RemoveListener(listener);
            }

            /// <summary>
            /// Triggers an event with the given <see cref="TEventId"/>
            /// <para>If the event is not defined / hasn't been added, nothing will happen</para>
            /// </summary>
            /// <param name="eventData">An <see cref="IGameEvent"/> object containing the data to pass to the event</param>
            /// <typeparam name="T"><see cref="IGameEvent"/></typeparam>
            public void TriggerEvent<T>(T eventData) where T : IGameEvent
            {
                var eventType = typeof(T);
                if (eventDictionary.TryGetValue(eventType, out var value))
                {
                    ((GameEvent<T>)value).Trigger(eventData);
                }
            }
            
            /// <summary>
            /// Triggers an event with the given <see cref="TEventId"/>
            /// <para>If the event is not defined / hasn't been added, nothing will happen</para>
            /// </summary>
            public void TriggerEvent()
            {
                var eventType = typeof(GameEvent);
                if (eventDictionary.TryGetValue(eventType, out var value))
                {
                    ((GameEvent)value).Trigger();
                }
            }

        }
        
        private Dictionary<TEventId, Storage> eventStorageById;

        /// <summary>
        /// Adds a listener that can be called with <see cref="TriggerEvent{T}"/>
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="listener">Function that should be called when the event is triggered</param>
        /// <typeparam name="TGameEvent"><see cref="IGameEvent"/> class that contains the data needed for the event</typeparam>
        public void AddListener<TGameEvent>(TEventId eventId, Action<TGameEvent> listener) where TGameEvent : IGameEvent
        {
            Storage eventStorage = GetOrCreateEventStorage(eventId);
            eventStorage?.AddListener(listener);
        }

        /// <summary>
        /// Adds a listener that can be called with <see cref="TriggerEvent"/>
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="listener">Function that should be called when the event is triggered</param>
        public void AddListener(TEventId eventId, Action listener)
        {
            Storage eventStorage = GetOrCreateEventStorage(eventId);
            eventStorage?.AddListener(listener);
        }

        /// <summary>
        /// Removes a listener with args
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="listener">Function that should be removed</param>
        /// <typeparam name="T"><see cref="IGameEvent"/> class that contains the data needed for the event</typeparam>
        /// <typeparam name="TGameEvent"></typeparam>
        public void RemoveListener<TGameEvent>(TEventId eventId, Action<TGameEvent> listener)  where TGameEvent : IGameEvent
        {
            Storage eventStorage = GetOrCreateEventStorage(eventId);
            eventStorage?.RemoveListener(listener);
        }

        /// <summary>
        /// Removes a listener with noArgs 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="listener">Function that should be removed</param>
        public void RemoveListener(TEventId eventId, Action listener)
        {
            Storage eventStorage = GetOrCreateEventStorage(eventId);
            eventStorage?.RemoveListener(listener);
        }

        /// <summary>
        /// Triggers an event with the given <see cref="TEventId"/>
        /// <para>If the event is not defined / hasn't been added, nothing will happen</para>
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventData">An <see cref="IGameEvent"/> object containing the data to pass to the event</param>
        /// <typeparam name="TGameEvent"></typeparam>
        public void TriggerEvent<TGameEvent>(TEventId eventId, TGameEvent eventData) where TGameEvent : IGameEvent
        {
            Storage eventStorage = GetOrCreateEventStorage(eventId);
            eventStorage?.TriggerEvent(eventData);
        }

        /// <summary>
        /// Triggers an event with the given <see cref="TEventId"/>
        /// <para>If the event is not defined / hasn't been added, nothing will happen</para>
        /// </summary>
        public void TriggerEvent(TEventId eventId) 
        {
            Storage eventStorage = GetOrCreateEventStorage(eventId);
            eventStorage?.TriggerEvent();
        }
        
        private Storage GetOrCreateEventStorage(TEventId eventId)
        {
            eventStorageById ??= new Dictionary<TEventId, Storage>();

            if (!eventStorageById.TryGetValue(eventId, out var storage))
            {
                storage = new Storage();
                eventStorageById.Add(eventId, storage);
            }
            
            return storage;
        }
        
        
        

    }
}