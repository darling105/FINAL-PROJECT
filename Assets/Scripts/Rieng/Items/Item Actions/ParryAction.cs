using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Parry Action")]
public class ParryAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.isInteracting)
            return;

        player.playerAnimatorManager.EraseHandIKForWeapon();
        WeaponItem parryingWeapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;

        if (parryingWeapon.weaponType == WeaponType.SmallShield)
        {
            player.playerAnimatorManager.PlayTargetAnimation("Parry", true);
            Debug.Log("Parry with small shield");
        }
        else if (parryingWeapon.weaponType == WeaponType.Shield)
        {
            player.playerAnimatorManager.PlayTargetAnimation("Parry", true);
            Debug.Log("Parry with shield");
        }
        
    }
}
