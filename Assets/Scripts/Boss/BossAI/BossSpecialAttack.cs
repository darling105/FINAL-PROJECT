using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpecialAttack : BossBaseState
{
    public BossSpecialAttack(BossStateMachine currentContext,BossStateFactory stateFactory):base(currentContext,stateFactory){
        
    }
    public override void CheckSwitchState()
    {
        if(!_context.TheAnimator.AnimationIsPlaying()){
            SwitchState(_factory.Idle());
        }
    }

    public override void EnterState()
    {
        _exitState = false;
        //_context.Anim.CrossFade("specialAttack",0.3f);
        //_context.Anim.Play("specialAttack");
        _context.TheAnimator.PlayAnimation("specialAttack");
    }

    public override void ExitState()
    {
        _exitState = true;
    }

    public override void UpdateState()
    {
        if(_exitState){
            return;
        }
        CheckSwitchState();
    }
}
