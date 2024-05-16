namespace JavacLMD.HFSM
{
    public interface ITransition<TStateID>
    {
        TStateID From { get; }
        TStateID To { get; }
        void Init(IStateMachine<TStateID> parentSM);
        bool ShouldTransition();
    }


}
