using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager playerManager;

    public PlayerInputManager playerInputManager;
    public PlayerLocomotion playerLocomotion;
    int vertical;
    int horizontal;
    public bool canRotate;

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
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
        canRotate = true;
    }

    public void StopRotation()
    {
        canRotate = false;
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

