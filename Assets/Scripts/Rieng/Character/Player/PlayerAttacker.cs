using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{

    PlayerAnimatorManager playerAnimatorManager;
    PlayerInputManager playerInputManager;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    private void Awake()
    {
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerInputManager = GetComponent<PlayerInputManager>();
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
            else if (lastAttack == weapon.TH_Light_Attack_1)
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
            lastAttack = weapon.OH_Heavy_Attack_1;
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
}
