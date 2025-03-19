using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Flask")]
public class FlaskItem : ConsumableItem
{
    PlayerManager playerManager;
    [Header("Flask Type")]
    public bool cocaFlask;
    public bool pepsiFlask;

    [Header("Recovery Amount")]
    public int healthRecoveryAmount;
    public int focusPointRecoveryAmount;

    [Header("Recovery FX")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        
        GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticleFX = recoveryFX;
        playerEffectsManager.amountToBeHealed = healthRecoveryAmount;
        playerEffectsManager.instantiatedFXModel = flask;
        weaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
