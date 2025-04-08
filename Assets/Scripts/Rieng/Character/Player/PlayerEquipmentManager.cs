using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    PlayerManager player;

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
        player = GetComponent<PlayerManager>();

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

    public void EquipAllEquipmentModelsOnStart()
    {
        //HELMET EQUIPMENT
        helmetModelChanger.UnEquipAllHelmetModels();

        if (player.playerInventoryManager.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
            player.playerStatsManager.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + player.playerStatsManager.physicalDamageAbsorptionHead + "%");
        }
        else
        {
            nakedHeadModel.SetActive(true);
            player.playerStatsManager.physicalDamageAbsorptionHead = 0;
        }

        //TORSO EQUIPMENT
        torsoModelChanger.UnEquipAllTorsoModels();
        upperLeftArmModelChanger.UnEquipAllModels();
        upperRightArmModelChanger.UnEquipAllModels();


        if (player.playerInventoryManager.currentBodyEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.currentBodyEquipment.torsoModelName);
            upperLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.upperLeftArmModelName);
            upperRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.upperRightArmModelName);
            player.playerStatsManager.physicalDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + player.playerStatsManager.physicalDamageAbsorptionBody + "%");
        }
        else
        {
            torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
            upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArmModel);
            upperRightArmModelChanger.EquipModelByName(nakedUpperRightArmModel);
            player.playerStatsManager.physicalDamageAbsorptionBody = 0;
        }

        //LEG EQUIPMENT
        hipModelChanger.UnEquipAllHipModels();
        leftLegModelChanger.UnEquipAllLeftLegModels();
        rightLegModelChanger.UnEquipAllRightLegModels();

        if (player.playerInventoryManager.currentLegEquipment != null)
        {
            hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
            leftLegModelChanger.EquipLeftLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegModelName);
            rightLegModelChanger.EquipRightLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegModelName);
            player.playerStatsManager.physicalDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + player.playerStatsManager.physicalDamageAbsorptionLegs + "%");
        }
        else
        {
            hipModelChanger.EquipHipModelByName(nakedHipModel);
            leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLegModel);
            rightLegModelChanger.EquipRightLegModelByName(nakedRightLegModel);
            player.playerStatsManager.physicalDamageAbsorptionLegs = 0;
        }

        //HAND EQUIPMENT
        lowerLeftArmModelChanger.UnEquipAllModels();
        lowerRightArmModelChanger.UnEquipAllModels();
        leftHandModelChanger.UnEquipAllModels();
        rightHandModelChanger.UnEquipAllModels();

        if (player.playerInventoryManager.currentHandEquipment != null)
        {
            lowerLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
            lowerRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
            leftHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
            rightHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.rightHandModelName);
            player.playerStatsManager.physicalDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
            Debug.Log("Body Absorption: " + player.playerStatsManager.physicalDamageAbsorptionHands + "%");
        }
        else
        {
            lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArmModel);
            lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArmModel);
            leftHandModelChanger.EquipModelByName(nakedLefHandModel);
            rightHandModelChanger.EquipModelByName(nakedRightHandModel);
            player.playerStatsManager.physicalDamageAbsorptionHands = 0;
        }
    }

    public void OpenBlockingCollider()
    {
        if (blockingCollider == null)
        {
            Debug.LogError("BlockingCollider is NULL in PlayerEquipmentManager! Make sure it's assigned.");
            return;
        }

        if (player.playerInputManager.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(player.playerInventoryManager.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(player.playerInventoryManager.leftWeapon);
        }

        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
