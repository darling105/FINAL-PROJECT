using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    public Animator anim;

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }
    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }

}
