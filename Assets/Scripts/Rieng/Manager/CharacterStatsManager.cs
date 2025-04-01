using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [Header("Team I.D")]
    public int teamIDNumber = 0;
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
    public int shadesAwardedOnDeath = 100;

    [Header("Poise")]
    public float totalPoiseDefence;
    public float offensivePoiseBonus;
    public float armorPoiseBonus;
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    [Header("Physical Armor Absorption")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody;
    public float physicalDamageAbsorptionHands;
    public float physicalDamageAbsorptionLegs;
    [Header("Fire Armor Absorption")]
    public float fireDamageAbsorptionHead;
    public float fireDamageAbsorptionBody;
    public float fireDamageAbsorptionHands;
    public float fireDamageAbsorptionLegs;
    [Header("Magic Armor Absorption")]
    public float magicDamageAbsorptionHead;
    public float magicDamageAbsorptionBody;
    public float magicDamageAbsorptionHands;
    public float magicDamageAbsorptionLegs;
    [Header("Lightning Armor Absorption")]
    public float lightningDamageAbsorptionHead;
    public float lightningDamageAbsorptionBody;
    public float lightningDamageAbsorptionHands;
    public float lightningDamageAbsorptionLegs;
    [Header("Dark Armor Absorption")]
    public float darkDamageAbsorptionHead;
    public float darkDamageAbsorptionBody;
    public float darkDamageAbsorptionHands;
    public float darkDamageAbsorptionLegs;

    //Fire
    //Lightning
    //Magic
    //Dark

    public bool isDead;

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    private void Start()
    {
        totalPoiseDefence = armorPoiseBonus;
    }

    public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation = "Damage_01")
    {
        if (isDead)
            return;

        //PHYSIC

        float totalPhysicalDamageAbsorption = 1 -
        (1 - physicalDamageAbsorptionHead / 100) *
        (1 - physicalDamageAbsorptionBody / 100) *
        (1 - physicalDamageAbsorptionHands / 100) *
        (1 - physicalDamageAbsorptionLegs / 100);

        Debug.Log("Total Physical Damage Absorption: " + totalPhysicalDamageAbsorption + "%");

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

        //FIRE

        float totalFireDamageAbsorption = 1 -
        (1 - fireDamageAbsorptionHead / 100) *
        (1 - fireDamageAbsorptionBody / 100) *
        (1 - fireDamageAbsorptionHands / 100) *
        (1 - fireDamageAbsorptionLegs / 100);

        Debug.Log("Total Fire Damage Absorption: " + totalFireDamageAbsorption + "%");

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

        float finalDamage = physicalDamage + fireDamage;

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        Debug.Log("Final Damage Dealt is: " + finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        if (isDead)
            return;

        //PHYSIC

        float totalPhysicalDamageAbsorption = 1 -
        (1 - physicalDamageAbsorptionHead / 100) *
        (1 - physicalDamageAbsorptionBody / 100) *
        (1 - physicalDamageAbsorptionHands / 100) *
        (1 - physicalDamageAbsorptionLegs / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

        //FIRE
        
        float totalFireDamageAbsorption = 1 -
        (1 - fireDamageAbsorptionHead / 100) *
        (1 - fireDamageAbsorptionBody / 100) *
        (1 - fireDamageAbsorptionHands / 100) *
        (1 - fireDamageAbsorptionLegs / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

        float finalDamage = physicalDamage + fireDamage;

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void TakePoisonDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }

}
