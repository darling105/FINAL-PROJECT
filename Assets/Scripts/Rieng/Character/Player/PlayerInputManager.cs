using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerInputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerManager player;

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
        player = GetComponent<PlayerManager>();
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
        if (player.isDead)
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

            if (player.playerStatsManager.currentStamina <= 0)
            {
                bInput = false;
                rollFlag = false;
            }

            if (moveAmount > 0.5f && player.playerStatsManager.currentStamina > 0)
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

            if (player.playerInventoryManager.rightWeapon.tapRBAction != null)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                player.playerInventoryManager.rightWeapon.tapRBAction.PerformAction(player);
            }

        }
    }

    private void HandleHoldRBInput()
    {
        if (holdRBInput)
        {
            if (player.playerInventoryManager.rightWeapon.holdRBAction != null)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                player.playerInventoryManager.rightWeapon.holdRBAction.PerformAction(player);
            }
        }
    }

    private void HandleTapRTInput()
    {
        if (rtInput)
        {
            rtInput = false;

            if (player.playerInventoryManager.rightWeapon.tapRTAction != null)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                player.playerInventoryManager.rightWeapon.tapRTAction.PerformAction(player);
            }
        }
    }

    private void HandleTapLBInput()
    {
        if (lbInput)
        {
            lbInput = false;

            if (player.isTwoHandingWeapon)
            {
                if (player.playerInventoryManager.rightWeapon.tapLBAction != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tapLBAction.PerformAction(player);
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.tapLBAction != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.tapLBAction.PerformAction(player);
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
            if (player.playerInventoryManager.leftWeapon.tapLTAction != null)
            {
                player.UpdateWhichHandCharacterIsUsing(false);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                player.playerInventoryManager.leftWeapon.tapLTAction.PerformAction(player);
            }
        }
    }

    private void HandleHoldLBInput()
    {
        if (holdLBInput)
        {
            holdLBInput = false;
            if (player.isTwoHandingWeapon)
            {
                if (player.playerInventoryManager.rightWeapon.holdLBAction != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.holdLBAction.PerformAction(player);
                }
            }
            else
            {
                if (player.playerInventoryManager.leftWeapon.holdLBAction != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(false);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                    player.playerInventoryManager.leftWeapon.holdLBAction.PerformAction(player);
                }
            }

        }
        else
        {
            player.isBlocking = false;

            if (player.blockingCollider.blockingCollider.enabled)
            {
                player.blockingCollider.DisableBlockingCollider();
            }
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (rightArrow)
        {
            player.playerInventoryManager.ChangeRightWeapon();
        }
        else if (leftArrow)
        {
            player.playerInventoryManager.ChangeLeftWeapon();
        }
    }

    private void HandleInventoryInput()
    {
        if (inventoryFlag)
        {
            player.uiManager.UpdateUI();
        }
        if (inventoryInput)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                player.uiManager.OpenSelectWindow();
                player.uiManager.hudWindow.SetActive(false);
            }
            else
            {
                player.uiManager.CloseSelectWindow();
                player.uiManager.CloseAllInventoryWindows();
                player.uiManager.hudWindow.SetActive(true);
            }
        }
    }

    private void HandleLockOnInput()
    {
        if (lockOnInput && lockOnFlag == false)
        {
            lockOnInput = false;
            player.playerCamera.HandleLockOn();
            if (player.playerCamera.nearestLockOnTarget != null)
            {
                player.playerCamera.currentLockOnTarget = player.playerCamera.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if (lockOnInput && lockOnFlag)
        {
            lockOnInput = false;
            lockOnFlag = false;
            player.playerCamera.ClearLockOnTarget();
        }

        if (lockOnFlag && lockLeft)
        {
            lockLeft = false;
            player.playerCamera.HandleLockOn();
            if (player.playerCamera.leftLockTarget != null)
            {
                player.playerCamera.currentLockOnTarget = player.playerCamera.leftLockTarget;
            }
        }

        if (lockOnFlag && lockRight)
        {
            lockRight = false;
            player.playerCamera.HandleLockOn();
            if (player.playerCamera.rightLockTarget != null)
            {
                player.playerCamera.currentLockOnTarget = player.playerCamera.rightLockTarget;
            }
        }

        player.playerCamera.SetCameraHeight();
    }

    private void HandleTwoHandInput()
    {
        if (twoHandInput)
        {
            twoHandInput = false;
            twoHandFlag = !twoHandFlag;
            if (twoHandFlag)
            {
                player.isTwoHandingWeapon = true;
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);

            }
            else
            {
                player.isTwoHandingWeapon = false;
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                player.playerWeaponSlotManager.LoadTwoHandIKTargets(false);
            }
        }
    }

    private void HandleUseConsumableInput()
    {
        if (consumableInput)
        {
            consumableInput = false;
            player.playerInventoryManager.currentConsumable.AttemptToConsumeItem(player.playerAnimatorManager, player.playerWeaponSlotManager, player.playerEffectsManager);
        }
    }

}


