using System.Collections;
using System.Collections.Generic;


public class StateFactory
{
    private StateMachine _context;
    public StateFactory(StateMachine currentContext){
        _context = currentContext;
    }

    private IdleState _idleState;
    private SprintState _sprintState;
    private NormalAttackState _normalAttack;
    private MoveState _moveState;
    private JumpState _jumpState;
    private FallState _fallState;
    public BaseState Idle(){
        return _idleState ??= new IdleState(_context,this);
    }
    public BaseState NormalAttack(){
        return _normalAttack ??= new NormalAttackState(_context,this);
    }
    public BaseState Sprint(){
        return _sprintState ??= new SprintState(_context,this);
    }
    public BaseState Move(){
        return _moveState ??= new MoveState(_context,this);
    }
    public BaseState Jump(){
        return _jumpState ??= new JumpState(_context,this);
    }
    public BaseState Fall(){
        return _fallState ??= new FallState(_context,this);
    }
}
