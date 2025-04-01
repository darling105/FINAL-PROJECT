using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerStatsManager playerStatsManager;

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
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();

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

        if (playerInventoryManager.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
            playerStatsManager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + playerStatsManager.physicalDamageAbsorptionHead + "%");
        }
        else
        {
            nakedHeadModel.SetActive(true);
            playerStatsManager.physicalDamageAbsorptionHead = 0;
        }

        //TORSO EQUIPMENT
        torsoModelChanger.UnEquipAllTorsoModels();
        upperLeftArmModelChanger.UnEquipAllModels();
        upperRightArmModelChanger.UnEquipAllModels();


        if (playerInventoryManager.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
            upperLeftArmModelChanger.EquipModelByName(playerInventoryManager.currentTorsoEquipment.upperLeftArmModelName);
            upperRightArmModelChanger.EquipModelByName(playerInventoryManager.currentTorsoEquipment.upperRightArmModelName);
            playerStatsManager.physicalDamageAbsorptionBody = playerInventoryManager.currentTorsoEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + playerStatsManager.physicalDamageAbsorptionBody + "%");
        }
        else
        {
            torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
            upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArmModel);
            upperRightArmModelChanger.EquipModelByName(nakedUpperRightArmModel);
            playerStatsManager.physicalDamageAbsorptionBody = 0;
        }

        //LEG EQUIPMENT
        hipModelChanger.UnEquipAllHipModels();
        leftLegModelChanger.UnEquipAllLeftLegModels();
        rightLegModelChanger.UnEquipAllRightLegModels();

        if (playerInventoryManager.currentLegEquipment != null)
        {
            hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
            leftLegModelChanger.EquipLeftLegModelByName(playerInventoryManager.currentLegEquipment.leftLegModelName);
            rightLegModelChanger.EquipRightLegModelByName(playerInventoryManager.currentLegEquipment.rightLegModelName);
            playerStatsManager.physicalDamageAbsorptionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + playerStatsManager.physicalDamageAbsorptionLegs + "%");
        }
        else
        {
            hipModelChanger.EquipHipModelByName(nakedHipModel);
            leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLegModel);
            rightLegModelChanger.EquipRightLegModelByName(nakedRightLegModel);
            playerStatsManager.physicalDamageAbsorptionLegs = 0;
        }

        //HAND EQUIPMENT
        lowerLeftArmModelChanger.UnEquipAllModels();
        lowerRightArmModelChanger.UnEquipAllModels();
        leftHandModelChanger.UnEquipAllModels();
        rightHandModelChanger.UnEquipAllModels();

        if (playerInventoryManager.currentHandEquipment != null)
        {
            lowerLeftArmModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
            lowerRightArmModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
            leftHandModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.leftHandModelName);
            rightHandModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.rightHandModelName);
            playerStatsManager.physicalDamageAbsorptionHands = playerInventoryManager.currentHandEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + playerStatsManager.physicalDamageAbsorptionHands + "%");
        }
        else
        {
            lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArmModel);
            lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArmModel);
            leftHandModelChanger.EquipModelByName(nakedLefHandModel);
            rightHandModelChanger.EquipModelByName(nakedRightHandModel);
            playerStatsManager.physicalDamageAbsorptionHands = 0;
        }
    }

    public void OpenBlockingCollider()
    {
        if (playerInputManager.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);
        }

        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
