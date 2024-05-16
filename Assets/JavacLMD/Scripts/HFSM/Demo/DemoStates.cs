using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JavacLMD.HFSM
{
    public class DemoStates : MonoBehaviour
    {

        StateMachine<string> stateMachine;

        public bool IsGrounded;
        public bool IsSwimming;  

        // Start is called before the first frame update
        void Start()
        {
            stateMachine = new StateMachine<string>();

            //Setup states

            /* Grounded */
            var idleState = new State<string>("Idle");
            idleState.OnEnterState += () => Debug.Log("I am now Idle!");
            idleState.OnExitState += () => Debug.Log("I am no longer Idle!");

            var walkState = new State<string>("Walk");
            walkState.OnEnterState += () => Debug.Log("I am now Walking!");
            walkState.OnExitState += () => Debug.Log("I decided against Walking!");

            var runState = new State<string>("Run");
            runState.OnEnterState += () => Debug.Log("Entering Run State");
            runState.OnExitState += () => Debug.Log("Exiting Run State");

            var crouchState = new State<string>("Crouch");
            crouchState.OnEnterState += () => Debug.Log("Entering Crouch State");
            crouchState.OnExitState += () => Debug.Log("Exiting Crouch State");

            var crawlState = new State<string>("Crawl");
            crawlState.OnEnterState += () => Debug.Log("Entering Crouch State");
            crawlState.OnExitState += () => Debug.Log("Exiting Crouch State");

            /* Swimming */
            var swimIdle = new State<string>("Swim Idle");
            swimIdle.OnEnterState += () => Debug.Log("Entering Swim Idle State");
            swimIdle.OnExitState += () => Debug.Log("Exiting Swim Idle State");
            
            var swimMove = new State<string>("Swim Move");
            swimMove.OnEnterState += () => Debug.Log("Entering Swim Move State");
            swimMove.OnExitState += () => Debug.Log("Exiting Swim Move State");
            /* Falling */


            var fallIdleState = new State<string>("FallIdle");
            fallIdleState.OnEnterState += () => Debug.Log("Entering FallIdle State");
            fallIdleState.OnExitState += () => Debug.Log("Exiting FallIdle State");

            var fallMoveState = new State<string>("FallMove");
            fallMoveState.OnEnterState += () => Debug.Log("Entering FallMove State");
            fallMoveState.OnExitState += () => Debug.Log("Exiting FallMove State");


            //Setup Parent States

            var GroundedState = new State<string, string>("Grounded");
            GroundedState.OnEnterState += () => Debug.Log("Entering Grounded State");
            GroundedState.OnExitState += () => Debug.Log("Exiting Grounded State");

            var SwimmingState = new State<string, string>("Swimming");
            SwimmingState.OnEnterState += () => Debug.Log("Entering Swimming State");
            SwimmingState.OnExitState += () => Debug.Log("Exiting Swimming State");

            var FallingState = new State<string, string>("Falling");
            FallingState.OnEnterState += () => Debug.Log("Entering Falling State");
            FallingState.OnExitState += () => Debug.Log("Exiting Falling State");

            //Setup Relationships

            /* Adding sub states to their parents */
            GroundedState.AddState(idleState);
            GroundedState.AddState(walkState);
            GroundedState.AddState(runState);
            GroundedState.AddState(crouchState);
            GroundedState.AddState(crawlState);

            SwimmingState.AddState(swimIdle);
            SwimmingState.AddState(swimMove);

            FallingState.AddState(fallIdleState);
            FallingState.AddState(fallMoveState);


            /* Setup transitions */
            GroundedState.AddTransition(new Transition<string>(idleState.ID, walkState.ID, () => IsMoving() && !IsRunning()));
            GroundedState.AddTransition(new Transition<string>(idleState.ID, runState.ID, () => IsMoving() && IsRunning()));
            GroundedState.AddTransition(new Transition<string>(walkState.ID, runState.ID, () => IsRunning()));
            GroundedState.AddTransition(new Transition<string>(runState.ID, walkState.ID, () => !IsRunning())); 
            GroundedState.AddTransition(new Transition<string>(walkState.ID, idleState.ID, () => !IsMoving())); 
            GroundedState.AddTransition(new Transition<string>(runState.ID, idleState.ID, () => !IsMoving()));
            GroundedState.AddAnyTransition(new Transition<string>("", crouchState.ID, () => IsCrouching() && !IsCrawling()));
            GroundedState.AddAnyTransition(new Transition<string>("", crawlState.ID, () => !IsCrouching() && IsCrawling()));
            GroundedState.AddTransition(new Transition<string>(crouchState.ID, idleState.ID, () => !IsCrouching() && !IsCrawling()));
            GroundedState.AddTransition(new Transition<string>(crouchState.ID, crawlState.ID, () => !IsCrouching() && IsCrawling()));
            GroundedState.AddTransition(new Transition<string>(crawlState.ID, idleState.ID, () => !IsCrouching() && !IsCrawling()));
            GroundedState.AddTransition(new Transition<string>(crawlState.ID, crouchState.ID, () => IsCrouching() && !IsCrawling()));
            GroundedState.SetInitialState(idleState.ID);

            SwimmingState.AddTransition(new Transition<string>(swimMove.ID, swimIdle.ID, () => !IsMoving()));
            SwimmingState.AddTransition(new Transition<string>(swimIdle.ID, swimMove.ID, () => IsMoving()));
            SwimmingState.SetInitialState(swimIdle.ID);

            FallingState.AddTransition(new Transition<string>(fallIdleState.ID, fallMoveState.ID, () => IsMoving()));
            FallingState.AddTransition(new Transition<string>(fallMoveState.ID, fallIdleState.ID, () => !IsMoving()));
            FallingState.SetInitialState(fallIdleState.ID);


            // Driving state machine!

            stateMachine.AddState(GroundedState);
            stateMachine.AddState(FallingState);
            stateMachine.AddState(SwimmingState);

            /* Setup Parent state transitions */
            stateMachine.AddAnyTransition(new Transition<string>("", GroundedState.ID, () => IsGrounded && !IsSwimming));
            stateMachine.AddAnyTransition(new Transition<string>("", FallingState.ID, () => !IsGrounded && !IsSwimming));
            stateMachine.AddAnyTransition(new Transition<string>("", SwimmingState.ID, () => IsSwimming));

            stateMachine.SetInitialState(GroundedState.ID);
            stateMachine.Init(null); //setting null because this state machine doesn't have a parent state machine.     

            stateMachine.EnterState();
        }

        // Update is called once per frame
        void Update()
        {
            // 
            stateMachine.UpdateState();
        }

        void LateUpdate()
        {
            // 
            stateMachine.LateUpdateState();
        }

        void FixedUpdate()
        {
            // 
            stateMachine.LateUpdateState();
        }

        bool IsMoving()
        {
            Vector2 vector = Vector2.zero;
            vector.x = Input.GetAxisRaw("Horizontal");
            vector.y = Input.GetAxisRaw("Vertical");
            vector = Vector2.ClampMagnitude(vector, 1);

            return vector.sqrMagnitude > 0;
        }

        bool IsRunning()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        bool IsCrouching()
        {
            return Input.GetKey(KeyCode.LeftControl);
        }

        bool IsCrawling()
        {
            return Input.GetKey(KeyCode.LeftAlt);
        }


    }
}
