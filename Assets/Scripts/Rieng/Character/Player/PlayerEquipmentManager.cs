using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    PlayerInventory playerInventory;
    PlayerStats playerStats;

    [Header("Equipment Model Changer")]
    //Head Equipment
    HelmetModelChanger helmetModelChanger;

    //Torso Equipment
    TorsoModelChanger torsoModelChanger;
    UpperLeftArmModelChanger upperLeftArmModelChanger;
    UpperRightArmModelChanger upperRightArmModelChanger;

    //Leg Equipment
    HipModelChanger hipModelChanger;
    LeftLegModelChanger leftLegModelChanger;
    RightLegModelChanger rightLegModelChanger;

    //Hand Equipment
    LowerLeftArmModelChanger lowerLeftArmModelChanger;
    LowerRightArmModelChanger lowerRightArmModelChanger;
    LeftHandModelChanger leftHandModelChanger;
    RightHandModelChanger rightHandModelChanger;


    [Header("Default Naked Models")]
    public GameObject nakedHeadModel;
    public string nakedTorsoModel;
    public string nakedUpperLeftArmModel;
    public string nakedUpperRightArmModel;
    public string nakedLowerLeftArmModel;
    public string nakedLowerRightArmModel;
    public string nakedLefHandModel;
    public string nakedRightHandModel;
    public string nakedHipModel;
    public string nakedLeftLegModel;
    public string nakedRightLegModel;

    public BlockingCollider blockingCollider;

    private void Awake()
    {
        playerInputManager = GetComponentInParent<PlayerInputManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        playerStats = GetComponentInParent<PlayerStats>();

        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
        hipModelChanger = GetComponentInChildren<HipModelChanger>();
        leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
        rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
        upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
        upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
        lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
        lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
        leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
        rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();
    }

    private void EquipAllEquipmentModelsOnStart()
    {
        //HELMET EQUIPMENT
        helmetModelChanger.UnEquipAllHelmetModels();

        if (playerInventory.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            playerStats.physicalDamageAbsorptionHead = playerInventory.currentHelmetEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + playerStats.physicalDamageAbsorptionHead + "%");
        }
        else
        {
            nakedHeadModel.SetActive(true);
            playerStats.physicalDamageAbsorptionHead = 0;
        }

        //TORSO EQUIPMENT
        torsoModelChanger.UnEquipAllTorsoModels();
        upperLeftArmModelChanger.UnEquipAllModels();
        upperRightArmModelChanger.UnEquipAllModels();


        if (playerInventory.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
            upperLeftArmModelChanger.EquipModelByName(playerInventory.currentTorsoEquipment.upperLeftArmModelName);
            upperRightArmModelChanger.EquipModelByName(playerInventory.currentTorsoEquipment.upperRightArmModelName);
            playerStats.physicalDamageAbsorptionBody = playerInventory.currentTorsoEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + playerStats.physicalDamageAbsorptionBody + "%");
        }
        else
        {
            torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
            upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArmModel);
            upperRightArmModelChanger.EquipModelByName(nakedUpperRightArmModel);
            playerStats.physicalDamageAbsorptionBody = 0;
        }

        //LEG EQUIPMENT
        hipModelChanger.UnEquipAllHipModels();
        leftLegModelChanger.UnEquipAllLeftLegModels();
        rightLegModelChanger.UnEquipAllRightLegModels();

        if (playerInventory.currentLegEquipment != null)
        {
            hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
            leftLegModelChanger.EquipLeftLegModelByName(playerInventory.currentLegEquipment.leftLegModelName);
            rightLegModelChanger.EquipRightLegModelByName(playerInventory.currentLegEquipment.rightLegModelName);
            playerStats.physicalDamageAbsorptionLegs = playerInventory.currentLegEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + playerStats.physicalDamageAbsorptionLegs + "%");
        }
        else
        {
            hipModelChanger.EquipHipModelByName(nakedHipModel);
            leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLegModel);
            rightLegModelChanger.EquipRightLegModelByName(nakedRightLegModel);
            playerStats.physicalDamageAbsorptionLegs = 0;
        }

        //HAND EQUIPMENT
        lowerLeftArmModelChanger.UnEquipAllModels();
        lowerRightArmModelChanger.UnEquipAllModels();
        leftHandModelChanger.UnEquipAllModels();
        rightHandModelChanger.UnEquipAllModels();

        if (playerInventory.currentHandEquipment != null)
        {
            lowerLeftArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerLeftArmModelName);
            lowerRightArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerRightArmModelName);
            leftHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.leftHandModelName);
            rightHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.rightHandModelName);
            playerStats.physicalDamageAbsorptionHands = playerInventory.currentHandEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + playerStats.physicalDamageAbsorptionHands + "%");
        }
        else
        {
            lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArmModel);
            lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArmModel);
            leftHandModelChanger.EquipModelByName(nakedLefHandModel);
            rightHandModelChanger.EquipModelByName(nakedRightHandModel);
            playerStats.physicalDamageAbsorptionHands = 0;
        }
    }

    public void OpenBlockingCollider()
    {
        if (playerInputManager.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
        }

        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
