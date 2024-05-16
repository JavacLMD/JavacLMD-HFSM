using System;
using System.Collections;
using UnityEngine;


namespace JavacLMD.HFSM
{

    [Serializable]
    public class State<TStateID> : IState<TStateID>
    {
        private TStateID stateId;
        private IStateMachine<TStateID> parentStateMachine;

        public TStateID ID => stateId;

        public event System.Action OnInit, OnEnterState, OnExitState, OnUpdateState, OnLateUpdateState, OnFixedUpdateState;


        public State(TStateID id)
        {
            this.stateId = id;
        }

        public void Init(IStateMachine<TStateID> parentStateMachine)
        {
            this.parentStateMachine = parentStateMachine;
            OnInit?.Invoke();
        }

        public virtual void EnterState()
        {
            OnEnterState?.Invoke();
        }

        public virtual void ExitState()
        {
            OnExitState?.Invoke();
        }

        public virtual void FixedUpdateState()
        {
            OnFixedUpdateState?.Invoke();
        }

        public virtual void LateUpdateState()
        {
            OnLateUpdateState?.Invoke();
        }

        public virtual void UpdateState()
        {
            OnUpdateState?.Invoke();
        }


    }

    [Serializable]
    public class State<TStateID, TSubStateID> : State<TStateID>, IStateMachine<TSubStateID>
    {

        private StateMachine<TSubStateID> subStateMachine;

        public TSubStateID ActiveStateID => subStateMachine.ActiveStateID ?? default;

        public TSubStateID PreviousStateID => subStateMachine.PreviousStateID ?? default;

        public State(TStateID id) : base(id)
        {

        }

        public void AddState(IState<TSubStateID> state)
        {
            InitSubStateMachine();

            subStateMachine.AddState(state);
        }

        public void AddTransition(ITransition<TSubStateID> transition)
        {
            InitSubStateMachine();
            subStateMachine.AddTransition(transition);
        }

        public void AddAnyTransition(ITransition<TSubStateID> transition)
        {
            InitSubStateMachine();
            subStateMachine.AddAnyTransition(transition);
        }

        public void SwitchState(TSubStateID nextState)
        {
            if (subStateMachine == null) return;
            subStateMachine.SwitchState(nextState);
        }

        public void SetInitialState(TSubStateID stateID)
        {
            if (subStateMachine == null) return;
            subStateMachine.SetInitialState(stateID);
        }


        private void InitSubStateMachine()
        {
            if (subStateMachine == null)
            {
                subStateMachine = new StateMachine<TSubStateID>();
                subStateMachine.Init(this);
            }
        }

        public override void EnterState()
        {
            base.EnterState();
            if (subStateMachine != null)
            {
                subStateMachine.EnterState();
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            if (subStateMachine != null)
                subStateMachine.ExitState();
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();

            if (subStateMachine != null)
                subStateMachine.FixedUpdateState();
        }

        public override void LateUpdateState()
        {
            base.LateUpdateState();

            if (subStateMachine != null)
                subStateMachine.LateUpdateState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (subStateMachine != null)
                subStateMachine.UpdateState();
        }
    }

}
