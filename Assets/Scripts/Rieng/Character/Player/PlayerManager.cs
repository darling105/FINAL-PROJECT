using System.Collections;
using System.Collections.Generic;
using Den.Tools.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    [Header("Camera")]
    public PlayerCamera playerCamera;

    [Header("Input")]
    public PlayerInputManager playerInputManager;

    [Header("UI")]
    public UIManager uiManager;

    [Header("Player")]
    public PlayerStatsManager playerStatsManager;
    public PlayerWeaponSlotManager playerWeaponSlotManager;
    public PlayerCombatManager playerCombatManager;
    public PlayerEffectsManager playerEffectsManager;
    public PlayerInventoryManager playerInventoryManager;
    public PlayerEquipmentManager playerEquipmentManager;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerLocomotionManager playerLocomotionManager;

    [Header("Colliders")]
    public BlockingCollider blockingCollider;

    [Header("Interactables")]
    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    protected override void Awake()
    {
        base.Awake();
        playerCamera = FindAnyObjectByType<PlayerCamera>();
        uiManager = FindObjectOfType<UIManager>();
        interactableUI = FindObjectOfType<InteractableUI>();
        playerInputManager = GetComponent<PlayerInputManager>();
        animator = GetComponent<Animator>();

        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();

        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
    }

    private void Start()
    {
        WorldSaveGameManager.instance.playerManager = this;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        canRotate = animator.GetBool("canRotate");
        isInvulnerable = animator.GetBool("isInvulnerable");
        animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
        animator.SetBool("isBlocking", isBlocking);
        animator.SetBool("isInAir", isInAir);
        animator.SetBool("isDead", isDead);

        playerInputManager.TickInput(delta);
        playerLocomotionManager.HandleRollingAndSprinting();
        playerLocomotionManager.HandleJumping();
        playerStatsManager.RegenerateStanima();

        CheckForInteractableObject();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float delta = Time.fixedDeltaTime;
        playerLocomotionManager.HandleMovement();
        playerLocomotionManager.HandleFalling(playerLocomotionManager.moveDirection);
        playerLocomotionManager.HandleRotation();
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
            playerLocomotionManager.inAirTimer = playerLocomotionManager.inAirTimer + Time.deltaTime;
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
        playerLocomotionManager.rigidbody.velocity = Vector3.zero;
        transform.position = playerStandHereWhenOpenChest.transform.position;
        playerAnimatorManager.PlayTargetAnimation("Open Chest", true);

    }

    public void PassThroughFogWallInteraction(Transform fogWallEntrance)
    {
        playerLocomotionManager.rigidbody.velocity = Vector3.zero;

        Vector3 rotationDirection = fogWallEntrance.transform.forward;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;


        playerAnimatorManager.PlayTargetAnimation("Pass_Through_Fog", true);
    }

    #endregion

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentCharacterData.characterName = characterName;
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;
    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        characterName = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;
    }

}
