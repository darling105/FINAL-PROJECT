using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager playerManager;
    PlayerStats playerStats;

    PlayerInputManager playerInputManager;
    PlayerLocomotion playerLocomotion;
    int vertical;
    int horizontal;


    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        anim = GetComponent<Animator>();
        playerInputManager = GetComponentInParent<PlayerInputManager>();
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");

    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        #endregion

        #region Horizontal
        float h = 0;
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate()
    {
        anim.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
        anim.SetBool("canRotate", false);
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }

    public void EnableIsInvulnerable()
    {
        anim.SetBool("isInvulnerable", true);
    }

    public void DisableIsInvulnerable()
    {
        anim.SetBool("isInvulnerable", false);
    }

    public void EnableIsParrying()
    {
        playerManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        playerManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        playerManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
        playerManager.canBeRiposted = false;
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        playerStats.TakeDamageNoAnimation(playerManager.pendingCriticalDamage);
        playerManager.pendingCriticalDamage = 0;
    }

    public void EnableCollision()
    {
        playerLocomotion.characterCollider.enabled = true;
        playerLocomotion.characterCollisionBlockerCollider.enabled = true;
    }

    public void DisableCollision()
    {
        playerLocomotion.characterCollider.enabled = false;
        playerLocomotion.characterCollisionBlockerCollider.enabled = false;
    }

    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false)
        {
            return;
        }
        float delta = Time.deltaTime;
        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
    }

}

