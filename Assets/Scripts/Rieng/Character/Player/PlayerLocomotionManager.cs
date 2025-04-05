using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    PlayerCamera playerCamera;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    PlayerInputManager playerInputManager;
    Transform cameraObject;
    public Vector3 moveDirection;
    [HideInInspector] public float moveAmount;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;

    [Header("Ground And Air Detection Settings")]
    [SerializeField] float groundDetectionRayStartPoint = 0.5f;
    [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] float groundDirectionRayDistance = 0.2f;
    public LayerMask groundCheck;
    public float inAirTimer;

    [Header("Movement Settings")]
    [HideInInspector] public Transform myTransform;
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

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    private void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        rigidbody = GetComponent<Rigidbody>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    void Start()
    {
        cameraObject = Camera.main.transform;
        myTransform = transform;
        playerManager.isGrounded = true;
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;
    public void HandleRotation()
    {
        if (playerAnimatorManager.canRotate)
        {
            if (playerInputManager.lockOnFlag)
            {
                if (playerInputManager.sprintFlag || playerInputManager.rollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = playerCamera.cameraTransform.forward * playerInputManager.verticalInput;
                    targetDirection += playerCamera.cameraTransform.right * playerInputManager.horizontalInput;
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
                    rotationDirection = playerCamera.currentLockOnTarget.transform.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                Vector3 targetRotationDirection = Vector3.zero;
                float moveOverride = playerInputManager.moveAmount;

                targetRotationDirection = cameraObject.forward * playerInputManager.verticalInput;
                targetRotationDirection += cameraObject.right * playerInputManager.horizontalInput;

                targetRotationDirection.Normalize();
                targetRotationDirection.y = 0;

                if (targetRotationDirection == Vector3.zero)
                {
                    targetRotationDirection = myTransform.forward;
                }

                Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                myTransform.rotation = targetRotation;
            }
        }
    }
    public void HandleMovement()
    {

        if (playerInputManager.rollFlag)
            return;

        if (playerManager.isInteracting)
            return;

        moveDirection = cameraObject.forward * playerInputManager.verticalInput;
        moveDirection += cameraObject.right * playerInputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (playerInputManager.sprintFlag && playerInputManager.moveAmount > 0.5)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
            playerStatsManager.TakeStaminaDamage(sprintStaminaCost);
        }
        else
        {
            if (playerInputManager.moveAmount < 0.5)
            {
                moveDirection *= walkingSpeed;
                playerManager.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        if (playerInputManager.lockOnFlag && playerInputManager.sprintFlag == false)
        {
            playerAnimatorManager.UpdateAnimatorValues(playerInputManager.verticalInput, playerInputManager.horizontalInput, playerManager.isSprinting);
        }
        else
        {
            playerAnimatorManager.UpdateAnimatorValues(playerInputManager.moveAmount, 0, playerManager.isSprinting);
        }
    }
    public void HandleRollingAndSprinting()
    {
        if (playerAnimatorManager.animator.GetBool("isInteracting"))
            return;

        if (playerStatsManager.currentStamina <= 0)
            return;

        if (playerInputManager.rollFlag)
        {
            playerInputManager.rollFlag = false;
            moveDirection = cameraObject.forward * playerInputManager.verticalInput;
            moveDirection += cameraObject.right * playerInputManager.horizontalInput;

            if (playerInputManager.moveAmount > 0)
            {
                playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                playerAnimatorManager.EraseHandIKForWeapon();
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
                playerStatsManager.TakeStaminaDamage(rollStaminaCost);
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Backstep", true);
                playerStatsManager.TakeStaminaDamage(backstepStaminaCost);
            }
        }
    }
    public void HandleFalling(Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }
        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, groundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.LogWarning("Thuoc than ki cho " + inAirTimer);
                    playerAnimatorManager.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;

                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Empty", true);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (playerManager.isInAir == false)
            {
                if (playerManager.isInteracting == false)
                {
                    playerAnimatorManager.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }
        if (playerManager.isInteracting || playerInputManager.moveAmount > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            myTransform.position = targetPosition;
        }
    }
    public void HandleJumping()
    {
        if (playerManager.isInteracting)
            return;

        if (playerStatsManager.currentStamina <= 0)
            return;

        if (playerInputManager.jumpInput)
        {

            moveDirection = cameraObject.forward * playerInputManager.verticalInput;
            moveDirection += cameraObject.right * playerInputManager.horizontalInput;
            moveDirection.y = 0;
            moveDirection.Normalize();

            if (playerInputManager.moveAmount > 0)
            {

                playerAnimatorManager.EraseHandIKForWeapon();
                playerAnimatorManager.PlayTargetAnimation("Jump", true);
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRotation;

                Vector3 jumpVelocity = moveDirection * jumpForwardMultiplier;
                jumpVelocity.y = jumpHeight;

                // Apply to Rigidbody (make sure you have a reference to it)
                rigidbody.velocity = jumpVelocity;
            }
             else
        {
            // Vertical jump if no movement input
            playerAnimatorManager.PlayTargetAnimation("Jump", true);
            Vector3 jumpVelocity = new Vector3(0, jumpHeight, 0);
            rigidbody.velocity = jumpVelocity;
        }
        }
    }

    #endregion

}
