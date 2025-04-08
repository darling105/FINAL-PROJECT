using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    PlayerManager player;
    public Vector3 moveDirection;
    [HideInInspector] public float moveAmount;

    [Header("Ground And Air Detection Settings")]
    [SerializeField] float groundDetectionRayStartPoint = 0.5f;
    [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] float groundDirectionRayDistance = 0.2f;
    public LayerMask groundCheck;
    public float inAirTimer;

    [Header("Movement Settings")]
    public new Rigidbody rigidbody;
    public GameObject normalCamera;
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float walkingSpeed = 1;
    [SerializeField] float sprintSpeed = 8;
    [SerializeField] float rotationSpeed = 10;
    [SerializeField] float fallingSpeed = 45;
    public float jumpHeight = 5f;
    public float jumpForwardMultiplier = 1f;

    [Header("Stamina Costs")]
    [SerializeField] int rollStaminaCost = 15;
    [SerializeField] int backstepStaminaCost = 12;
    [SerializeField] int sprintStaminaCost = 1;

    Vector3 normalVector;
    Vector3 targetPosition;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        player.isGrounded = true;
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    #region Movement

    public void HandleRotation()
    {
        if (player.canRotate)
        {
            if (player.playerInputManager.lockOnFlag)
            {
                if (player.playerInputManager.sprintFlag || player.playerInputManager.rollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = player.playerCamera.cameraTransform.forward * player.playerInputManager.verticalInput;
                    targetDirection += player.playerCamera.cameraTransform.right * player.playerInputManager.horizontalInput;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = player.playerCamera.currentLockOnTarget.transform.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                float moveOverride = player.playerInputManager.moveAmount;

                targetDir = player.playerCamera.cameraObject.transform.forward * player.playerInputManager.verticalInput;
                targetDir += player.playerCamera.cameraObject.transform.right * player.playerInputManager.horizontalInput;

                targetDir.Normalize();
                targetDir.y = 0;

                if (targetDir == Vector3.zero)
                {
                    targetDir = player.transform.forward;
                }

                Quaternion newRotation = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                player.transform.rotation = targetRotation;
            }
        }
    }
    
    public void HandleMovement()
    {

        if (player.playerInputManager.rollFlag)
            return;

        if (player.isInteracting)
            return;

        moveDirection = player.playerCamera.cameraObject.transform.forward * player.playerInputManager.verticalInput;
        moveDirection += player.playerCamera.cameraObject.transform.right * player.playerInputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (player.playerInputManager.sprintFlag && player.playerInputManager.moveAmount > 0.5)
        {
            speed = sprintSpeed;
            player.isSprinting = true;
            moveDirection *= speed;
            player.playerStatsManager.TakeStaminaDamage(sprintStaminaCost);
        }
        else
        {
            if (player.playerInputManager.moveAmount < 0.5)
            {
                moveDirection *= walkingSpeed;
                player.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                player.isSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        if (player.playerInputManager.lockOnFlag && player.playerInputManager.sprintFlag == false)
        {
            player.playerAnimatorManager.UpdateAnimatorValues(player.playerInputManager.verticalInput, player.playerInputManager.horizontalInput, player.isSprinting);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorValues(player.playerInputManager.moveAmount, 0, player.isSprinting);
        }
    }
    
    public void HandleRollingAndSprinting()
    {
        if (player.animator.GetBool("isInteracting"))
            return;

        if (player.playerStatsManager.currentStamina <= 0)
            return;

        if (player.playerInputManager.rollFlag)
        {
            player.playerInputManager.rollFlag = false;
            moveDirection = player.playerCamera.cameraObject.transform.forward * player.playerInputManager.verticalInput;
            moveDirection += player.playerCamera.cameraObject.transform.right * player.playerInputManager.horizontalInput;

            if (player.playerInputManager.moveAmount > 0)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                player.playerAnimatorManager.EraseHandIKForWeapon();
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = rollRotation;
                player.playerStatsManager.TakeStaminaDamage(rollStaminaCost);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation("Backstep", true);
                player.playerStatsManager.TakeStaminaDamage(backstepStaminaCost);
            }
        }
    }
    
    public void HandleFalling(Vector3 moveDirection)
    {
        player.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = player.transform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, player.transform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }
        if (player.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = player.transform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, groundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            player.isGrounded = true;
            targetPosition.y = tp.y;

            if (player.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.LogWarning("Thuoc than ki cho " + inAirTimer);
                    player.playerAnimatorManager.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;

                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Empty", true);
                    inAirTimer = 0;
                }

                player.isInAir = false;
            }
        }
        else
        {
            if (player.isGrounded)
            {
                player.isGrounded = false;
            }

            if (player.isInAir == false)
            {
                if (player.isInteracting == false)
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                player.isInAir = true;
            }
        }
        if (player.isInteracting || player.playerInputManager.moveAmount > 0)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            player.transform.position = targetPosition;
        }
    }
    
    public void HandleJumping()
    {
        if (player.isInteracting)
            return;

        if (player.playerStatsManager.currentStamina <= 0)
            return;

        if (player.playerInputManager.jumpInput)
        {

            moveDirection = player.playerCamera.cameraObject.transform.forward * player.playerInputManager.verticalInput;
            moveDirection += player.playerCamera.cameraObject.transform.right * player.playerInputManager.horizontalInput;
            moveDirection.y = 0;
            moveDirection.Normalize();

            if (player.playerInputManager.moveAmount > 0)
            {

                player.playerAnimatorManager.EraseHandIKForWeapon();
                player.playerAnimatorManager.PlayTargetAnimation("Jump", true);
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = jumpRotation;

                Vector3 jumpVelocity = moveDirection * jumpForwardMultiplier;
                jumpVelocity.y = jumpHeight;

                // Apply to Rigidbody (make sure you have a reference to it)
                rigidbody.velocity = jumpVelocity;
            }
            else
            {
                // Vertical jump if no movement input
                player.playerAnimatorManager.PlayTargetAnimation("Jump", true);
                Vector3 jumpVelocity = new Vector3(0, jumpHeight, 0);
                rigidbody.velocity = jumpVelocity;
            }
        }
    }

    #endregion

}
