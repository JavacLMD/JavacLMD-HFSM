namespace JavacLMD.HFSM
{
    public interface IState<TStateID>
    {
        TStateID ID { get; }
        void Init(IStateMachine<TStateID> parentStateMachine);

        void EnterState();
        void ExitState();
        void UpdateState();
        void LateUpdateState();
        void FixedUpdateState();
    }
}
