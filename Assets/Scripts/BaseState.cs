using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected StateMachine _context;
    protected StateFactory _factory;
    protected bool _exitState;

    public BaseState(StateMachine currentContext,StateFactory stateFactory){
        this._context = currentContext;
        this._factory = stateFactory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    public abstract void CheckSwitchState();

    protected void SwitchState(BaseState newState){
        _exitState =true;
        ExitState();

        newState.EnterState();
        _context.CurrentState = newState;
        
    }
}
