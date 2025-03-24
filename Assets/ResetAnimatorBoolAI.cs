using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBoolAI : ResetAnimatorBool
{
    public string isPhaseShiftingBool = "isPhaseShifting";
    public bool isPhaseShiftingStatus = false;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(isPhaseShiftingBool, isPhaseShiftingStatus);
    }
}
