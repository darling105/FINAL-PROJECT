using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{

    PlayerAnimatorManager playerAnimatorManager;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;
    PlayerInputManager playerInputManager;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    private void Awake()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerInputManager = GetComponentInParent<PlayerInputManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerInputManager.comboFlag)
        {
            playerAnimatorManager.anim.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
            }

            if (lastAttack == weapon.TH_Light_Attack_1)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                lastAttack = weapon.TH_Light_Attack_2;
            }
            else if (lastAttack == weapon.TH_Light_Attack_2)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_3, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (playerInputManager.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
            lastAttack = weapon.TH_Light_Attack_1;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            lastAttack = weapon.OH_Light_Attack_1;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (playerInputManager.twoHandFlag)
        {

        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }


    }

    #region Input Actions

    public void HandleRBAction()
    {
        if (playerInventory.rightWeapon.isMeleeWeapon)
        {
            PerformMeleeAction();
        }
        else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
        {
            PerformRBSpellAction(playerInventory.rightWeapon);
        }


    }

    #endregion

    #region Attack Actions

    private void PerformMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            playerInputManager.comboFlag = true;
            HandleWeaponCombo(playerInventory.rightWeapon);
            playerInputManager.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;
            if (playerManager.canDoCombo)
                return;

            playerAnimatorManager.anim.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventory.rightWeapon);
        }
    }

    private void PerformRBSpellAction(WeaponItem weapon)
    {
        if (weapon.isFaithCaster)
        {
            if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
            {
                playerInventory.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStats);
            }
        }
    }

    private void SuccessfullyCastSpell()
    {   
            playerInventory.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStats);
    }

    #endregion
}
