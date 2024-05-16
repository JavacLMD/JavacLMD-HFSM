using System;

namespace JavacLMD.HFSM.Exceptions {

    public abstract class HFSMException : Exception
    {
        public HFSMException(string message) : base(message)
        {

        }

        public HFSMException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }

    public class StateException<TStateID> : HFSMException
    {
        public StateException(string message) : base(message)
        {

        }

        public StateException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    public class StateNotFoundException<TStateID> : StateException<TStateID>
    {

        public StateNotFoundException(TStateID stateID) : base("State {" + stateID.ToString() + "} is not found!")
        {
            
        }

        public StateNotFoundException(TStateID stateID, Exception innerException) : base("State {" + stateID.ToString() + "} is not found!", innerException)
        {

        }
    }

    public class StateNotRegisteredException<TStateID> : StateException<TStateID>
    {
        public StateNotRegisteredException(TStateID stateID, IStateMachine<TStateID> sm) : base("State {" + stateID.ToString() + "} is not registered to StateMachine {" + sm + "}")
        {

        }

        public StateNotRegisteredException(TStateID stateID, IStateMachine<TStateID> sm, Exception innerException) : base("State {" + stateID.ToString() + "} is not registered to StateMachine {" + sm + "}", innerException)
        {

        }
    }

}

