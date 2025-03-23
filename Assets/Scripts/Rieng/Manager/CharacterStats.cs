using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina;
    public float currentStamina;

    public int focusLevel = 10;
    public float maxFocusPoint;
    public float currentFocusPoint;

    public int shadeCount = 0;

    [Header("Armor Absorption")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody;
    public float physicalDamageAbsorptionHands;
    public float physicalDamageAbsorptionLegs;

    //Fire
    //Lightning
    //Magic
    //Dark

    public bool isDead;


    public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
    {
        if (isDead)
            return;

        float totalPhysicalDamageAbsorption = 1 -
        (1 - physicalDamageAbsorptionHead / 100) *
        (1 - physicalDamageAbsorptionBody / 100) *
        (1 - physicalDamageAbsorptionHands / 100) *
        (1 - physicalDamageAbsorptionLegs / 100);

        Debug.Log("Total Physical Damage Absorption: " + totalPhysicalDamageAbsorption + "%");

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

        float finalDamage = physicalDamage;

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        Debug.Log("Final Damage Dealt is: " + finalDamage);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }
}
