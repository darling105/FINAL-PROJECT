using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    QuickSlotsUI quickSlotsUI;
    PlayerManager playerManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerStatsManager playerStatsManager;
    PlayerInputManager playerInputManager;
    PlayerEffectsManager playerEffectsManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerCamera playerCamera;

    protected override void Awake()
    {
        base.Awake();
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerManager = GetComponent<PlayerManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
    }

    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if (playerInputManager.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                }
                else
                {
                    backSlot.UnloadWeaponAndDestroy();
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
        else
        {
            weaponItem = unarmedWeapon;
            if (isLeft)
            {
                playerInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = unarmedWeapon;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                playerInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = unarmedWeapon;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }

    }

    public void SuccessfullyThrowFireBomb()
    {
        Destroy(playerEffectsManager.instantiatedFXModel);
        BombConsumableItem fireBombItem = playerInventoryManager.currentConsumable as BombConsumableItem;

        GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, playerCamera.cameraPivotTransform.rotation);
        activeModelBomb.transform.rotation = Quaternion.Euler(playerCamera.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
        BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

        damageCollider.explosionDamage = fireBombItem.baseDamage;
        damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
        damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
        damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
        damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
        //tinhs dame vaof nguowif minhf 
    }

}
