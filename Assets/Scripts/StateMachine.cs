using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachine : MonoBehaviour
{
    public float RotationPower;
    private Vector2 _look;
    private StateFactory _state;
    private BaseState _currentState;
    public BaseState CurrentState{get {return _currentState;} set{_currentState = value;}}
    
    public Animator Anim ;
    [Header("Movement")]
    public CharacterController _characterController;
    public float MoveSpeed;
    public float CanAttack ;
    public Vector2 Dir ;
    public GameObject FollowTarget;
    
    private void Awake() {
        //Anim = GetComponent<Animator>();
        _state = new StateFactory(this);
    }

    private void Start() {
        CurrentState = _state.Idle();
        //CurrentState.EnterState();
    }
    public void OnMove(InputValue value){
        
        Dir = value.Get<Vector2>();
        Debug.Log("On move");
    }
    public void OnAttack(InputValue value){
        
        CanAttack = value.Get<float>();
        Debug.Log("On attack");
    }
    public void OnCameraRotate(InputValue value){
        _look = value.Get<Vector2>();
    }

    private void Update() {
        //Debug.Log("root motion: " + Anim.applyRootMotion);
        RotateCam();
        CurrentState.UpdateState();
    }

    public bool AnimatorIsPlaying(float ratio = 0.9f){
        return Anim.GetCurrentAnimatorStateInfo(0).normalizedTime <ratio;
    }
    private void RotateCam(){
        FollowTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * RotationPower, Vector3.up);
        FollowTarget.transform.rotation *= Quaternion.AngleAxis(_look.y * RotationPower, Vector3.right);

        var angles = FollowTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = FollowTarget.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        FollowTarget.transform.localEulerAngles = angles;
    }
    
    
}
