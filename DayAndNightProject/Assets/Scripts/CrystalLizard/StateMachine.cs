using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine
{
    public event Action<State> OnStateChange;
    public State _currentState {get; private set;}
    public enum State {
        Sleeping, Idle, Escaping, Approaching
    };

    public void ChangeState(State newState) {
        _currentState = newState;
        OnStateChange?.Invoke(_currentState);
    }
}
