using JavacLMD.EventSystem;
using System;

namespace JavacLMD.HFSM
{
    [Serializable]
    public class ActionState<TStateID, TSubStateID, TEventID> : State<TStateID, TSubStateID>, IActionable<TEventID>
    {
        EventStorage<TEventID> eventStorage;

        public ActionState(TStateID id) : base(id)
        {

        }

        public void AddAction<TGameEvent>(TEventID eventID, Action<TGameEvent> action) where TGameEvent : IGameEvent
        {
            eventStorage ??= new EventStorage<TEventID>();
            eventStorage.AddListener(eventID, action);
        }

        public void AddAction(TEventID eventID, Action action)
        {
            eventStorage ??= new EventStorage<TEventID>();
            eventStorage.AddListener(eventID, action);
        }

        public void OnAction<TGameEvent>(TEventID eventID, TGameEvent eventData) where TGameEvent : IGameEvent
        {
            eventStorage?.TriggerEvent(eventID, eventData);
        }

        public void OnAction(TEventID eventID)
        {
            eventStorage?.TriggerEvent(eventID);
        }
    }

    public class ActionState<TStateID, TEventID> : State<TStateID>, IActionable<TEventID>
    {

        EventStorage<TEventID> eventStorage;


        public ActionState(TStateID id) : base(id)
        {


        }

        public void AddAction<TGameEvent>(TEventID eventID, Action<TGameEvent> action) where TGameEvent : IGameEvent
        {
            eventStorage ??= new EventStorage<TEventID>();
            eventStorage.AddListener(eventID, action);
        }

        public void AddAction(TEventID eventID, Action action)
        {
            eventStorage ??= new EventStorage<TEventID>();
            eventStorage.AddListener(eventID, action);
        }

        public void OnAction<TGameEvent>(TEventID eventID, TGameEvent eventData) where TGameEvent : IGameEvent
        {
            eventStorage?.TriggerEvent(eventID, eventData);
        }

        public void OnAction(TEventID eventID)
        {
            eventStorage?.TriggerEvent(eventID);
        }
    }

}