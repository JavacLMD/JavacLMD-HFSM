namespace JavacLMD.HFSM
{
    public interface IStateMachine<TStateID>
    {
        TStateID ActiveStateID { get; }
        TStateID PreviousStateID { get; }

        void AddState(IState<TStateID> state);
        void AddTransition(ITransition<TStateID> transition);
        void AddAnyTransition(ITransition<TStateID> transition);

        void SwitchState(TStateID nextState);
        void SetInitialState(TStateID stateID);

    }


}
