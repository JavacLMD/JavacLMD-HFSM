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

        public MeshRenderer mesh;
        public Material material;

        public BallColor ColorState;
        private StateMachine<BallColor> sm;

        // Start is called before the first frame update
        void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            material = mesh.material;


            sm = new StateMachine<BallColor>();

            State<BallColor> blueState = new State<BallColor>(BallColor.Blue);
            State<BallColor> whiteState = new State<BallColor>(BallColor.White);
            State<BallColor> redState = new State<BallColor>(BallColor.Red);
            State<BallColor> greenState = new State<BallColor>(BallColor.Green);
            State<BallColor> blackState = new State<BallColor>(BallColor.Black);

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


            //Add states to State Machine
            sm.AddState(blueState);
            sm.AddState(whiteState);
            sm.AddState(redState);
            sm.AddState(greenState);
            sm.AddState(blackState);

            //Any state can transition to white 
            sm.AddAnyTransition(new Transition<BallColor>(BallColor.White, BallColor.White, () => ColorState.Equals(BallColor.White)));

            //Red can transition to blue
            sm.AddTransition(new Transition<BallColor>(BallColor.Red, BallColor.Blue, () => ColorState.Equals(BallColor.Blue)));

            //Blue can transition to green
            sm.AddTransition(new Transition<BallColor>(BallColor.Blue, BallColor.Green, () => ColorState.Equals(BallColor.Green)));

            //Green can transition to Red and Black
            sm.AddTransition(new Transition<BallColor>(BallColor.Green, BallColor.Black, () => ColorState.Equals(BallColor.Black)));
            sm.AddTransition(new Transition<BallColor>(BallColor.Green, BallColor.Red, () => ColorState.Equals(BallColor.Red)));

            //Black can transition to blue and green
            sm.AddTransition(new Transition<BallColor>(BallColor.Black, BallColor.Blue, () => ColorState.Equals(BallColor.Blue)));
            sm.AddTransition(new Transition<BallColor>(BallColor.Black, BallColor.Green, () => ColorState.Equals(BallColor.Green)));

            //White can transition to blue, green, red, and black
            sm.AddTransition(new Transition<BallColor>(BallColor.White, BallColor.Blue, () => ColorState.Equals(BallColor.Blue)));
            sm.AddTransition(new Transition<BallColor>(BallColor.White, BallColor.Green, () => ColorState.Equals(BallColor.Green)));
            sm.AddTransition(new Transition<BallColor>(BallColor.White, BallColor.Red, () => ColorState.Equals(BallColor.Red)));
            sm.AddTransition(new Transition<BallColor>(BallColor.White, BallColor.Black, () => ColorState.Equals(BallColor.Black)));

            sm.SetInitialState(BallColor.White);
            sm.Init(null);
            sm.EnterState();
        }

        private void OnDisable()
        {
            sm.ExitState();
        }


        // Update is called once per frame
        void Update()
        {
            sm.UpdateState();
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
