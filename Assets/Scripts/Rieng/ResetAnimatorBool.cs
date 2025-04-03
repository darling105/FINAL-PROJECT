using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isUsingRightHandBool = "isUsingRightHand";
    public bool isUsingRightHandStatus = false;

    public string isUsingLeftHandBool = "isUsingLeftHand";
    public bool isUsingLeftHandStatus = false;

    public string isInvulnerableBool = "isInvulnerable";
    public bool isInvulnerableStatus = false;

    public string isInteractingBool = "isInteracting";
    public bool isInteractingStatus = false;

    public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
    public bool isRotatingWithRootMotionStatus = false;

    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isUsingRightHandBool, isUsingRightHandStatus);
        animator.SetBool(isUsingLeftHandBool, isUsingLeftHandStatus);
        animator.SetBool(isInvulnerableBool, isInvulnerableStatus);
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
    }
}
