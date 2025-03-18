using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : BossBaseState
{
    
    public BossRun(BossStateMachine currentContext,BossStateFactory stateFactory):base(currentContext,stateFactory){
        
    }
    int indexOfState;
    
    public override void CheckSwitchState()
    {
        // if(_context._canDoMeleeAttack){
        //     SwitchState(_factory.NormalAttack());
        // }else{
        //     if(_context._canRushInAttack){
        //         SwitchState(_factory.RushInAttack());
        //     }
        // }

        
        if(indexOfState == 0){
            if(_context._canDoMeleeAttack){
                SwitchState(_factory.NormalAttack());
            }
        }else{
            if(_context._canRushInAttack){
                SwitchState(_factory.RushInAttack());
            }
        }
    }

    public override void EnterState()
    {
        (BossBaseState action, double probability)[] attackActions = {
            (_factory.NormalAttack(), 0.6),  
            (_factory.RushInAttack(),  0.4)
        };
        _factory.GetRandomOutcome(attackActions,out indexOfState);
        Debug.Log("the index:" + indexOfState);
        _exitState = false;
        //_context.Anim.CrossFade("run",0.2f);
        //_context.Anim.Play("run");
        _context.TheAnimator.PlayAnimation("run");
        _context._bossAgent.speed = 3;
        _context._bossAgent.SetDestination(_context.PlayerPosition.position);
    }

    public override void ExitState()
    {
        _exitState = true;
        _context.TurnOffNavMesh();
    }

    public override void UpdateState()
    {
        if(_exitState){
            Debug.Log("Exit state is true");
            return;
        }
        
        CheckSwitchState();
    }
}
