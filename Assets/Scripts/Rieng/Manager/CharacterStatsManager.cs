using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Team I.D")]
    public int teamIDNumber = 0;
    public int maxHealth;
    public int currentHealth;

    public float maxStamina;
    public float currentStamina;

    public float maxFocusPoint;
    public float currentFocusPoint;

    public int currentShadesCount = 0;
    public int shadesAwardedOnDeath = 100;

    [Header("Player Level")]
    public int playerLevel = 1;

    [Header("Stats Levels")]
    public int healthLevel = 10;
    public int staminaLevel = 10;
    public int focusLevel = 10;
    public int poiseLevel = 10;
    public int strengthLevel = 10;
    public int dexterityLevel = 10;
    public int intelligenceLevel = 10;
    public int faithLevel = 10;


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

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    private void Start()
    {
        totalPoiseDefence = armorPoiseBonus;
    }


    public int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public float SetMaxFocusPointFromFocusLevel()
    {
        maxFocusPoint = focusLevel * 10;
        return maxFocusPoint;
    }


    public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
    {
        if (character.isDead)
            return;

        character.characterAnimatorManager.EraseHandIKForWeapon();
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
            character.isDead = true;
        }

        character.characterSoundFXManager.PlayRandomDamageSoundFX();
    }

    public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        if (character.isDead)
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
            character.isDead = true;
        }

        character.characterSoundFXManager.PlayRandomDamageSoundFX();

    }

    public virtual void TakePoisonDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            character.isDead = true;
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

    public virtual void DrainStaminaBasedOnAttackType()
    {

    }

}
