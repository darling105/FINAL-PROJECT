using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    Animator anim;
    PlayerCamera playerCamera;
    PlayerLocomotion playerLocomotion;
    public bool isInteracting;

    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    private void Awake()
    {
        //instance = this;
        playerCamera = FindAnyObjectByType<PlayerCamera>();
    }

    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");

        playerInputManager.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (playerCamera != null)
        {
            playerCamera.FollowTarget(delta);
            playerCamera.HandleCameraRotation(delta, playerInputManager.mouseX, playerInputManager.mouseY);
        }
    }

    private void LateUpdate()
    {
        playerInputManager.rollFlag = false;
        playerInputManager.sprintFlag = false;
        playerInputManager.rbInput = false;
        playerInputManager.rtInput = false;
        playerInputManager.upArrow = false;
        playerInputManager.downArrow = false;
        playerInputManager.leftArrow = false;
        playerInputManager.rightArrow = false;

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }

    }

}
