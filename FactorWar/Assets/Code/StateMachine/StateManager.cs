using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : SingletonComponent<StateManager> {

    public virtual State CurrentState
    {
        get { return _currentState; }
        set { TransitionTo(value); }
    }

    protected State _currentState;
    protected bool _inTransition;

    public virtual T GetState<T> () where T : State
    {
        T target = GetComponent<T>();
        if(target == null)
        {
            target = gameObject.AddComponent<T>();
        }
        return target;
    }

    public virtual void ChangeState<T> () where T : State {
        CurrentState = GetState<T>();
    }

    protected virtual void TransitionTo (State value)
    {
        if(_currentState == value || _inTransition)
        {
            return;
        }
        _inTransition = true;

        if(_currentState != null) {
            _currentState.Exit();
        }

        _currentState = value;

        if(_currentState != null)
        {
            _currentState.Init();
        }

        _inTransition = false;
    }
}
