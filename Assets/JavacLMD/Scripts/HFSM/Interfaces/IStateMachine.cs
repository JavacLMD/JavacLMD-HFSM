namespace JavacLMD.HFSM
{
    public interface IStateMachine<TStateID>
    {
        TStateID ActiveStateID { get; }
        TStateID PreviousStateID { get; }

        void AddState<T>(T state) where T : IState<TStateID>;
        void RemoveState<T>(T state) where T : IState<TStateID>;
        void AddTransition<T>(T transition) where T : ITransition<TStateID>;
        void RemoveTransition<T>(T transition) where T: ITransition<TStateID>;
        void AddAnyTransition<T>(T transition) where T : ITransition<TStateID>;
        void RemoveAnyTransition<T>(T transition) where T : ITransition<TStateID>;

        void SwitchState(TStateID nextState);
        void SetInitialState(TStateID stateID);

        T GetState<T>(TStateID stateID) where T : IState<TStateID>;
        T GetActiveState<T>() where T : IState<TStateID>;


    }


}
