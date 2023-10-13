using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StateMachine
{
    public event Action<State> OnStateChange;
    public State CurrentState {get; private set;}
    public enum State {
        Sleeping, Idle, Escaping, Approaching
    };

    public StateMachine() {
        this.CurrentState = State.Sleeping;
    }

    public void DirectlyChangeState(State newState) {
        if(CurrentState != newState) {
            CurrentState = newState;
            OnStateChange?.Invoke(CurrentState);
        }
    }

    public void ChangeStateBasedOnGlow(GlowEffectManager.GlowType newGlow) {
        State newState = 0;

        switch (newGlow) {
            case GlowEffectManager.GlowType.Sun:
                newState = State.Approaching;
            break;
            case GlowEffectManager.GlowType.Moon:
                newState = State.Escaping;
            break;
            case GlowEffectManager.GlowType.Twilight:
                newState = State.Idle;
            break;
            default:
                newState = State.Sleeping;
            break;
        }

        DirectlyChangeState(newState);
    }
}
