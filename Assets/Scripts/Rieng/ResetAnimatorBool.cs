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

    public string isMirroredBool = "isMirrored";
    public bool isMirroredStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterManager characterManager = animator.GetComponent<CharacterManager>();

        characterManager.isUsingLeftHand = false;
        characterManager.isUsingRightHand = false;
        
        animator.SetBool(isInvulnerableBool, isInvulnerableStatus);
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isMirroredBool, isMirroredStatus);
    }
}
