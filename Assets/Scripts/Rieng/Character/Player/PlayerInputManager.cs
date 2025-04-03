using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerInputManager : MonoBehaviour
{
    //public static PlayerInputManager instance;
    // public PlayerManager player;
    PlayerControls playerControls;
    PlayerCombatManager playerCombatManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerManager playerManager;
    PlayerEffectsManager playerEffectsManager;
    PlayerStatsManager playerStatsManager;
    BlockingCollider blockingCollider;
    PlayerWeaponSlotManager weaponSlotManager;
    PlayerCamera playerCamera;
    PlayerAnimatorManager playerAnimatorManager;
    UIManager uiManager;

    [Header("Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;


    [Header("Camera Input")]
    [SerializeField] Vector2 cameraInput;
    public float mouseY;
    public float mouseX;

    [Header("Actions Input")]
    public bool bInput;
    public bool aInput;
    public bool consumableInput;
    public bool twoHandInput;
    public bool rbInput;
    public bool rtInput;
    public bool lbInput;
    public bool ltInput;
    public bool criticalAttackInput;
    public bool jumpInput;
    public bool inventoryInput;
    public bool lockOnInput;
    public bool lockRight;
    public bool lockLeft;

    public bool upArrow;
    public bool downArrow;
    public bool leftArrow;
    public bool rightArrow;

    [Header("Flags Input")]
    public bool rollFlag;
    public bool twoHandFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    public bool inventoryFlag;
    public float rollInputTimer;

    public Transform criticalAttackRayCastStartPoint;


    private void Awake()
    {
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
        uiManager = FindObjectOfType<UIManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += playerControls => movementInput = playerControls.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.RB.performed += i => rbInput = true;
            playerControls.PlayerActions.RT.performed += i => rtInput = true;
            playerControls.PlayerActions.LB.performed += i => lbInput = true;
            playerControls.PlayerActions.LB.canceled += i => lbInput = false;
            playerControls.PlayerActions.LT.performed += i => ltInput = true;
            playerControls.PlayerInventory.Right.performed += i => rightArrow = true;
            playerControls.PlayerInventory.Left.performed += i => leftArrow = true;
            playerControls.PlayerActions.Interactable.performed += i => aInput = true;
            playerControls.PlayerActions.Roll.performed += i => bInput = true;
            playerControls.PlayerActions.Roll.canceled += i => bInput = false;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Inventory.performed += i => inventoryInput = true;
            playerControls.PlayerActions.Consumable.performed += i => consumableInput = true;
            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
            playerControls.PlayerMovement.LockOnTargetRight.performed += i => lockRight = true;
            playerControls.PlayerMovement.LockOnTargetLeft.performed += i => lockLeft = true;
            playerControls.PlayerActions.Y.performed += i => twoHandInput = true;
            playerControls.PlayerActions.CriticalAttack.performed += i => criticalAttackInput = true;
        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void TickInput(float delta)
    {
        HandlePlayerMovementInput(delta);
        HandleRollInput(delta);
        HandleCombatInput(delta);
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
        HandleUseConsumableInput();
    }
    private void HandlePlayerMovementInput(float delta)
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
        mouseY = cameraInput.y;
        mouseX = cameraInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    }
    private void HandleRollInput(float delta)
    {

        if (bInput)
        {
            rollInputTimer += delta;

            if (playerStatsManager.currentStamina <= 0)
            {
                bInput = false;
                rollFlag = false;
            }

            if (moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
            {
                sprintFlag = true;
            }
        }
        else
        {
            sprintFlag = false;
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                rollFlag = true;
            }

            rollInputTimer = 0;

        }
    }
    private void HandleCombatInput(float delta)
    {
        if (rbInput)
        {
            playerCombatManager.HandleRBAction();
        }
        if (rtInput)
        {
            playerCombatManager.HandleHeavyAttack(playerInventoryManager.rightWeapon);
        }

        if (ltInput)
        {
            if (twoHandFlag)
            {

            }
            else
            {
                playerCombatManager.HandleLTAction();
            }
        }

        if (lbInput)
        {
            playerCombatManager.HandleLBAction();
        }
        else
        {
            playerManager.isBlocking = false;

            if (blockingCollider.blockingCollider.enabled)
            {
                blockingCollider.DisableBlockingCollider();
            }
        }


    }
    private void HandleQuickSlotsInput()
    {
        if (rightArrow)
        {
            playerInventoryManager.ChangeRightWeapon();
        }
        else if (leftArrow)
        {
            playerInventoryManager.ChangeLeftWeapon();
        }
    }
    private void HandleInventoryInput()
    {
        if (inventoryInput)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                uiManager.OpenSelectWindow();
                uiManager.UpdateUI();
                uiManager.hudWindow.SetActive(false);
            }
            else
            {
                uiManager.CloseSelectWindow();
                uiManager.CloseAllInventoryWindows();
                uiManager.hudWindow.SetActive(true);
            }
        }
    }
    private void HandleLockOnInput()
    {
        if (lockOnInput && lockOnFlag == false)
        {
            lockOnInput = false;
            playerCamera.HandleLockOn();
            if (playerCamera.nearestLockOnTarget != null)
            {
                playerCamera.currentLockOnTarget = playerCamera.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if (lockOnInput && lockOnFlag)
        {
            lockOnInput = false;
            lockOnFlag = false;
            playerCamera.ClearLockOnTarget();
        }

        if (lockOnFlag && lockLeft)
        {
            lockLeft = false;
            playerCamera.HandleLockOn();
            if (playerCamera.leftLockTarget != null)
            {
                playerCamera.currentLockOnTarget = playerCamera.leftLockTarget;
            }
        }

        if (lockOnFlag && lockRight)
        {
            lockRight = false;
            playerCamera.HandleLockOn();
            if (playerCamera.rightLockTarget != null)
            {
                playerCamera.currentLockOnTarget = playerCamera.rightLockTarget;
            }
        }

        playerCamera.SetCameraHeight();
    }
    private void HandleTwoHandInput()
    {
        if (twoHandInput)
        {
            twoHandInput = false;
            twoHandFlag = !twoHandFlag;
            if (twoHandFlag)
            {
                playerManager.isTwoHandingWeapon = true;
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                
            }
            else
            {
                playerManager.isTwoHandingWeapon = false;
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
                weaponSlotManager.LoadTwoHandIKTargets(false);
            }
        }
    }
    private void HandleCriticalAttackInput()
    {
        if (criticalAttackInput)
        {
            criticalAttackInput = false;
            playerCombatManager.AttempBackStabOrRiposte();
        }
    }

    private void HandleUseConsumableInput()
    {
        if (consumableInput)
        {
            consumableInput = false;
            playerInventoryManager.currentConsumable.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        }
    }

}


