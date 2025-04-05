using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Animator Replacer")]
    public AnimatorOverrideController weaponController;
    public string offHandIdleAnimation = "Left_Arm_Idle_01";

    [Header("Weapon Types")]
    public WeaponType weaponType;

    [Header("Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int magicDamage;
    public int lightningDamage;
    public int darkDamage;
    public int criticalDamageMultiplier = 4;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Absorption")]
    public float physicalDamageAbsorption;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Item Actions")]
    public ItemAction holdRBAction;
    public ItemAction tapRBAction;
    public ItemAction holdLBAction;
    public ItemAction tapLBAction;
    public ItemAction holdRTAction;
    public ItemAction tapRTAction; 
    public ItemAction holdLTAction;
    public ItemAction tapLTAction;

    //[Header("Two Handed Item Actions")]
    // public ItemAction thHoldRBAction;
    // public ItemAction thTapRBAction;
    // public ItemAction thHoldLBAction;
    // public ItemAction thTapLBAction;
    // public ItemAction thHoldRTAction;
    // public ItemAction thTapRTAction; 

    // public ItemAction thHoldLTAction;
    // public ItemAction thTapLTAction;
}
