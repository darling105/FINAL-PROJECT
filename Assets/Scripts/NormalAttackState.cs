using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackState : BaseState
{
    int _currentAttackPhase = 0;
    int _maxAttackPhase = 3;
    string _attackName = "attack";
    bool _continueAttack = false;
    public NormalAttackState(StateMachine currentContext,StateFactory stateFactory):base(currentContext,stateFactory){
        
    }
    public override void EnterState()
    {
        _context.CanAttack = 0;
        _continueAttack = false;
        _currentAttackPhase += 1;
        _exitState = false;
        _context.Anim.Play(_attackName + _currentAttackPhase);
    }

    public override void ExitState()
    {
        if(_currentAttackPhase == _maxAttackPhase){
            _currentAttackPhase = 0;
        }
    }

    public override void UpdateState()
    {
        if( (0 < _currentAttackPhase && _currentAttackPhase <_maxAttackPhase)  && _context.CanAttack == 1 && _context.AnimatorIsPlaying()  ){
            _continueAttack = true;
        }
        CheckSwitchState();
        if(_exitState){
            return;
        }
    }
    public override void CheckSwitchState(){
        if(_continueAttack && !_context.AnimatorIsPlaying() ){
            SwitchState(_factory.NormalAttack());
        }
        else if(!_continueAttack && !_context.AnimatorIsPlaying(1)){
            _currentAttackPhase = 0;
            SwitchState(_factory.Idle());
        }
    }
}
