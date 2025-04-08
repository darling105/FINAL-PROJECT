using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;
    int vertical;
    int horizontal;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
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

        player.animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        player.animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void EnableCollision()
    {
        player.playerLocomotionManager.characterCollider.enabled = true;
        player.playerLocomotionManager.characterCollisionBlockerCollider.enabled = true;
    }

    public void DisableCollision()
    {
        player.playerLocomotionManager.characterCollider.enabled = false;
        player.playerLocomotionManager.characterCollisionBlockerCollider.enabled = false;
    }

    public void AwardShadesOnDeath()
    {

    }

    private void OnAnimatorMove()
    {
        if (character.isInteracting == false)
            return;

        float delta = Time.deltaTime;
        player.playerLocomotionManager.rigidbody.drag = 0;
        Vector3 deltaPosition = player.animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        player.playerLocomotionManager.rigidbody.velocity = velocity;
    }

}

