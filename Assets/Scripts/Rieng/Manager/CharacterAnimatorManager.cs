using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimatorManager : MonoBehaviour
{
    public Animator animator;
    protected CharacterManager characterManager;
    protected CharacterStatsManager characterStatsManager;
    public bool canRotate;

    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandIKConstraint;
    public TwoBoneIKConstraint rightHandIKConstraint;

    bool handIKWeightReset = false;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("canRotate", canRotate);
        animator.SetBool("isInteracting", isInteracting);
        animator.SetBool("isMirrored", mirrorAnim);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isRotatingWithRootMotion", true);
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public virtual void CanRotate()
    {
        animator.SetBool("canRotate", true);
    }

    public virtual void StopRotation()
    {
        animator.SetBool("canRotate", false);
    }

    public virtual void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public virtual void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public virtual void EnableIsInvulnerable()
    {
        animator.SetBool("isInvulnerable", true);
    }

    public virtual void DisableIsInvulnerable()
    {
        animator.SetBool("isInvulnerable", false);
    }

    public virtual void EnableIsParrying()
    {
        characterManager.isParrying = true;
    }

    public virtual void DisableIsParrying()
    {
        characterManager.isParrying = false;
    }

    public virtual void EnableCanBeRiposted()
    {
        characterManager.canBeRiposted = true;
    }

    public virtual void DisableCanBeRiposted()
    {
        characterManager.canBeRiposted = false;
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {
        characterStatsManager.TakeDamageNoAnimation(characterManager.pendingCriticalDamage, 0);
        characterManager.pendingCriticalDamage = 0;
    }

    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
    {
        if (isTwoHandingWeapon)
        {
            if (rightHandTarget != null)
            {
                rightHandIKConstraint.data.target = rightHandTarget.transform;
                rightHandIKConstraint.data.targetPositionWeight = 1;
                rightHandIKConstraint.data.targetRotationWeight = 1;
            }

            if (leftHandTarget != null)
            {
                leftHandIKConstraint.data.target = leftHandTarget.transform;
                leftHandIKConstraint.data.targetPositionWeight = 1;
                leftHandIKConstraint.data.targetRotationWeight = 1;
            }
        }
        else
        {
            rightHandIKConstraint.data.target = null;
            leftHandIKConstraint.data.target = null;
        }

        rigBuilder.Build();
    }

    public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
    {
        if (characterManager.isInteracting)
            return;

        if (handIKWeightReset)
        {
            handIKWeightReset = false;

            if (rightHandIKConstraint.data.target != null)
            {
                rightHandIKConstraint.data.target = rightHandIK.transform;
                rightHandIKConstraint.data.targetPositionWeight = 1;
                rightHandIKConstraint.data.targetRotationWeight = 1;
            }

            if (leftHandIKConstraint.data.target != null)
            {
                leftHandIKConstraint.data.target = leftHandIK.transform;
                leftHandIKConstraint.data.targetPositionWeight = 1;
                leftHandIKConstraint.data.targetRotationWeight = 1;
            }
        }
    }

    public virtual void EraseHandIKForWeapon()
    {
        handIKWeightReset = true;

        if (rightHandIKConstraint.data.target != null)
        {
            rightHandIKConstraint.data.targetPositionWeight = 0;
            rightHandIKConstraint.data.targetRotationWeight = 0;
        }

        if (leftHandIKConstraint.data.target != null)
        {
            leftHandIKConstraint.data.targetPositionWeight = 0;
            leftHandIKConstraint.data.targetRotationWeight = 0;
        }
    }

}
