using System.Collections;
using System.Collections.Generic;
using Den.Tools.GUI;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerCamera playerCamera;
    Animator animator;
    public PlayerInputManager playerInputManager;
    public UIManager uiManager;
    public PlayerStatsManager playerStatsManager;
    public PlayerWeaponSlotManager playerWeaponSlotManager;
    public PlayerCombatManager playerCombatManager;
    public PlayerEffectsManager playerEffectsManager;
    public PlayerInventoryManager playerInventoryManager;
    public PlayerEquipmentManager playerEquipmentManager;

    public PlayerAnimatorManager playerAnimatorManager;


    public PlayerLocomotionManager playerLocomotion;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    protected override void Awake()
    {
        //instance = this;
        base.Awake();
        playerCamera = FindAnyObjectByType<PlayerCamera>();
        uiManager = FindObjectOfType<UIManager>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        animator = GetComponent<Animator>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerLocomotion = GetComponent<PlayerLocomotionManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        isInvulnerable = animator.GetBool("isInvulnerable");
        animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
        animator.SetBool("isBlocking", isBlocking);
        animator.SetBool("isInAir", isInAir);
        animator.SetBool("isDead", playerStatsManager.isDead);

        playerInputManager.TickInput(delta);
        playerAnimatorManager.canRotate = animator.GetBool("canRotate");
        playerLocomotion.HandleRollingAndSprinting();
        playerLocomotion.HandleJumping();
        playerStatsManager.RegenerateStanima();

        CheckForInteractableObject();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float delta = Time.fixedDeltaTime;
        playerLocomotion.HandleMovement();
        playerLocomotion.HandleFalling(playerLocomotion.moveDirection);
        playerLocomotion.HandleRotation();
        playerEffectsManager.HandleAllBuildUpEffects();
    }

    private void LateUpdate()
    {
        playerInputManager.upArrow = false;
        playerInputManager.downArrow = false;
        playerInputManager.leftArrow = false;
        playerInputManager.rightArrow = false;
        playerInputManager.aInput = false;
        playerInputManager.jumpInput = false;
        playerInputManager.inventoryInput = false;

        if (playerCamera != null)
        {
            playerCamera.FollowTarget();
            playerCamera.HandleCameraRotation(playerInputManager.mouseX, playerInputManager.mouseY);
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
