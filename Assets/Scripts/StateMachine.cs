using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{
    private StateFactory _state;
    private BaseState _currentState;
    public BaseState CurrentState{get {return _currentState;} set{_currentState = value;}}
    
    public Animator Anim {get;private set;}
    [Header("Movement")]
    public CharacterController _characterController;
    public float MoveSpeed;
    public float CanAttack ;
    public Vector2 Dir ;

    private void Awake() {
        Anim = GetComponent<Animator>();
        _state = new StateFactory(this);
    }

    private void Start() {
        CurrentState = _state.Idle();
        CurrentState.EnterState();
    }
    public void OnMove(InputValue value){
        Dir = value.Get<Vector2>();
        Debug.Log("On move");
    }
    public void OnAttack(InputValue value){
        CanAttack = value.Get<float>();
        Debug.Log("On attack");
    }

    private void Update() {

        //CurrentState.UpdateState();
    }

    public bool AnimatorIsPlaying(float ratio = 0.9f){
        return Anim.GetCurrentAnimatorStateInfo(0).normalizedTime <ratio;
    }
}
