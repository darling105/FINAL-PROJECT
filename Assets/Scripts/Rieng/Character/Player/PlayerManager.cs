using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerStatsManager playerStatsManager;
    PlayerEffectsManager playerEffectsManager;
    PlayerInputManager playerInputManager;
    PlayerAnimatorManager playerAnimatorManager;
    Animator animator;
    PlayerCamera playerCamera;
    PlayerLocomotionManager playerLocomotion;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    private void Awake()
    {
        //instance = this;
        playerCamera = FindAnyObjectByType<PlayerCamera>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        animator = GetComponent<Animator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerLocomotion = GetComponent<PlayerLocomotionManager>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        isUsingRightHand = animator.GetBool("isUsingRightHand");
        isUsingLeftHand = animator.GetBool("isUsingLeftHand");
        isInvulnerable = animator.GetBool("isInvulnerable");
        animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
        animator.SetBool("isBlocking", isBlocking);
        animator.SetBool("isInAir", isInAir);
        animator.SetBool("isDead", playerStatsManager.isDead);

        playerInputManager.TickInput(delta);
        playerAnimatorManager.canRotate = animator.GetBool("canRotate");
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleJumping();
        playerStatsManager.RegenerateStanima();

        CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleRotation(delta);
        playerEffectsManager.HandleAllBuildUpEffects();
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

    public void PassThroughFogWallInteraction(Transform fogWallEntrance)
    {
        playerLocomotion.rigidbody.velocity = Vector3.zero;

        Vector3 rotationDirection = fogWallEntrance.transform.forward;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;


        playerAnimatorManager.PlayTargetAnimation("Pass_Through_Fog", true);
    }

    #endregion
}
