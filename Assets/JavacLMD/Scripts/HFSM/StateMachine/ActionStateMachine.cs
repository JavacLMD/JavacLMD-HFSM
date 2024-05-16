using JavacLMD.EventSystem;
using System;
using System.Collections.Generic;

namespace JavacLMD.HFSM
{
    [Serializable]
    public class ActionStateMachine<TStateID, TEventID> : StateMachine<TStateID>, IActionable<TEventID>, ITriggerable<TEventID>
    {

        private EventStorage<TEventID> eventStorage;
        private Dictionary<TStateID, Dictionary<TEventID, List<ITransition<TStateID>>>> stateEventTransitions;
        private Dictionary<TEventID, List<ITransition<TStateID>>> anyEventTransitions;

        public void AddEventTransitions(TEventID eventID, ITransition<TStateID> transition)
        {
            stateEventTransitions ??= new Dictionary<TStateID, Dictionary<TEventID, List<ITransition<TStateID>>>>();

            Dictionary<TEventID, List<ITransition<TStateID>>> eventTransitions;
            if (!stateEventTransitions.TryGetValue(transition.From, out eventTransitions) || eventTransitions == null)
            {
                eventTransitions = new Dictionary<TEventID, List<ITransition<TStateID>>>();
                stateEventTransitions[transition.From] = eventTransitions;
            }

            List<ITransition<TStateID>> transitions;
            if (!eventTransitions.TryGetValue(eventID, out transitions) || transitions == null)
            {
                transitions = new List<ITransition<TStateID>>();
                eventTransitions[eventID] = transitions;
            }

            transition.Init(parentStateMachine ?? this);
            transitions.Add(transition);

            eventTransitions[eventID] = transitions;
            stateEventTransitions[transition.From] = eventTransitions;
        }

        public void AddAnyEventTransitions(TEventID eventID, ITransition<TStateID> transition)
        {
            List<ITransition<TStateID>> transitions;
            if (!anyEventTransitions.TryGetValue(eventID, out transitions) || transitions == null)
            {
                transitions = new List<ITransition<TStateID>>();
                anyEventTransitions[eventID] = transitions;
            }

            transition.Init(parentStateMachine ?? this);
            transitions.Add(transition);

            anyEventTransitions[eventID] = transitions;
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

        public void Trigger(TEventID eventId)
        {
            if (TryTriggerTransitions(eventId)) return;

            if (activeStateBundle != null && activeStateBundle.State != null && activeStateBundle.State is ITriggerable<TEventID> state)
                state.Trigger(eventId);
        }

        public void Trigger<TGameEvent>(TEventID eventId, TGameEvent eventData)
        {
            if (TryTriggerTransitions(eventId)) return;

            if (activeStateBundle != null && activeStateBundle.State != null && activeStateBundle.State is ITriggerable<TEventID> state)
                state.Trigger(eventId, eventData);
        }

        private bool TryTriggerTransitions(TEventID eventID)
        {
            List<ITransition<TStateID>> eventTransitions;
            ITransition<TStateID> targetTransition = null;

            //Are there any event transitions for this event?
            if (!anyEventTransitions.TryGetValue(eventID, out eventTransitions) || eventTransitions.Count == 0) goto CheckActiveState;

            if (CheckTransitions(eventTransitions, out targetTransition)) goto SwitchState;


        CheckActiveState:
            //Is there an active state?
            if (activeStateBundle == null || activeStateBundle.State == null) goto SwitchState;
            // Does the active state have any registered events?
            if (!stateEventTransitions.TryGetValue(activeStateBundle.State.ID, out var activeEventTransitions) || activeEventTransitions == null || activeEventTransitions.Count == 0) goto SwitchState; //are there any event transitions available for the state
                                                                                                                                                                                                         //are there any transitions registered for this event
            if (!activeEventTransitions.TryGetValue(eventID, out eventTransitions) || eventTransitions == null || eventTransitions.Count == 0) goto SwitchState; //does the event transitions have a value
            
            if (CheckTransitions(eventTransitions, out targetTransition)) goto SwitchState;


            SwitchState:

            if (targetTransition == null) return false;

            SwitchState(targetTransition.To);
            return true;
        }


    }
}