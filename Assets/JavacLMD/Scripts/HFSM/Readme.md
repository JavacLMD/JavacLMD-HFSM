# Hierarchical Finite State Machine (HFSM) Documentation

## Overview

The Hierarchical Finite State Machine (HFSM) is a design pattern used to manage state transitions in software applications. It provides a structured approach to organizing states and their transitions, making it easier to manage complex behaviors.
It's use case is predefined to be use in [Unity 3D](https://unity.com/)

## Features

- Hierarchical organization of states
- Clear separation of concerns between states
- Easy to add new states and transitions
- Support for concurrent states
- Supports both deterministic and non-deterministic transitions

## Usage

### Installation

```bash
$ git clone [https://github.com/JavacLMD/hfsm.git](https://github.com/JavacLMD/JavacLMD-HFSM.git)
```

### Basic Example

```csharp
using JavacLMD.HFSM;

//Define your states identifier
public enum MovementStateID
{
    IDLE,
    RUN
}

// Define your state machine
public class MyStateMachine : MonoBehaviour
{

    private StateMachine<MovementStateID> fsm;

    //Setup condition
    public bool isRunning = false;

    public void Awake()
    {
        //Define States
        var idleState = new State<MovementStateID>(MovementStateID.IDLE);
        var runState = new State<MovementStateID>(MovementStateID.RUN);

        //Define Transitions -> (From, To, Condition)
        var idleToRunTransition = new Transition(MovementStateID.IDLE, MovementStateID.RUN, () => isRunning);
        var runToIdleTransition = new Transition(MovementStateID.RUN, MovementStateID.IDLE, () => !isRunning);

        //Setup StateMachine
        fsm = new StateMachine<MovementStateID>();

        //Add States
        fsm.AddState(idleState);
        fsm.AddState(runState);

        //Add Transitions
        fsm.AddTransition(idleToRunTransition);
        fsm.AddTransition(runToIdleTransition);

        // Set initial state and initalize
        fsm.SetInitialState(idleState.ID); //Not necessary but useful if needing to change what state the state machine starts in, defaults to the first state added to StateMachine.
        fsm.Init(); //You can pass in another state machine act as the 'parent'.

        fsm.EnterState();
    }

    public void Update()
    {
        // Checks transitions and executes State's Update logic...
        fsm.UpdateState();
    }

    public void LateUpdate()
    {
        //Executes State's LateUpdate logic
        fsm.LateUpdateState();
    }

    public void FixedUpdate()
    {
        //Executes State's FixedUpdate logic
        fsm.FixedUpdateState();
    }
    
}
```

### Advanced Usage

- **Hierarchical States**: States can contain sub-states, enabling hierarchical organization of state logic.
```csharp
using JavacLMD.HFSM;

//Define your states identifier
public enum MovementStateID
{
    GROUNDED,
    FALLING,
}

public enum GroundedStateID {
    IDLE,
    RUN
}

// Define your state machine
public class MyStateMachine : MonoBehaviour
{
    private StateMachine<MovementStateID> fsm;

    public bool isGrounded;
    public bool isRunning;

    public void Awake()
    {
        //Define Parent States
        //* Sub State IDs can be the same as the parent, but for this useage demo I'll keep it separate *//
        var groundedState = new State<MovementStateID, GroundedStateID>(MovementStateID.GROUNDED);
        var fallingState = new State<MovementStateID, GroundedStateID>(MovementStateID.FALLING);

        //Definite Parent State Transitions
        //Define Sub-State Transitions -> (From, To, Condition)
        var groundToFallingTransition = new Transition(MovementStateID.GROUNDED, MovementStateID.FALLING, () => !isGrounded);
        var fallingToGroundedTransition = new Transition(MovementStateID.FALLING, MovementStateID.GROUNDED, () => isGrounded);

        //Define Sub-States
        var idleState = new State<GroundedStateID>(GroundedStateID.IDLE);
        var runState = new State<GroundedStateID>(GroundedStateID.RUN);

        //Define Sub-State Transitions -> (From, To, Condition)
        var idleToRunTransition = new Transition(GroundedStateID.IDLE, GroundedStateID.RUN, () => isRunning);
        var runToIdleTransition = new Transition(GroundedStateID.RUN, GroundedStateID.IDLE, () => !isRunning);

        //Add sub states to Parents
        groundedState.AddState(idleState);
        groundedState.AddState(runState);

        //Add sub state transitions
        groundedState.AddTransition(idleToRunTransition);
        groundedState.AddTransition(runToIdleTransition);

        groundedState.SetInitialState(idleState.ID);

        //Setup StateMachine
        fsm = new StateMachine<MovementStateID>();

        //Add Parent States
        fsm.AddState(groundedState);
        fsm.AddState(fallingState);

        //Add Parent Transitions
        fsm.AddTransition(groundToFallingTransition);
        fsm.AddTransition(fallingToGroundedTransition);

        // Set initial state and initalize
        fsm.SetInitialState(idleState.ID); //Not necessary but useful if needing to change what state the state machine starts in, defaults to the first state added to StateMachine.
        fsm.Init(); //You can pass in another state machine to act as the 'parent'.

        fsm.EnterState();
    }

    public void Update()
    {
        // Checks transitions and executes State's Update logic...
        fsm.UpdateState();
    }

    public void LateUpdate()
    {
        //Executes State's LateUpdate logic
        fsm.LateUpdateState();
    }

    public void FixedUpdate()
    {
        //Executes State's FixedUpdate logic
        fsm.FixedUpdateState();
    }
    
}

```
  
- **Custom Actions**: Define custom actions to be executed on state entry, exit, or during state execution.
```csharp
using JavacLMD.HFSM;

//Define your states identifier
public enum MovementStateID
{
    IDLE,
    RUN
}

// Define your state machine
public class MyStateMachine : MonoBehaviour
{

    private StateMachine<MovementStateID> fsm;

    //Setup condition
    public bool isRunning = false;

    public void Awake()
    {
        //Define States
        var idleState = new State<MovementStateID>(MovementStateID.IDLE);
        var runState = new State<MovementStateID>(MovementStateID.RUN);

        // Setup actions with event delegates
        idleState.OnInit += () => Debug.Log("init");
        idleState.OnEnterState += () => Debug.Log("Enter");
        idleState.OnExitState += () => Debug.Log("Exit");
        idleState.OnUpdateState += () => Debug.Log("Update");
        idleState.OnLateUpdateState += () => Debug.Log("Late");
        idleState.OnFixedUpdateState += () => Debug.Log("Fixed");

        { ... }
    }

    { ... }
    
}
```
## API Reference

### StateMachine

#### Properties

- `TStateID ActiveStateID { get; }` : Returns the State Machine's active state's ID 
- `TStateID PreviousStateID { get; }` : Returns the State Machine's previously active state's ID

#### Methods

- `AddState(IState<TStateID> state)` : Adds a state to the state machine
- `AddTransition(ITransition<TStateID> transition)` : Adds a transition to the state machine
- `AddAnyTransition(ITransition<TStateID> transition)` : Adds a 'global' transition to the state machine (universal transition that can occur from any state)
- `SwitchState(TStateID nextState)` : Called internally in the state machine to handle the changing of states (
- `SetInitialState(TStateID stateID)` : Sets the initial state the state machine enters (defaults to the first state added to state machine)

### State

#### Properties

- `TStateID ID { get; }` : Return's the State's unique identifier (used for cached lookup)

#### Methods

- `Init(IStateMachine<TStateID> parentStateMachine)` : Initializes the State. Called when the state is added to the State Machine
- `EnterState()` : Called when the State Machine enters the state 
- `ExitState()` : Called when the State Machine exits the state
- `UpdateState()` : Called every frame
- `LateUpdateState()` : Called every frame after UpdateState
- `FixedUpdateState()` : Called every fixed (Physics) frame

### Transition

#### Properties

- `TStateID From { get; }` : The State ID that must be active to consider transition (Not valid if the Transition is passed into AddAnyTransition(...)
- `TStateID To { get; }` : The destination State ID if the transition passes.

#### Methods

- `ShouldTransition()`: Checks if the transition is valid.
- `Init(IStateMachine<TStateID> parentSM)`: Initializes the Transition. Called when transition is added to the State Machine.


## Conclusion

The Hierarchical Finite State Machine provides a powerful mechanism for managing state transitions in your C# applications. By organizing states hierarchically and defining clear transitions between them, you can create complex behaviors in a structured and maintainable way.

## Customizeable

If my predefined classes aren't enough, you can define your own using the interfaces!
