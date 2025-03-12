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
    public bool rbInput;
    public bool rtInput;
    public bool jumpInput;
    public bool inventoryInput;
    public bool upArrow;
    public bool downArrow;
    public bool leftArrow;
    public bool rightArrow;

    [Header("Flags Input")]
    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public float rollInputTimer;


    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += playerControls => movementInput = playerControls.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
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
        HandleInteractingButtonInput();
        HandleJumpInput();
        HandleInventoryInput();
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

        if (bInput)
        {
            rollInputTimer += delta;
            sprintFlag = true;
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
        playerControls.PlayerActions.RB.performed += i => rbInput = true;
        playerControls.PlayerActions.RT.performed += i => rtInput = true;

        if (rbInput)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }
        if (rtInput)
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.canDoCombo)
                return;
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }
    private void HandleQuickSlotsInput()
    {
        playerControls.PlayerInventory.Right.performed += i => rightArrow = true;
        playerControls.PlayerInventory.Left.performed += i => leftArrow = true;

        if (rightArrow)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if (leftArrow)
        {
            playerInventory.ChangeLeftWeapon();
        }
    }
    private void HandleInteractingButtonInput()
    {
        playerControls.PlayerActions.Interactable.performed += i => aInput = true;
    }
    private void HandleJumpInput()
    {
        playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
    }
    private void HandleInventoryInput()
    {
        playerControls.PlayerActions.Inventory.performed += i => inventoryInput = true;

        if (inventoryInput)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                uiManager.OpenSelectWindow();
            }
            else
            {
                uiManager.CloseSelectWindow();
            }
        }
    }

}


