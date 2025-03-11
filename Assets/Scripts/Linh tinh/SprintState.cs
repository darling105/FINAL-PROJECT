using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : BaseState
{
    bool _playSprint = false;
    public SprintState(StateMachine currentContext,StateFactory stateFactory):base(currentContext,stateFactory){
        
    }
    public override void EnterState()
    {
        _exitState = false;
        _context.Anim.Play("sprint");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
        
    }
    public override void CheckSwitchState(){
        if(_context.Dir.x == 0 && _context.Dir.y == 0){
            SwitchState(_factory.Idle());
        }
    }
}
