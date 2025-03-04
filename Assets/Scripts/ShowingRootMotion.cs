using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShowingRootMotion : MonoBehaviour
{
    public CharacterController Controller;
    private Animator Anim;
    void Awake()
    {
        Anim = GetComponent<Animator>();
    }
    void OnAnimatorMove()
    {
        Debug.Log("OnAnimatorMove");
        if (Anim && Controller)
        {
            Vector3 rootMotionMove = Anim.deltaPosition;
            rootMotionMove.y = 0; // Keep movement only on the XZ plane to avoid gravity issues
            Controller.Move(rootMotionMove);
        }
    }
}
