using System;
using UnityEngine;

namespace JavacLMD.EventSystem
{
    public abstract class ScriptableGameEvent<TEventID, TGameEvent> : ScriptableObject where TGameEvent : IGameEvent
    {
        private EventStorage<TEventID> eventStorage = new EventStorage<TEventID>();

        public void AddListener(TEventID eventId, Action<TGameEvent> action)
        {
            eventStorage.AddListener(eventId, action);
        }

        public void RemoveListener(TEventID eventId, Action<TGameEvent> action)
        {
            eventStorage.RemoveListener(eventId, action);
        }

        public void Trigger(TEventID eventId, TGameEvent eventData)
        {
            eventStorage.TriggerEvent(eventId, eventData);
        }

    }

    [CreateAssetMenu(menuName = "JavacLMD/Event System/New Scriptable Event")]
    public class ScriptableGameEvent : ScriptableGameEvent<string, IGameEvent>
    {
    }


}

