using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
public class HeavyAttackAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.playerStatsManager.currentStamina <= 0)
            return;

        player.playerAnimatorManager.EraseHandIKForWeapon();
        player.playerEffectsManager.PlayWeaponEffect(false);

        // if (player.canDoCombo)
        // {
        //     player.playerInputManager.comboFlag = true;
        //     HandleHeavyWeaponCombo(player);
        //     player.playerInputManager.comboFlag = false;
        // }
        // else
        // {
            if (player.isInteracting)
                return;
            if (player.canDoCombo)
                return;

            HandleHeavyAttack(player);
        //}

    }
    public void HandleHeavyAttack(PlayerManager player)
    {
        if (player.isUsingLeftHand)
        {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_01, true, false, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_01;
        }
        else if (player.isUsingRightHand)
        {
            if (player.playerInputManager.twoHandFlag)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_01, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_01;
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_01, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_01;
            }
        }
    }
}

