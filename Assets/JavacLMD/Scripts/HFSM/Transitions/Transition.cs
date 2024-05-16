using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JavacLMD.HFSM
{

    [Serializable]
    public class Transition<TStateID> : ITransition<TStateID>
    {
        [SerializeField]
        private TStateID from, to;
        private Func<bool> predicate;

        public TStateID From => from;
        public TStateID To => to;

        public Transition(TStateID from, TStateID to, Func<bool> predicate)
        {
            this.from = from;
            this.to = to;
            this.predicate = predicate;
        }

        public void Init(IStateMachine<TStateID> parentSM)
        {

        }

        public bool ShouldTransition()
        {
            return predicate.Invoke();
        }
    }
}
