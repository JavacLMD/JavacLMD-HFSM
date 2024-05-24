# Javac-HFSM (Hierarchical Finite State Machine)

Hierarchical finite state machines (or 'HFSM') are finite state machines (or just 'FSM') whose states themselves can be other state machines.
This HFSM solution allows for scalable and customisable states and statemachines. Unity's own Animator is a HFSM!

## Why Use Javac-HFSM?

- **Easy** and **Straightforward**
- Helps you write **self documenting code**, that is **maintainable** and **readable**
- Completely **Customizable**, use the existing State class or use your own with the interfaces!
- Efficiency is key! States are **cached** within it's parent State Machine!
- **Reduces boilerplate** code!
- **Event-Drivable** with GameEvents

## Why not a normal Finite State Machine?

Nothing states (haha get it?) you can't achieve what you want with a normal FSM. With this HFSM structure, we enable you to quickly create your state without excessive boilerplate coding.

An example:

Let's start by defining what kind of states a character controller would have...
- Walk
- Run
- Crouch
- Crawl
- Fall
- Swim 

Right away we see some States that share 1 core detail, the ground.
In a normal FSM structure, we would have to write all the ground related movement OVER and OVER again. Setting up every transition more than once per Walk, Run, Crouch.
Instead, with a State being StateMachine, we can define the **Ground** state. While we're at it, we can define some of the other core states as well...

The hierachy shift:
- Central Parent State Machine
    - **Ground**
        - Walk
        - Run
        - Crouch
        - Crawl
    - **Fall**
        - Idle
        - Move
    - **Swim**
        - Submerged
            - Idle
            - Move
        - Surfaced
            - Idle
            - Move

Now if the Character loses contact with the ground, the number of transitions you have to maintain are minimized. Ground to Fall, Ground to Swim, Fall to Ground, Fall to Swim, Swim to Ground, etc..

## What makes this **Event-Drivable**?

With the use of the **ActionState** and **ActionStateMachine**, you can define a series of Transitions to be checked upon a triggered action type by simply passing in the unique **EventId** given.
This applies to the currently active state or globally, if defined... If there is a GameEvent action that needs to occur instead, you can trigger the event instead!

Example(s):
- Triggering Jump State
- Triggering Landing State and specific animation varying from heights...
- Dropping items per effect received...

## Customizeable

If my predefined classes aren't enough, you can define your own using the interfaces!
