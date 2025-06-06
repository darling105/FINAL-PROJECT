using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Spell Cost")]
    public int focusPointCost;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager playerStats,PlayerWeaponSlotManager playerWeaponSlotManager, bool isLeftHanded)
    {
        Debug.Log("Attempting to cast spell...");
    }

    public virtual void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager playerStats, PlayerWeaponSlotManager playerWeaponSlotManager, bool isLeftHanded)
    {
        Debug.Log("Successfully cast spell...");
        playerStats.DeductFocusPoint(focusPointCost);
    }
}
