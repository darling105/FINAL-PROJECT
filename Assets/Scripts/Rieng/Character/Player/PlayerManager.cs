using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;
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
        interactableUI = FindObjectOfType<InteractableUI>();
    }
    void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        anim.SetBool("isInAir", isInAir);

        playerInputManager.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleJumping();

        CheckForInteractableObject();
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
        playerInputManager.aInput = false;
        playerInputManager.jumpInput = false;
        playerInputManager.inventoryInput = false;

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }

    }

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

}
