using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerInputManager : MonoBehaviour
{
    //public static PlayerInputManager instance;
    // public PlayerManager player;
    PlayerControls playerControls;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    WeaponSlotManager weaponSlotManager;
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
    public bool twoHandInput;
    public bool rbInput;
    public bool rtInput;
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
        playerAttacker = GetComponentInChildren<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        uiManager = FindObjectOfType<UIManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
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
            playerControls.PlayerInventory.Right.performed += i => rightArrow = true;
            playerControls.PlayerInventory.Left.performed += i => leftArrow = true;
            playerControls.PlayerActions.Interactable.performed += i => aInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Inventory.performed += i => inventoryInput = true;
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
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
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
        bInput = playerControls.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        sprintFlag = bInput;
        if (bInput)
        {
            rollInputTimer += delta;

        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = true;
                rollFlag = true;
            }

            rollInputTimer = 0;

        }
    }
    private void HandleAttackInput(float delta)
    {
        if (rbInput)
        {
            playerAttacker.HandleRBAction();
        }
        if (rtInput)
        {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }
    private void HandleQuickSlotsInput()
    {
        if (rightArrow)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if (leftArrow)
        {
            playerInventory.ChangeLeftWeapon();
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
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            }
            else
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if (criticalAttackInput)
        {
            criticalAttackInput = false;
            playerAttacker.AttempBackStabOrRiposte();
        }
    }

}


