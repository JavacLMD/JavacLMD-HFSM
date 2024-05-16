using JavacLMD.HFSM.Exceptions;
using System;
using System.Collections.Generic;


namespace JavacLMD.HFSM
{
    [Serializable]
    public class StateMachine<TStateID> : IStateMachine<TStateID>, IState<TStateID>
    {

        protected class StateBundle
        {
            public IState<TStateID> State;
            public List<ITransition<TStateID>> Transitions;
        }

        protected Dictionary<TStateID, StateBundle> stateBundleMap;
        protected StateBundle activeStateBundle;
        private (TStateID id, bool hasValue) initialState = (default, false);
        protected List<ITransition<TStateID>> anyTransitions;

        public TStateID ID => throw new System.NotImplementedException();
        public TStateID ActiveStateID { get; private set; }
        public TStateID PreviousStateID { get; private set; }

        protected IStateMachine<TStateID> parentStateMachine;

        public void AddTransition(ITransition<TStateID> transition)
        {
            var bundle = GetOrCreateStateBundle(transition.From);
            bundle.Transitions ??= new List<ITransition<TStateID>>();
            transition.Init(parentStateMachine ?? this);
            bundle.Transitions.Add(transition);

            stateBundleMap[transition.From] = bundle;
        }

 
        public void AddAnyTransition(ITransition<TStateID> transition)
        {
            anyTransitions ??= new List<ITransition<TStateID>>();
            transition.Init(parentStateMachine ?? this);
            anyTransitions.Add(transition);
        }

        public void AddState(IState<TStateID> state)
        {
            var bundle = GetOrCreateStateBundle(state.ID);
            state.Init(parentStateMachine ?? this);
            bundle.State = state;

            stateBundleMap[state.ID] = bundle;

            if (stateBundleMap != null && stateBundleMap.Count == 1)
            {
                SetInitialState(state.ID);
            }

        }

        public void SetInitialState(TStateID stateID)
        {
            if (!stateBundleMap.TryGetValue(stateID, out var bundle) || bundle == null || bundle.State == null) {
                throw new StateNotRegisteredException<TStateID>(stateID, this, new StateNotFoundException<TStateID>(stateID));
            }

            initialState = (stateID, true);
        }



        public void SwitchState(TStateID nextState)
        {
            if (activeStateBundle != null && activeStateBundle.State != null && activeStateBundle.State.ID.Equals(nextState))
                throw new StateException<TStateID>("State cannot transition into the same state! Validation conditions to skip checking Transitions with 'To' state that equal active state.\n" +
                    "This will allow other transitions have a chance to pass conditions...");

            if (!stateBundleMap.TryGetValue(nextState, out var nextStateBundle) || nextStateBundle == null || nextStateBundle.State == null)
            {
                throw new StateNotRegisteredException<TStateID>(nextState, this, new StateNotFoundException<TStateID>(nextState));
            }

            if (activeStateBundle != null && activeStateBundle.State != null)
            {
                activeStateBundle.State.ExitState();
                PreviousStateID = activeStateBundle.State.ID;
            }

            activeStateBundle = nextStateBundle;

            if (activeStateBundle != null && activeStateBundle.State != null)
            {
                activeStateBundle.State.EnterState();
                ActiveStateID = activeStateBundle.State.ID;
                //ghost state stuff?
            
            }
        }

        /// <summary>
        /// Helper function to create get and create state bundles;
        /// </summary>
        /// <param name="stateID"></param>
        /// <returns></returns>
        private StateBundle GetOrCreateStateBundle(TStateID stateID)
        {
            stateBundleMap ??= new Dictionary<TStateID, StateBundle>();
            if (!stateBundleMap.TryGetValue(stateID, out var bundle) || bundle == null)
            {
                bundle = new StateBundle();
                stateBundleMap[stateID] = bundle;
            }

            return bundle;
        }

        /// <summary>
        /// Helper method to quickly loop through list of transitions and return the first one that returns true...
        /// </summary>
        /// <param name="transitions">List of transitions</param>
        /// <param name="transition">Transition that returned true in ITransition.ShouldTransition() call...</param>
        /// <returns>True if found transition, false if no action needed</returns>
        protected bool CheckTransitions(List<ITransition<TStateID>> transitions, out ITransition<TStateID> transition)
        {
            transition = null;

            foreach (var t  in transitions)
            {
                //Condition to examine the destination state and skip it as we don't want to transitions to itself...
                if (activeStateBundle != null 
                    && activeStateBundle.State != null 
                    && EqualityComparer<TStateID>.Default.Equals(t.To, activeStateBundle.State.ID))
                    continue;

                if (t.ShouldTransition())
                {
                    transition = t;
                    break;
                }
            }

            return transition != null;
        }

        private void EvaluateTransitions()
        {
            ITransition<TStateID> transition = null;
            if (anyTransitions != null && anyTransitions.Count > 0)
            {
                if (CheckTransitions(anyTransitions, out transition)) goto runSwitchState;
            }

            if (activeStateBundle != null && activeStateBundle.Transitions != null && activeStateBundle.Transitions.Count > 0)
            {
                if (CheckTransitions(activeStateBundle.Transitions, out transition)) goto runSwitchState;
            }


        runSwitchState:

            if (transition == null) return;

            SwitchState(transition.To);

        }

        public void Init(IStateMachine<TStateID> stateMachine)
        {
            parentStateMachine = stateMachine;
        }

        public void EnterState()
        {
            if (!initialState.hasValue) throw new System.Exception("State Machine does not have an inital state set!");

            SwitchState(initialState.id);
        }

        public void ExitState()
        {
            if (activeStateBundle == null) return;

            activeStateBundle.State?.ExitState();
            activeStateBundle = null;
        }

        public void UpdateState()
        {
            EvaluateTransitions();

            if (activeStateBundle.State == null) return;
            activeStateBundle.State.UpdateState();
        }

        public void LateUpdateState()
        {
            if (activeStateBundle == null || activeStateBundle.State == null) return;

            activeStateBundle.State.LateUpdateState();
        }

        public void FixedUpdateState()
        {
            if (activeStateBundle == null || activeStateBundle.State == null) return;

            activeStateBundle.State.FixedUpdateState();
        }
    }

}
