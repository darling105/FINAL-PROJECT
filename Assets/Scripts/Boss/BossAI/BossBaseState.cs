using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState
{
    protected BossStateMachine _context;
    protected BossStateFactory _factory;
    protected bool _exitState;

    public BossBaseState(BossStateMachine currentContext,BossStateFactory stateFactory){
        this._context = currentContext;
        this._factory = stateFactory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    public abstract void CheckSwitchState();

    protected void SwitchState(BossBaseState newState){
        _exitState =true;
        ExitState();

        newState.EnterState();
        _context.CurrentState = newState;
        
    }
}
