using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;
    GameObject instantiateWarmUpSpellFX;
    GameObject instantiateSpellCastFX;
    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
    {
        base.AttemptToCastSpell(playerAnimatorManager, playerStats);
        instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorManager.transform);
        playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
        Debug.Log("Attempting to cast spell...");
    }

    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
    {
        base.SuccessfullyCastSpell(playerAnimatorManager, playerStats);
        Destroy(instantiateWarmUpSpellFX);
        Destroy(instantiateSpellCastFX);
        instantiateSpellCastFX = Instantiate(spellCastFX, playerAnimatorManager.transform);
        playerStats.HealPlayer(healAmount);
        Debug.Log("Successfully cast spell...");
        Destroy(instantiateSpellCastFX, 3f);
    }
}
