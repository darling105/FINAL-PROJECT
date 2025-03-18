using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRushIn : BossBaseState
{
    public BossRushIn(BossStateMachine currentContext,BossStateFactory stateFactory):base(currentContext,stateFactory){
        
    }
    public override void CheckSwitchState()
    {
        if(!_context.TheAnimator.AnimationIsPlaying(0.71f)){
            SwitchState(_factory.NormalAttack());
        }
    }

    public override void EnterState()
    {
        _exitState = false;
        //_context.Anim.CrossFade("rushInAttack",0.2f);
        //_context.Anim.Play("rushInAttack");
        _context.TheAnimator.PlayAnimation("rushInAttack",0);
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
