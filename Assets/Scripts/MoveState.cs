using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(StateMachine currentContext,StateFactory stateFactory):base(currentContext,stateFactory){
        
    }
    public override void EnterState()
    {
        _exitState = false;
        _context.Anim.Play("move");
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        if(_exitState){
            return;
        }
        Vector3 direction = new Vector3(_context.Dir.x,0,_context.Dir.y);
        _context._characterController.Move(direction * _context.MoveSpeed);
        CheckSwitchState();
        
    }
    public override void CheckSwitchState()
    {
        if(_context.Dir == Vector2.zero){
            SwitchState(_factory.Idle());
        }
    }
}
