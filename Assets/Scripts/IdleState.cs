using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine currentContext,StateFactory stateFactory):base(currentContext,stateFactory){
        
    }
    public override void EnterState()
    {
        _context.Anim.Play("idle");
        _exitState = false;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        if(_exitState){
            return;
        }
    }
    public override void CheckSwitchState()
    {
        if(_context.CanAttack == 1){
            SwitchState(_factory.NormalAttack());
        }else if(_context.Dir != Vector2.zero){
            SwitchState(_factory.Move());
        }
    }
}
