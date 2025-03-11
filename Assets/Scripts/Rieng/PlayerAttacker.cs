using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{

    PlayerAnimatorManager playerAnimatorManager;
    PlayerInputManager playerInputManager;
    public string lastAttack;

    private void Awake()
    {
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerInputManager.comboFlag)
        {
            playerAnimatorManager.anim.SetBool("canDoCombo", false);
            if (lastAttack == weapon.OH_Heavy_Attack_1)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
            }
        }


    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        lastAttack = weapon.OH_Heavy_Attack_1;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        lastAttack = weapon.OH_Heavy_Attack_1;

    }
}
