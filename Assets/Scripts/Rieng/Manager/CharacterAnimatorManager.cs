using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimatorManager : MonoBehaviour
{
    protected CharacterManager character;

    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandIKConstraint;
    public TwoBoneIKConstraint rightHandIKConstraint;

    bool handIKWeightReset = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
    {
        character.animator.applyRootMotion = isInteracting;
        character.animator.SetBool("canRotate", canRotate);
        character.animator.SetBool("isInteracting", isInteracting);
        character.animator.SetBool("isMirrored", mirrorAnim);
        character.animator.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
        character.animator.applyRootMotion = isInteracting;
        character.animator.SetBool("isRotatingWithRootMotion", true);
        character.animator.SetBool("isInteracting", isInteracting);
        character.animator.CrossFade(targetAnim, 0.2f);
    }

    public virtual void CanRotate()
    {
        character.animator.SetBool("canRotate", true);
    }

    public virtual void StopRotation()
    {
        character.animator.SetBool("canRotate", false);
    }

    public virtual void EnableCombo()
    {
        character.animator.SetBool("canDoCombo", true);
    }

    public virtual void DisableCombo()
    {
        character.animator.SetBool("canDoCombo", false);
    }

    public virtual void EnableIsInvulnerable()
    {
        character.animator.SetBool("isInvulnerable", true);
    }

    public virtual void DisableIsInvulnerable()
    {
        character.animator.SetBool("isInvulnerable", false);
    }

    public virtual void EnableIsParrying()
    {
        character.isParrying = true;
    }

    public virtual void DisableIsParrying()
    {
        character.isParrying = false;
    }

    public virtual void EnableCanBeRiposted()
    {
        character.canBeRiposted = true;
    }

    public virtual void DisableCanBeRiposted()
    {
        character.canBeRiposted = false;
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {
        character.characterStatsManager.TakeDamageNoAnimation(character.pendingCriticalDamage, 0);
        character.pendingCriticalDamage = 0;
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
        if (character.isInteracting)
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
