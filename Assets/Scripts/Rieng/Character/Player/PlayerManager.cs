using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerStats playerStats;
    PlayerInputManager playerInputManager;
    PlayerAnimatorManager playerAnimatorManager;
    Animator anim;
    PlayerCamera playerCamera;
    PlayerLocomotion playerLocomotion;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;
    public bool isInteracting;

    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isInvulnerable;

    private void Awake()
    {
        //instance = this;
        playerCamera = FindAnyObjectByType<PlayerCamera>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        anim = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }
    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        isUsingRightHand = anim.GetBool("isUsingRightHand");
        isUsingLeftHand = anim.GetBool("isUsingLeftHand");
        isInvulnerable = anim.GetBool("isInvulnerable");
        anim.SetBool("isInAir", isInAir);
        anim.SetBool("isDead", playerStats.isDead);

        playerInputManager.TickInput(delta);
        playerAnimatorManager.canRotate = anim.GetBool("canRotate");
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleJumping();
        playerStats.RegenerateStanima();

        CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleRotation(delta);
    }

    private void LateUpdate()
    {
        playerInputManager.rollFlag = false;
        playerInputManager.rbInput = false;
        playerInputManager.rtInput = false;
        playerInputManager.ltInput = false;
        playerInputManager.upArrow = false;
        playerInputManager.downArrow = false;
        playerInputManager.leftArrow = false;
        playerInputManager.rightArrow = false;
        playerInputManager.aInput = false;
        playerInputManager.jumpInput = false;
        playerInputManager.inventoryInput = false;

        float delta = Time.deltaTime;
        if (playerCamera != null)
        {
            playerCamera.FollowTarget(delta);
            playerCamera.HandleCameraRotation(delta, playerInputManager.mouseX, playerInputManager.mouseY);
        }

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }

    }

    #region Player Interactions
    public void CheckForInteractableObject()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, playerCamera.ignoreLayers))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (playerInputManager.aInput)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if (interactableUIGameObject != null)
            {
                interactableUIGameObject.SetActive(false);
            }

            if (itemInteractableGameObject != null && playerInputManager.aInput)
            {
                itemInteractableGameObject.SetActive(false);
            }
        }
    }
    
    public void OpenChestInteraction(Transform playerStandHereWhenOpenChest)
    {
        playerLocomotion.rigidbody.velocity = Vector3.zero;
        transform.position = playerStandHereWhenOpenChest.transform.position;
        playerAnimatorManager.PlayTargetAnimation("Open Chest", true);

    }

    #endregion
}
