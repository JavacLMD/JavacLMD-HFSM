using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JavacLMD.HFSM.Demo {


    public class Ball : MonoBehaviour
    {
        public enum BallColor
        {
            White,
            Red,
            Green,
            Blue,
            Black,
        }


        public enum WhiteSubStateID { Magenta, Grey }

        public MeshRenderer mesh;
        public Material material;

        public BallColor ColorState;
        public WhiteSubStateID WhiteColorSubState;
        private StateMachine<BallColor> sm;

        // Start is called before the first frame update
        void Awake()
        {
            mesh = GetComponent<MeshRenderer>();
            material = mesh.material;

            //instantiating the state machine
            sm = new StateMachine<BallColor>();

            //Defining the states
            State<BallColor, WhiteSubStateID> whiteState = new State<BallColor, WhiteSubStateID>(BallColor.White);
            State<BallColor> blueState = new State<BallColor>(BallColor.Blue);
            State<BallColor> redState = new State<BallColor>(BallColor.Red);
            State<BallColor> greenState = new State<BallColor>(BallColor.Green);
            State<BallColor> blackState = new State<BallColor>(BallColor.Black);

            State<WhiteSubStateID> magentaState = new State<WhiteSubStateID>(WhiteSubStateID.Magenta);
            State<WhiteSubStateID> greyState = new State<WhiteSubStateID>(WhiteSubStateID.Grey);

            //Defining the OnEnterState action for debug purposes...
            blueState.OnEnterState += () =>
            {
                material.color = Color.blue;
                Debug.Log("Setting the color blue");
            };

            whiteState.OnEnterState += () =>
            {
                material.color = Color.white;
                Debug.Log("Setting the color white");
            };
            
            blackState.OnEnterState += () => 
            { 
                material.color = Color.black;
                Debug.Log("Setting the color black");
            };
            redState.OnEnterState += () => 
            { 
                material.color = Color.red;
                Debug.Log("Setting the color red");
            };
            greenState.OnEnterState += () => 
            { 
                material.color = Color.green;
                Debug.Log("Setting the color green");
            };

            magentaState.OnEnterState += () =>
            {
                material.color = Color.magenta;
                Debug.Log("Setting the color magenta");
            };

            greyState.OnEnterState += () =>
            {
                material.color = Color.gray;
                Debug.Log("Setting the color grey");
            };

            //Define transitions
            var toWhiteTransition = new Transition<BallColor>(BallColor.White, BallColor.White, () => ColorState.Equals(BallColor.White));

            var redToBlue = new Transition<BallColor>(BallColor.Red, BallColor.Blue, () => ColorState.Equals(BallColor.Blue));
            var blueToGreen = new Transition<BallColor>(BallColor.Blue, BallColor.Green, () => ColorState.Equals(BallColor.Green));
            var greenToBlack = new Transition<BallColor>(BallColor.Green, BallColor.Black, () => ColorState.Equals(BallColor.Black));
            var greenToRed = new Transition<BallColor>(BallColor.Green, BallColor.Red, () => ColorState.Equals(BallColor.Red));
            var blackToBlue = new Transition<BallColor>(BallColor.Black, BallColor.Blue, () => ColorState.Equals(BallColor.Blue));
            var blackToGreen = new Transition<BallColor>(BallColor.Black, BallColor.Green, () => ColorState.Equals(BallColor.Green));
            var whiteToBlue = new Transition<BallColor>(BallColor.White, BallColor.Blue, () => ColorState.Equals(BallColor.Blue));
            var whiteToGreen = new Transition<BallColor>(BallColor.White, BallColor.Green, () => ColorState.Equals(BallColor.Green));
            var whiteToRed = new Transition<BallColor>(BallColor.White, BallColor.Green, () => ColorState.Equals(BallColor.Red));
            var whiteToBlack = new Transition<BallColor>(BallColor.White, BallColor.Black, () => ColorState.Equals(BallColor.Black));

            //Sub state transitions
            var white_magentaToGrey = new Transition<WhiteSubStateID>(WhiteSubStateID.Magenta, WhiteSubStateID.Grey, () => WhiteColorSubState.Equals(WhiteSubStateID.Grey));
            var white_greyToMagenta = new Transition<WhiteSubStateID>(WhiteSubStateID.Grey, WhiteSubStateID.Magenta, () => WhiteColorSubState.Equals(WhiteSubStateID.Magenta));

            //Adding sub states to parent state...
            whiteState.AddState(magentaState);
            whiteState.AddState(greyState);

            //Add sub state transitions to parent state
            whiteState.AddTransition(white_greyToMagenta);
            whiteState.AddTransition(white_magentaToGrey);

            //Add states to Root State Machine
            sm.AddState(blueState);
            sm.AddState(whiteState);
            sm.AddState(redState);
            sm.AddState(greenState);
            sm.AddState(blackState);

            //Add all the transitions            
            sm.AddAnyTransition(toWhiteTransition); //all states will be able to switch to the white state

            sm.AddTransition(redToBlue);
            sm.AddTransition(blueToGreen);
            sm.AddTransition(greenToBlack);
            sm.AddTransition(greenToRed);
            sm.AddTransition(blackToBlue);
            sm.AddTransition(blackToGreen);
            sm.AddTransition(whiteToRed);
            sm.AddTransition(whiteToBlue);
            sm.AddTransition(whiteToGreen);
            sm.AddTransition(whiteToBlack);

            //Initialize the state machine
            sm.SetInitialState(BallColor.White); //initial state is defaulted to firstly added State
            sm.Init(null);
        }


        private void OnEnable()
        {
            //Call EnterState
            sm.EnterState();
        }

        private void OnDisable()
        {
            //Call exit state
            sm.ExitState();
        }


        void Update()
        {
            /* Update the state every frame (if active)
             * Transitions will be checked in this frame
             */
            sm.UpdateState();
        }

        private void LateUpdate()
        {
            /* Late Update the state after every frame (if active)
             */
            sm.LateUpdateState();
        }

        private void FixedUpdate()
        {
            /* Fixed Update the state after fixed (physics) frame (if active)
             */
            sm.FixedUpdateState();
        }



    }

    public class BallStateMachine : StateMachine<Ball.BallColor>
    {


    }

    public class BallState : State<Ball.BallColor>
    {
        public BallState(Ball.BallColor id) : base(id)
        {

        }



    }

}
