using System;

namespace JavacLMD.EventSystem
{
    public interface IActionable<TEventID>
    {
        void OnAction<TGameEvent>(TEventID eventID, TGameEvent eventData) where TGameEvent : IGameEvent;
        void OnAction(TEventID eventID);

        void AddAction<TGameEvent>(TEventID eventID, Action<TGameEvent> action) where TGameEvent : IGameEvent;
        void AddAction(TEventID eventID, Action action);


    }
}