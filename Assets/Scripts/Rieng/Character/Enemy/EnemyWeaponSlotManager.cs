using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;
    EnemyStatsManager enemyStatsManager;

    private void Awake()
    {
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        LoadWeaponHolderSlots();
    }

    private void Start()
    {
        LoadWeaponOnBothHands();
    }

    private void LoadWeaponHolderSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(true);
        }
        else
        {
            rightHandSlot.currentWeapon = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(false);
        }
    }

    public void LoadWeaponOnBothHands()
    {
        if (rightHandWeapon != null)
        {
            LoadWeaponOnSlot(rightHandWeapon, false);
        }

        if (leftHandWeapon != null)
        {
            LoadWeaponOnSlot(leftHandWeapon, true);
        }
    }

    public void LoadWeaponsDamageCollider(bool isLeft)
    {
        if (isLeft)
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            leftHandDamageCollider.physicalDamage = leftHandWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = leftHandWeapon.fireDamage;
            
            leftHandDamageCollider.teamIDNumber = enemyStatsManager.teamIDNumber;
        }
        else
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            rightHandDamageCollider.physicalDamage = rightHandWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = rightHandWeapon.fireDamage;

            rightHandDamageCollider.teamIDNumber = enemyStatsManager.teamIDNumber;
        }
    }

    public void OpenDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void DrainStaminaLightAttack()
    {

    }
    public void DrainStaminaHeavyAttack()
    {

    }

    public void EnableCombo()
    {
        //anim.SetBool("canDoCombo", true);
    }
    public void DisableCombo()
    {
        //anim.SetBool("canDoCombo", false);
    }


    public void GrantWeaponAttackingPoiseBonus()
    {
        enemyStatsManager.totalPoiseDefence = enemyStatsManager.totalPoiseDefence + enemyStatsManager.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        enemyStatsManager.totalPoiseDefence = enemyStatsManager.armorPoiseBonus;
    }

}
