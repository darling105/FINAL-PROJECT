using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    Transform cameraObject;
    private Vector3 moveDirection;
    [HideInInspector] public float moveAmount;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;

    [Header("Movement Settings")]
    [HideInInspector] public Transform myTransform;
    public new Rigidbody rigidbody;
    public GameObject normalCamera;
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float rotationSpeed = 10;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        playerAnimatorManager.Initialize();
    }
    private void Update()
    {
        float delta = Time.deltaTime;
        playerInputManager.TickInput(delta);

        moveDirection = cameraObject.forward * playerInputManager.verticalInput;
        moveDirection += cameraObject.right * playerInputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        playerAnimatorManager.UpdateAnimatorValues(playerInputManager.moveAmount, 0);

        if (playerAnimatorManager.canRotate)
        {
            HandleRotation(delta);
        }
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

    #endregion

}
