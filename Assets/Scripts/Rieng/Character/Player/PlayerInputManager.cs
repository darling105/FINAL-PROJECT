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
    public bool holdRBInput;
    public bool holdLBInput;
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
            playerControls.PlayerActions.HoldLB.performed += i => holdLBInput = true;
            //playerControls.PlayerActions.HoldLB.canceled += i => holdLBInput = false;
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
            playerControls.PlayerActions.CriticalAttack.performed += i => holdRBInput = true;
            playerControls.PlayerActions.CriticalAttack.canceled += i => holdRBInput = false;
        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void TickInput(float delta)
    {
        if (playerStatsManager.isDead)
            return;

        HandlePlayerMovementInput();
        HandleRollInput();

        HandleHoldRBInput();
        HandleHoldLBInput();

        HandleTapLBInput();
        HandleTapRBInput();
        HandleTapRTInput();
        HandleTapLTInput();

        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleUseConsumableInput();
    }
    private void HandlePlayerMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }
    private void HandleRollInput()
    {

        if (bInput)
        {
            rollInputTimer += Time.deltaTime;

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

    private void HandleTapRBInput()
    {
        if (rbInput)
        {
            rbInput = false;

            if (playerInventoryManager.rightWeapon.tapRBAction != null)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                playerInventoryManager.rightWeapon.tapRBAction.PerformAction(playerManager);
            }

        }
    }

    private void HandleHoldRBInput()
    {
        if (holdRBInput)
        {
            if (playerInventoryManager.rightWeapon.holdRBAction != null)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                playerInventoryManager.rightWeapon.holdRBAction.PerformAction(playerManager);
            }
        }
    }

    private void HandleTapRTInput()
    {
        if (rtInput)
        {
            rtInput = false;

            if (playerInventoryManager.rightWeapon.tapRTAction != null)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                playerInventoryManager.rightWeapon.tapRTAction.PerformAction(playerManager);
            }
        }
    }

    private void HandleTapLBInput()
    {
        if (lbInput)
        {
            lbInput = false;

            if (playerManager.isTwoHandingWeapon)
            {
                if (playerInventoryManager.rightWeapon.tapLBAction != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.tapLBAction.PerformAction(playerManager);
                }
                else
                {
                    if (playerInventoryManager.leftWeapon.tapLBAction != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.tapLBAction.PerformAction(playerManager);
                    }
                }
            }
        }
    }

    private void HandleTapLTInput()
    {
        if (ltInput)
        {
            ltInput = false;
            if (playerInventoryManager.leftWeapon.tapLTAction != null)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(false);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                playerInventoryManager.leftWeapon.tapLTAction.PerformAction(playerManager);
            }
        }
    }

    private void HandleHoldLBInput()
    {
        if (holdLBInput)
        {
            holdLBInput = false;
            if (playerManager.isTwoHandingWeapon)
            {
                if (playerInventoryManager.rightWeapon.holdLBAction != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.holdLBAction.PerformAction(playerManager);
                }
            }
            else
            {
                if (playerInventoryManager.leftWeapon.holdLBAction != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(false);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                    playerInventoryManager.leftWeapon.holdLBAction.PerformAction(playerManager);
                }
            }

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

    private void HandleUseConsumableInput()
    {
        if (consumableInput)
        {
            consumableInput = false;
            playerInventoryManager.currentConsumable.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        }
    }

}


