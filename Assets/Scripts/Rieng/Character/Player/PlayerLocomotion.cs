using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInputManager playerInputManager;
    Transform cameraObject;
    public Vector3 moveDirection;
    [HideInInspector] public float moveAmount;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;

    [Header("Ground And Air Detection Settings")]
    [SerializeField] float groundDetectionRayStartPoint = 0.5f;
    [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] float groundDirectionRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;
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
    [SerializeField] float jumpForce = 10;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        playerAnimatorManager.Initialize();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;
    private void HandleRotation(float delta)
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
    public void HandleMovement(float delta)
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
        playerAnimatorManager.UpdateAnimatorValues(playerInputManager.moveAmount, 0, playerManager.isSprinting);

        if (playerAnimatorManager.canRotate)
        {
            HandleRotation(delta);
        }
    }
    public void HandleRollingAndSprinting(float delta)
    {
        if (playerAnimatorManager.anim.GetBool("isInteracting"))
            return;

        if (playerInputManager.rollFlag)
        {
            moveDirection = cameraObject.forward * playerInputManager.verticalInput;
            moveDirection += cameraObject.right * playerInputManager.horizontalInput;

            if (playerInputManager.moveAmount > 0)
            {
                playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Backstep", true);
            }
        }
    }
    public void HandleFalling(float delta, Vector3 moveDirection)
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
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
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

        if (playerInputManager.jumpInput)
        {
            if (playerInputManager.moveAmount > 0)
            {
                moveDirection = cameraObject.forward * playerInputManager.verticalInput;
                moveDirection += cameraObject.right * playerInputManager.horizontalInput;

                playerAnimatorManager.PlayTargetAnimation("Jump", true);
                moveDirection.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRotation;
            }
        }
    }

    #endregion

}
