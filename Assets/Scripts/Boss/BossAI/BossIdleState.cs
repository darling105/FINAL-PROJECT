using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine currentContext, BossStateFactory stateFactory) : base(currentContext, stateFactory)
    {

    }
    int indexOfState;
    private bool _isWaiting = true;
    public override void CheckSwitchState()
    {
        (BossBaseState action, double probability)[] actions = {
            (_factory.NormalAttack(), 0.8),
            (_factory.SpecialAttack(),  0.2)

        };

        (BossBaseState action, double probability)[] actions2 = {
            (_factory.RushInAttack(), 0.3),
            (_factory.SpecialAttack(),  0.2),
            (_factory.Run(),  0.5)

        };


        if (_context._canDoMeleeAttack)
        {
            Debug.Log("can do melee attack");
            SwitchState(_factory.GetRandomOutcome(actions, out indexOfState));
        }
        else
        {

            SwitchState(_factory.Run());
        }
    }

    public override void EnterState()
    {
        if (_context.CurrentState != _factory.Idle())
        {
            //_context.Anim.CrossFade("idle",1);
            //_context.Anim.Play("idle");
            _context.TheAnimator.PlayAnimation("idle");
            Debug.Log("enter the idle state");
        }
        _exitState = false;


        _context.StartCoroutine(WaitBeforeSwitch());


    }

    public override void ExitState()
    {
        _exitState = true;


    }

    public override void UpdateState()
    {
        if (_exitState)
        {
            return;
        }
        //CheckSwitchState();
    }

    private IEnumerator WaitBeforeSwitch()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(1.5f);  // Wait for 1.5 seconds
        _isWaiting = false;
        CheckSwitchState();
    }
}
